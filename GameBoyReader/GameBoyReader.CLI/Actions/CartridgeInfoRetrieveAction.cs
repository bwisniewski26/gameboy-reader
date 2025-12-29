using GameBoyReader.Core.Models;
using GameBoyReader.Core.Services;
using GameBoyReader.Core.Utils;

namespace GameBoyReader.CLI.Actions
{

    public class CartridgeInfoRetrieveAction
    {
        private static CartridgePreparationService cartridgeService = new();
        public async static Task DisplayCartridgeInfo()
        {
            if (!ConnectionService.IsConnectionEstablished)
            {
                await COMPortPicker.TerminalCOMPortPicker();
            }
            CartridgeInformation information = await cartridgeService.RetrieveCartridgeInformation();

            Console.Clear();

            Console.WriteLine($"Game title: {information.Name}");
            Console.WriteLine($"Cartridge type: {CartridgeTypeConverter.ConvertFromCartridgeTypeToString(information.Type)}");
            Console.WriteLine($"ROM Size: {32 * 1024 * ( 1 << information.ROMSize)}B");
            Console.WriteLine($"RAM Size: {information.RAMSize}");
        }
    }
}
