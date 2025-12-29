using System.IO.Ports;

namespace GameBoyReader.Core.Services
{
    public static class ConnectionService
    {
        public static SerialPort? SerialPort = null;
        public static bool IsConnectionEstablished = false;
        public static ArduinoSerialClient serialClient = new();

        public static async Task StartConnection(string comPort)
        {
            SerialPort = new SerialPort(comPort, 115200);
            SerialPort.ReadBufferSize = 65536;
            SerialPort.WriteBufferSize = 4096;
            SerialPort.Open();
            Thread.Sleep(1500);
            try
            {
                var result = await serialClient.RetrieveBytes("PING");
                IsConnectionEstablished = true;
            } catch (Exception ex)
            {
                Console.WriteLine("Error has occured while establishing connection. Error details:");
                Console.WriteLine(ex.Message);
                IsConnectionEstablished = false;
                SerialPort.Close();
            }
        }

        public static void StopConnection()
        {
            if (SerialPort != null)
            {
                SerialPort.Close();
            }
            SerialPort = null;
            IsConnectionEstablished = false;
        }
    }
}
