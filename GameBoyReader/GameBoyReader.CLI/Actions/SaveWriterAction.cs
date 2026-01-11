using GameBoyReader.Core.Services;

namespace GameBoyReader.CLI.Actions
{
    internal static class SaveWriterAction
    {
        private static CartridgePreparationService _preparationService = new();
        private static CartridgeDumperService _dumperService = new();
        public static async Task WriteSaveAction()
        {
            if (!ConnectionService.IsConnectionEstablished)
            {
               await COMPortPicker.TerminalCOMPortPicker();
            }
            int requiredFileSize = await _preparationService.RetrieveRAMSize();
            bool transferComplete = false;
            Console.WriteLine("Please wait while program checks if inserted cartridge has built-in RAM...");
            bool isRAMPresent = await _preparationService.VerifyIfRAMPresent();
            if (!isRAMPresent)
            {
                transferComplete = true;
                Console.WriteLine("Inserted cartridge does not have RAM memory.");
                return;
            }
            while (!transferComplete) {
                Console.Clear();
                Console.WriteLine("Choose file to be written to cartridge, leave empty to return to previous menu: ");
                string? path = Console.ReadLine();
                if (path == null || path == "")
                {
                    return;
                }
                while (!File.Exists(path))
                {
                    Console.WriteLine("File doesn't exist. Please provide full path to chosen file.");
                    path = Console.ReadLine();
                    if (path == null || path == "")
                    {
                        return;
                    }
                }
                try
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    if (bytes.Length / 1024 != requiredFileSize)
                    {
                        Console.WriteLine($"Expected: {requiredFileSize}, received: {bytes.Length / 1024}");
                        Console.WriteLine("Incorrect file size.");
                    } else
                    {
                        await _dumperService.WriteCartridgeRAM(bytes);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error has occured. Error details: ");
                    Console.WriteLine($"{ex.Message}");
                    return;
                }
                transferComplete = true;
                return;
            }
        }
    }
}
