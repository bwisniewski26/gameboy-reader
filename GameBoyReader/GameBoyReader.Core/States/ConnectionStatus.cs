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

        public static Task StartConnection(string comPort)
        {
            SerialPort = new SerialPort(comPort, 115200);
            SerialPort.ReadBufferSize = 65536;
            SerialPort.WriteBufferSize = 4096;
            SerialPort.Open();
            IsConnectionEstablished = true;
            return Task.CompletedTask;
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
