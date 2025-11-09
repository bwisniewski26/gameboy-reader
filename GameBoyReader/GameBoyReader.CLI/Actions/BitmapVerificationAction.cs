using GameBoyReader.Core.Models;
using GameBoyReader.Core.Services;

namespace GameBoyReader.CLI.Actions
{
    public static class BitmapVerificationAction
    {
        private static CartridgePreparationService cartridgeService = new();
        public async static Task VerifyBitmap()
        {
            if (!ConnectionService.IsConnectionEstablished)
            {
                COMPortPicker.TerminalCOMPortPicker();
            }
            RetrievedBitmap result = await cartridgeService.ValidateBootBitmap();
            Console.WriteLine("");
            Console.WriteLine("Received data:");
            foreach (var bit in result.Bitmap)
            {
                Console.Write(bit);
            }
            Console.WriteLine("\nExpected data:");
            foreach (var bit in CartridgeValidationBitmap.bootBitmap)
            {
                Console.Write(bit);
            }
            if (result.IsBitmapCorrect)
            {
                Console.WriteLine("\nBoot bitmap was verified successfully. This means that cartridge is a correct GameBoy cartridge.");
            }
            else
            {
                Console.WriteLine("Received boot bitmap was incorrect.");
            }
        }

    }
}
