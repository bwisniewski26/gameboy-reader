using GameBoyReader.Core.Models;

namespace GameBoyReader.Core.Services
{
    public interface ICartridgePreparationService
    {
        public Task<RetrievedBitmap> ValidateBootBitmap(string? comPort = null);
        public Task<CartridgeInformation> RetrieveCartridgeInformation(string? comPort = null);
        
    }
}
