using GameBoyReader.Core.Enums;
using GameBoyReader.Core.Models;

namespace GameBoyReader.Core.Services
{
    public interface IArduinoSerialClient
    {
        public string[] RetrieveAvailableCOMPorts();
        public List<Byte> RetrieveBytes(string comPort, string readerCommand);
    }
}
