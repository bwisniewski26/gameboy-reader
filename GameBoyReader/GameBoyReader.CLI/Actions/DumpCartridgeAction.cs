using GameBoyReader.Core.Models;
using GameBoyReader.Core.Services;
using GameBoyReader.Core.Utils;
using GameBoyReader.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBoyReader.Core.States;

namespace GameBoyReader.CLI.Actions
{
    public class DumpCartridgeAction
    {
        private static CartridgeDumperService dumperService = new();
        public static async Task DumpCartridge()
        {
            if (!ConnectionStatus.IsConnectionEstablished)
            {
                COMPortPicker.TerminalCOMPortPicker();
            }

            CartridgeContent content = await dumperService.DumpCartridge();


            Console.Clear();
            Console.WriteLine($"Received {content.CartridgeByteContent.Count / 1024}KB");
            Console.WriteLine("Choose path to save your dumped file: ");
            string path = Console.ReadLine();

            try
            {
                File.WriteAllBytes(path, content.CartridgeByteContent.ToArray());
                Console.WriteLine($"File saved successfully: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while saving file:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
