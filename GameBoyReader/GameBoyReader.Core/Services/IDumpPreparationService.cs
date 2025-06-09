using GameBoyReader.Core.Models;

namespace GameBoyReader.Core.Services
{
    public interface IDumpPreparationService
    {
        public bool ValidateBootBitmap(string comPort);
        public CartridgeInformation RetrieveCartridgeInformation(string comPort);
        
    }
}
