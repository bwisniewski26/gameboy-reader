using GameBoyReader.Core.Enums;

namespace GameBoyReader.Core.Services
{
    internal interface ICartridgeByteContentService
    {
        public string RetrieveGameName(byte[] byteContent);
        public CartridgeType RetrieveCartridgeType(byte[] byteContent);
        public bool CalculateHeaderChecksum(List<byte> byteContent);
    }
}
