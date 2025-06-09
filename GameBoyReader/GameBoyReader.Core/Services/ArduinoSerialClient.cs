using GameBoyReader.Core.Exceptions;
using GameBoyReader.Core.States;
using System.IO.Ports;
using System.Text;

namespace GameBoyReader.Core.Services
{
    public class ArduinoSerialClient : IArduinoSerialClient
    {
        public string[] RetrieveAvailableCOMPorts()
        {
            return SerialPort.GetPortNames();
        }

        public async Task<List<Byte>> RetrieveBytes(string readerCommand)
        {
            if (ConnectionStatus.SerialPort == null)
                throw new SerialConnectionException();

            List<byte> readResult = new();
            ConnectionStatus.SerialPort.ReadTimeout = 1000;
            await Task.Delay(200);
            ConnectionStatus.SerialPort.WriteLine(readerCommand);

            const int bufferSize = 256;
            byte[] buffer = new byte[bufferSize];
            Queue<byte> byteBuffer = new();
            StringBuilder textBuffer = new();
            bool foundHeaderStart = false;

            while (true)
            {
                int bytesRead;
                try
                {
                    bytesRead = await Task.Run(() => ConnectionStatus.SerialPort.Read(buffer, 0, bufferSize));
                }
                catch (TimeoutException)
                {
                    break;
                }

                if (bytesRead == 0)
                    break;

                for (int i = 0; i < bytesRead; i++)
                {
                    byte b = buffer[i];
                    byteBuffer.Enqueue(b);
                    textBuffer.Append((char)b);

                    if (!foundHeaderStart)
                    {
                        string currentText = textBuffer.ToString();
                        if (currentText.Contains("START\r\n"))
                        {
                            foundHeaderStart = true;
                            int headerEndIndex = currentText.IndexOf("START\r\n") + "START\r\n".Length;
                            int skipCount = Encoding.ASCII.GetByteCount(currentText[..headerEndIndex]);

                            for (int j = 0; j < skipCount; j++)
                                byteBuffer.Dequeue();
                        }
                        continue;
                    }

                    readResult.Add(b);

                    if (readResult.Count >= 5 &&
                        readResult[^5] == 'E' &&
                        readResult[^4] == 'N' &&
                        readResult[^3] == 'D' &&
                        readResult[^2] == '\r' &&
                        readResult[^1] == '\n')
                    {
                        readResult.RemoveRange(readResult.Count - 5, 5);
                        return readResult;
                    }
                }
            }

            return readResult;
        }


    }
}
