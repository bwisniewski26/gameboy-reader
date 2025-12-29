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
                await COMPortPicker.TerminalCOMPortPicker();
            }

            CartridgeContent content = await dumperService.DumpCartridge(null);


            Console.Clear();
            Console.WriteLine($"Received {content.CartridgeByteContent.Count / 1024}KB");
            Console.WriteLine("Choose filename for dumped data: ");
            string? path = Console.ReadLine();
            while (true)
            {
                int fileNameIssues = 0;
                if (path == null) path = "";
                if (path != null && path.Length > 32)
                {
                    Console.WriteLine("File name cannot be longer than 32 characters");
                    fileNameIssues++;
                } 
                if (path.Length == 0)
                {
                    Console.WriteLine("File name cannot be empty.");
                    fileNameIssues++;
                } 
                if (File.Exists(Path.Combine(RetrieveSaveLocation(DumpType.ROM), path)))
                {
                    Console.WriteLine("File of that name already exists");
                    fileNameIssues++;
                }
                if (DoesContainForbiddenCharacters(path))
                {
                    Console.WriteLine("Provided file name is invalid. Please check if it contains forbidden characters or file of this name exists. Illegal characters: ");
                string visibleIllegalCharacters = new string(
                    Path.GetInvalidFileNameChars()
                        .Where(c => !char.IsControl(c))
                        .ToArray());
                    Console.WriteLine(visibleIllegalCharacters);
                    fileNameIssues++;
                }
                if (fileNameIssues == 0)
                {
                    break;
                }
                Console.WriteLine("Please choose new filename: ");
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
        private static bool DoesContainForbiddenCharacters(string name)
        {
            return name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
        }

        public static async Task DumpCartridgeRAM()
        {
            if (!ConnectionService.IsConnectionEstablished)
            {
                await COMPortPicker.TerminalCOMPortPicker();
            }

            CartridgeRAMContent content = await dumperService.DumpCartridgeRAM(null);


            Console.Clear();
            Console.WriteLine($"Received {content.CartridgeRAMByteContent.Count / 1024}KB");
            Console.WriteLine("Choose filename for dumped data: ");
            string? path = Console.ReadLine();
            while (true)
            {
                int fileNameIssues = 0;
                if (path == null) path = "";
                if (path != null && path.Length > 32)
                {
                    Console.WriteLine("File name cannot be longer than 32 characters");
                    fileNameIssues++;
                }
                if (path.Length == 0)
                {
                    Console.WriteLine("File name cannot be empty.");
                    fileNameIssues++;
                }
                if (File.Exists(Path.Combine(RetrieveSaveLocation(DumpType.RAM), path)))
                {
                    Console.WriteLine("File of that name already exists");
                    fileNameIssues++;
                }
                if (DoesContainForbiddenCharacters(path))
                {
                    Console.WriteLine("Provided file name is invalid. Please check if it contains forbidden characters or file of this name exists. Illegal characters: ");
                    string visibleIllegalCharacters = new string(
                        Path.GetInvalidFileNameChars()
                            .Where(c => !char.IsControl(c))
                            .ToArray());
                    Console.WriteLine(visibleIllegalCharacters);
                    fileNameIssues++;
                }
                if (fileNameIssues == 0)
                {
                    break;
                }
                Console.WriteLine("Please choose new filename: ");
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
