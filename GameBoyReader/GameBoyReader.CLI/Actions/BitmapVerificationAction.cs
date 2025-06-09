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
        private static DumpPreparationService cartridgeService = new();
        public static bool VerifyBitmap()
        {
            string comPort = COMPortPicker.TerminalCOMPortPicker();
            return cartridgeService.ValidateBootBitmap(comPort);
        }

    }
}
