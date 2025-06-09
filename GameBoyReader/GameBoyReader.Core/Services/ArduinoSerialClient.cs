using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.Core.Services
{
    public class ArduinoSerialClient : IArduinoSerialClient
    {
        public SerialPort? serialPort;
        public string[] RetrieveAvailableCOMPorts()
        {
            return SerialPort.GetPortNames();
        }

        public List<byte> RetrieveBytes(string comPort, string readerCommand)
        {
            List<byte> readResult = new();
            using (SerialPort serialPort = new SerialPort(comPort, 115200))
            {
                serialPort.ReadTimeout = 500; // 500 ms
                serialPort.Open();
                Thread.Sleep(1000);
                serialPort.WriteLine(readerCommand);

                try
                {
                    StringBuilder textBuffer = new();
                    Queue<byte> byteBuffer = new();

                    bool foundHeaderStart = false;

                    while (true)
                    {
                        int result = serialPort.ReadByte();
                        byte b = (byte)result;

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

                                for (int i = 0; i < skipCount; i++)
                                    byteBuffer.Dequeue();
                            }

                            continue;
                        }

                        readResult.Add(b);

                        Thread.Sleep(10);
                    }

                }
                catch (TimeoutException)
                {
                }
                catch (IOException ex)
                {
                    Console.WriteLine("IO błąd: " + ex.Message);
                }
            }

            return removeSuffix(readResult);
        }


        private List<Byte> removeSuffix(List<Byte> readBytes)
        {
            byte[] headerEndBytes = Encoding.ASCII.GetBytes("END\r\n");
            if (readBytes.Count >= headerEndBytes.Length)
            {
                bool endsWithHeaderEnd = true;
                for (int i = 0; i < headerEndBytes.Length; i++)
                {
                    if (readBytes[readBytes.Count - headerEndBytes.Length + i] != headerEndBytes[i])
                    {
                        endsWithHeaderEnd = false;
                        break;
                    }
                }
                if (endsWithHeaderEnd)
                {
                    readBytes.RemoveRange(readBytes.Count - headerEndBytes.Length, headerEndBytes.Length);
                }
            }

            return readBytes;
        }

    }
}
