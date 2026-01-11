using GameBoyReader.Core.Enums;
using GameBoyReader.Core.Models;

namespace GameBoyReader.Core.Services
{
    public interface ICartridgePreparationService
    {
        public Task<RetrievedBitmap> ValidateBootBitmap(string? comPort = null);
        public Task<CartridgeInformation> RetrieveCartridgeInformation(string? comPort = null);

        public Task<int> RetrieveRAMSize();

        public Task<CartridgeType> RetrieveCartridgeType();

        public Task<bool> VerifyIfRAMPresent();
        
    }
}
