using GameBoyReader.Core.Services;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.Core.States
{
    public static class ConnectionStatus
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
            try
            {
                var result = await serialClient.RetrieveBytes("PING");
                IsConnectionEstablished = true;
            } catch (Exception ex)
            {
                Console.WriteLine("Error has occured while establishing connection. Error details:");
                Console.WriteLine(ex.Message);
                IsConnectionEstablished = false;
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
