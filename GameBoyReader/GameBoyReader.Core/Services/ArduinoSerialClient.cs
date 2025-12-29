using GameBoyReader.Core.Exceptions;
using System.IO.Ports;
using System.Text;

namespace GameBoyReader.Core.Services
{
    public class ArduinoSerialClient : IArduinoSerialClient
    {
        public List<string> RetrieveAvailableCOMPorts()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public async Task<List<Byte>> RetrieveBytes(string readerCommand)
        {
            if (ConnectionService.SerialPort == null)
                throw new SerialConnectionException();

            List<byte> readResult = new();
            ConnectionService.SerialPort.ReadTimeout = 1000;
            await Task.Delay(200);
            ConnectionService.SerialPort.WriteLine(readerCommand);

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
                    bytesRead = await Task.Run(() => ConnectionService.SerialPort.Read(buffer, 0, bufferSize));
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

        public async Task SendBytes(string readerCommand, byte[] bytes)
        {
            if (ConnectionService.SerialPort == null)
                throw new SerialConnectionException();

            ConnectionService.SerialPort.ReadTimeout = 1000;

            await Task.Delay(200);
            ConnectionService.SerialPort.WriteLine(readerCommand);
            await Task.Delay(200);

            ConnectionService.SerialPort.Write("START");
            await Task.Delay(200);

            int blockSize = 1;
            for (int i = 0; i < bytes.Length; i += blockSize)
            {
                int len = Math.Min(blockSize, bytes.Length - i);
                ConnectionService.SerialPort.Write(bytes, i, 1);
            }

            await Task.Delay(200);
            ConnectionService.SerialPort.Write("END");
        }
    }
}
