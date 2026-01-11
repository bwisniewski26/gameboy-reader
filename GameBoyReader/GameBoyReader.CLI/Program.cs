using GameBoyReader.Core.Services;
using GameBoyReader.CLI.Actions;

namespace GameBoyReader.CLI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GameBoyReader")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GameBoyReader"));
            }

            int selectedIndex = 0;
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Console.WriteLine("CTRL+C hotkey received. This app will shutdown. Cleaning up...");
                OnProcessExit(sender, eventArgs);
            };

            while (true)
            {
                selectedIndex = TerminalPicker();
                Console.Clear();
                switch (selectedIndex)
                {
                    case 0:
                        await COMPortPicker.TerminalCOMPortPicker();
                        Console.WriteLine("Press any button to return.");
                        Console.ReadKey();
                        break;
                    case 1:
                        await BitmapVerificationAction.VerifyBitmap();
                        Console.WriteLine("Press any button to return.");
                        Console.ReadKey();
                        break;
                    case 2:
                        await CartridgeInfoRetrieveAction.DisplayCartridgeInfo();
                        Console.WriteLine("Press any button to return.");
                        Console.ReadKey();
                        break;
                    case 3:
                        await DumpCartridgeAction.DumpCartridge();
                        Console.WriteLine("Press any button to return.");
                        Console.ReadKey();
                        break;
                    case 4:
                        await DumpCartridgeAction.DumpCartridgeRAM();
                        Console.WriteLine("Press any button to return.");
                        Console.ReadKey();
                        break;
                    case 5:
                        await SaveWriterAction.WriteSaveAction();
                        Console.WriteLine("Press any button to return.");
                        Console.ReadKey();
                        break;
                    case 6:
                        return;
                }
            }

        }

        static void OnProcessExit(object? sender, EventArgs e)
        {
            Console.WriteLine("Exiting GameBoy Reader...");
            if (ConnectionService.SerialPort != null)
            {
                Console.WriteLine("Closing port...");
                ConnectionService.SerialPort.Close();
            }
        }

        static int TerminalPicker()
        {
            string[] options = { "Choose COM port", "Check boot bitmap", "Check cartridge header", "Dump cartridge", "Dump cartridge RAM","Write RAM to cartridge", "Exit" };
            int selectedIndex = 0;
            while (true)
            {
                ConsoleKey key;
                string title = "GameBoy Reader CLI";

                do
                {
                    int windowWidth = Console.WindowWidth;
                    int x = (windowWidth - title.Length) / 2;
                    Console.Clear();
                    Console.SetCursorPosition(x, 0);
                    Console.WriteLine(title);
                    Console.WriteLine($"Is COM connection establised: {ConnectionService.IsConnectionEstablished}");
                    for (int i = 0; i < options.Length; i++)
                    {
                        if (i == selectedIndex)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.ResetColor();
                        }

                        Console.WriteLine(options[i]);
                    }
                    Console.ResetColor();

                    var keyInfo = Console.ReadKey(true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.UpArrow)
                    {
                        selectedIndex--;
                        if (selectedIndex < 0)
                            selectedIndex = options.Length - 1;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        selectedIndex++;
                        if (selectedIndex >= options.Length)
                            selectedIndex = 0;
                    }

                } while (key != ConsoleKey.Enter);

                return selectedIndex;
            }
        }
    }
}
