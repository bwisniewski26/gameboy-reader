using GameBoyReader.Core.Enums;
using GameBoyReader.Core.Exceptions;
using GameBoyReader.Core.Models;
using GameBoyReader.Core.States;

namespace GameBoyReader.Core.Services
{
    public class CartridgeDumperService : ICartridgeDumperService
    {
        private ArduinoSerialClient _serialClient = new();
        private CartridgePreparationService _preparationService = new();
        public async Task<CartridgeContent> DumpCartridge(string? comPort = null)
        {
            CartridgeInformation information = new CartridgeInformation();
            CartridgeContent cartridgeContent = new CartridgeContent();

            try
            {
                if (!ConnectionStatus.IsConnectionEstablished)
                {
                    if (comPort == null)
                    {
                        throw new SerialConnectionException();
                    }
                    await ConnectionStatus.StartConnection(comPort);
                    await Task.Delay(500);
                }
                cartridgeContent.CartridgeInformation = await _preparationService.RetrieveCartridgeInformation(comPort);
                switch (cartridgeContent.CartridgeInformation.Type)
                {
                    case CartridgeType.MBC0:
                        cartridgeContent.CartridgeByteContent = await _serialClient.RetrieveBytes("DUMP_MBC0");
                        break;
                    case CartridgeType.MBC1:
                        cartridgeContent.CartridgeByteContent = await _serialClient.RetrieveBytes("DUMP_MBC1");
                        break;
                    default:
                        throw new CartridgeNotSupportedException();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured. Received data may be incorrect. Error details:");
                Console.WriteLine(e);
            }

            return cartridgeContent;
        }
    }
}
