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

    public class CartridgeInfoRetrieve
    {
        private static DumpPreparationService cartridgeService = new();
        public static void DisplayCartridgeInfo()
        {
            string comPort = COMPortPicker.TerminalCOMPortPicker();
            CartridgeInformation information = cartridgeService.RetrieveCartridgeInformation(comPort);

            Console.Clear();

            Console.WriteLine($"Game title: {information.Name}");
            Console.WriteLine($"Cartridge type: {CartridgeTypeConverter.ConvertFromCartridgeTypeToString(information.Type)}");
            Console.WriteLine($"ROM Size: {information.ROMSize}");
            Console.WriteLine($"RAM Size: {information.RAMSize}");
        }
    }
}
