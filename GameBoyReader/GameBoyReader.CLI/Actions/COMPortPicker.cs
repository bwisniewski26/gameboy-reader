using GameBoyReader.Core.Services;
using GameBoyReader.Core.States;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.CLI.Actions
{
    public static class COMPortPicker
    {
        private static ArduinoSerialClient arduinoClient = new();
        public static async void TerminalCOMPortPicker()
        {
            var options = arduinoClient.RetrieveAvailableCOMPorts();
            options.Add("Back");
            int selectedIndex = 0;
            while (true)
            {
                ConsoleKey key;
                string title = "Choose COM port";


                do
                {
                    int windowWidth = Console.WindowWidth;
                    int x = (windowWidth - title.Length) / 2;
                    Console.Clear();
                    Console.SetCursorPosition(x, 0);
                    Console.WriteLine(title);
                    for (int i = 0; i < options.Count; i++)
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
                            selectedIndex = options.Count - 1;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        selectedIndex++;
                        if (selectedIndex >= options.Count)
                            selectedIndex = 0;
                    }

                } while (key != ConsoleKey.Enter);

                Console.Clear();
                if (selectedIndex != options.Count - 1)
                {
                    try
                    {
                        await ConnectionStatus.StartConnection(options[selectedIndex]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("COM connection was unsuccessful:");
                        Console.WriteLine(e.Message);
                    }
                }
                return;
            }
        }
    }
}
