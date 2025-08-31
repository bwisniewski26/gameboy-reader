using GameBoyReader.Core.Services;
using GameBoyReader.Core.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.CLI.Actions
{
    internal static class SaveWriterAction
    {
        private static CartridgePreparationService _preparationService = new();
        private static CartridgeDumperService _dumperService = new();
        public static async Task WriteSaveAction()
        {
            if (!ConnectionStatus.IsConnectionEstablished)
            {
                COMPortPicker.TerminalCOMPortPicker();
            }
            int requiredFileSize = await _preparationService.RetrieveRAMSize();
            bool transferComplete = false;

            while (!transferComplete) {
                Console.Clear();
                Console.WriteLine("Choose file to be written to cartridge, leave empty to return to previous menu: ");
                string path = Console.ReadLine();
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
