using GameBoyReader.Core.Models;
using GameBoyReader.Core.Utils;
using System.Text;

namespace GameBoyReader.Core.Services
{
    public class DumpPreparationService : IDumpPreparationService
    {

        private ArduinoSerialClient arduinoClient = new();
        public CartridgeInformation RetrieveCartridgeInformation(string comPort)
        {
            CartridgeInformation information = new();
            try
            {
                information.Name = Encoding.ASCII.GetString(arduinoClient.RetrieveBytes(comPort, "GET_TITLE").ToArray());
                information.Type = CartridgeTypeConverter.ConvertFromByte(arduinoClient.RetrieveBytes(comPort, "GET_MBC").First());
                information.ROMSize = arduinoClient.RetrieveBytes(comPort, "GET_ROM_SIZE").First();
                information.RAMSize = arduinoClient.RetrieveBytes(comPort, "GET_RAM_SIZE").First();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured. Received data may be incorrect. Error details:");
                Console.WriteLine(e);
            }
            return information;
        }

        public bool ValidateBootBitmap(string comPort)
        {
            List<Byte> bitmap = new();
            try
            {
                bitmap = arduinoClient.RetrieveBytes(comPort, "GET_HEADER");
                
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }

            foreach (var b in bitmap)
            {
                Console.Write(b + " ");
            }
            return bitmap.SequenceEqual(CartridgeValidationBootBitmap.bootBitmap);
        }

    }
}
