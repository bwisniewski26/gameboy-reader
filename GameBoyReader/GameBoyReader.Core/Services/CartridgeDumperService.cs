using GameBoyReader.Core.Enums;
using GameBoyReader.Core.Exceptions;
using GameBoyReader.Core.Models;

namespace GameBoyReader.Core.Services
{
    public class CartridgeDumperService : ICartridgeDumperService
    {
        private ArduinoSerialClient _serialClient = new();
        private CartridgePreparationService _preparationService = new();
        public async Task<CartridgeContent> DumpCartridge(string? comPort = null)
        {
            CartridgeContent cartridgeContent = new CartridgeContent();

            try
            {
                if (!ConnectionService.IsConnectionEstablished)
                {
                    if (comPort == null)
                    {
                        throw new SerialConnectionException();
                    }
                    await ConnectionService.StartConnection(comPort);
                    await Task.Delay(500);
                }
                cartridgeContent.CartridgeInformation = await _preparationService.RetrieveCartridgeInformation(comPort);
                cartridgeContent.CartridgeByteContent = await _serialClient.RetrieveBytes("DUMP_ROM");
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured. Received data may be incorrect. Error details:");
                Console.WriteLine(e);
            }

            return cartridgeContent;
        }

        public async Task<CartridgeRAMContent> DumpCartridgeRAM(string? comPort = null)
        {
            CartridgeRAMContent cartridgeRAMContent = new CartridgeRAMContent();

            try
            {
                if (!ConnectionService.IsConnectionEstablished)
                {
                    if (comPort == null)
                    {
                        throw new SerialConnectionException();
                    }
                    await ConnectionService.StartConnection(comPort);
                    await Task.Delay(500);
                }
                cartridgeRAMContent.CartridgeRAMByteContent = await _serialClient.RetrieveBytes("DUMP_RAM");
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured. Received data may be incorrect. Error details:");
                Console.WriteLine(e);
            }

            return cartridgeRAMContent;
        }

        public async Task WriteCartridgeRAM(byte[] bytes, string? comPort = null)
        {
            try
            {
                if (!ConnectionService.IsConnectionEstablished)
                {
                    if (comPort == null)
                    {
                        throw new SerialConnectionException();
                    }
                    await ConnectionService.StartConnection(comPort);
                    await Task.Delay(500);
                }
                await Task.Delay(500);
                await _serialClient.SendBytes("WRITE_RAM", bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured. Received data may be incorrect. Error details:");
                Console.WriteLine(e);
            }
        }
    }
}
