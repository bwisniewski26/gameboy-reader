using GameBoyReader.Core.Models;
using GameBoyReader.Core.Services;
using GameBoyReader.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.CLI.Actions
{

    public class CartridgeInfoRetrieveAction
    {
        private static CartridgePreparationService cartridgeService = new();
        public async static Task DisplayCartridgeInfo()
        {
            string comPort = COMPortPicker.TerminalCOMPortPicker();
            CartridgeInformation information = await cartridgeService.RetrieveCartridgeInformation(comPort);

            Console.Clear();

            Console.WriteLine($"Game title: {information.Name}");
            Console.WriteLine($"Cartridge type: {CartridgeTypeConverter.ConvertFromCartridgeTypeToString(information.Type)}");
            Console.WriteLine($"ROM Size: {32 * 1024 * ( 1 << information.ROMSize)}KB");
            Console.WriteLine($"RAM Size: {information.RAMSize}");
        }
    }
}
