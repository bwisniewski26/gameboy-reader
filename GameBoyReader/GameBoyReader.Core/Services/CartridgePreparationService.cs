using GameBoyReader.Core.Exceptions;
using GameBoyReader.Core.Models;
using GameBoyReader.Core.States;
using GameBoyReader.Core.Utils;
using System.Text;

namespace GameBoyReader.Core.Services
{
    public class CartridgePreparationService : ICartridgePreparationService
    {

        private ArduinoSerialClient arduinoClient = new();
        public async Task<CartridgeInformation> RetrieveCartridgeInformation(string? comPort = null)
        {
            CartridgeInformation information = new();
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

                var titleBytes = await arduinoClient.RetrieveBytes("GET_TITLE");
                information.Name = Encoding.ASCII.GetString(titleBytes.ToArray());

                var mbcBytes = await arduinoClient.RetrieveBytes("GET_MBC");
                information.Type = CartridgeTypeConverter.ConvertFromByte(mbcBytes.First());

                var romSizeBytes = await arduinoClient.RetrieveBytes("GET_ROM_SIZE");
                information.ROMSize = romSizeBytes.First();

                var ramSizeBytes = await arduinoClient.RetrieveBytes("GET_RAM_SIZE");
                information.RAMSize = ramSizeBytes.First();

            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured. Received data may be incorrect. Error details:");
                Console.WriteLine(e);
            }
            return information;
        }

        public async Task<RetrievedBitmap> ValidateBootBitmap(string? comPort = null)
        {
            RetrievedBitmap retrievedBitmap = new();
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
                retrievedBitmap.Bitmap = await arduinoClient.RetrieveBytes("GET_HEADER");
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured. Received data may be incorrect. Error details:");
                Console.WriteLine(e);
            }
            retrievedBitmap.IsBitmapCorrect = retrievedBitmap.Bitmap.SequenceEqual(CartridgeValidationBitmap.bootBitmap);

            return retrievedBitmap;
        }

    }
}
