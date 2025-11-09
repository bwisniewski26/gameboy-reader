using GameBoyReader.Core.Models;
using GameBoyReader.Core.Services;

namespace GameBoyReader.CLI.Actions
{
    public class DumpCartridgeAction
    {
        private enum DumpType
        {
            RAM,
            ROM
        }
        private static CartridgeDumperService dumperService = new();
        public static async Task DumpCartridge()
        {
            if (!ConnectionService.IsConnectionEstablished)
            {
                COMPortPicker.TerminalCOMPortPicker();
            }

            CartridgeContent content = await dumperService.DumpCartridge(null);


            Console.Clear();
            Console.WriteLine($"Received {content.CartridgeByteContent.Count / 1024}KB");
            Console.WriteLine("Choose filename for dumped data: ");
            string? path = Console.ReadLine();
            while (path == null || File.Exists(Path.Combine(RetrieveSaveLocation(DumpType.ROM), path)))
            {
                Console.WriteLine("Invalid name. Input was empty or file of this name already exists. Choose another name: ");
                path = Console.ReadLine();
            }
            if (!path.EndsWith(".gb"))
            {
                path += ".gb";
            }
            try
            {
                File.WriteAllBytes(Path.Combine(RetrieveSaveLocation(DumpType.ROM), path), content.CartridgeByteContent.ToArray());
                Console.WriteLine($"File saved successfully: {Path.Combine(RetrieveSaveLocation(DumpType.ROM), path)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while saving file:");
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task DumpCartridgeRAM()
        {
            if (!ConnectionService.IsConnectionEstablished)
            {
                COMPortPicker.TerminalCOMPortPicker();
            }

            CartridgeRAMContent content = await dumperService.DumpCartridgeRAM(null);


            Console.Clear();
            Console.WriteLine($"Received {content.CartridgeRAMByteContent.Count / 1024}KB");
            Console.WriteLine("Choose filename for dumped data: ");
            string? path = Console.ReadLine();
            while (path == null || File.Exists(Path.Combine(RetrieveSaveLocation(DumpType.RAM), path)))
            {
                Console.WriteLine("Invalid name. Input was empty or file of this name already exists. Choose another name: ");
                path = Console.ReadLine();
            }
            if (!path.EndsWith(".sav"))
            {
                path += ".sav";
            }
            try
            {
                File.WriteAllBytes(Path.Combine(RetrieveSaveLocation(DumpType.RAM), path), content.CartridgeRAMByteContent.ToArray());
                Console.WriteLine($"File saved successfully: {Path.Combine(RetrieveSaveLocation(DumpType.RAM), path)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while saving file:");
                Console.WriteLine(ex.Message);
            }
        }

        private static string RetrieveSaveLocation(DumpType type)
        {
            string result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GameBoyReader");
            if (type == DumpType.RAM)
            {
                Path.Combine(result, "Saves");
            }
            else
            {
                Path.Combine(result, "ROMs");
            }
            return result;
        }
    }
}
