namespace GameBoyReader.Core.Services
{
    public interface IArduinoSerialClient
    {
        public List<string> RetrieveAvailableCOMPorts();
        public Task<List<Byte>> RetrieveBytes(string readerCommand);

        public Task SendBytes(string readerCommand, byte[] bytes);
    }
}
