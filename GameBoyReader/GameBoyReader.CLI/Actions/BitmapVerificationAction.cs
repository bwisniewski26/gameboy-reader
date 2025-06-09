using GameBoyReader.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.CLI.Actions
{
    public static class BitmapVerificationAction
    {
        private static CartridgePreparationService cartridgeService = new();
        public async static Task VerifyBitmap()
        {
            string comPort = COMPortPicker.TerminalCOMPortPicker();
            bool result = await cartridgeService.ValidateBootBitmap(comPort);
            Console.WriteLine("");
            if (result)
            {
                Console.WriteLine("Boot bitmap was verified successfully. This means that cartridge is a correct GameBoy cartridge.");
            }
            else
            {
                Console.WriteLine("Received boot bitmap was incorrect.");
            }
        }

    }
}
