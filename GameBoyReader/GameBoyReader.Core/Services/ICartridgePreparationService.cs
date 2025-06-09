using GameBoyReader.Core.Models;

namespace GameBoyReader.Core.Services
{
    public interface ICartridgePreparationService
    {
        public Task<bool> ValidateBootBitmap(string comPort);
        public Task<CartridgeInformation> RetrieveCartridgeInformation(string comPort);
        
    }
}
