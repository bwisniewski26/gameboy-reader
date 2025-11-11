using GameBoyReader.Core.Enums;
using GameBoyReader.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.GUI.Models
{
    public class DumpFile
    {
        public string title;
        public string cartridgeType;
        public string path;

        public DumpFile(string title, CartridgeType cartridgeType, string path)
        {
            this.title = title;
            this.cartridgeType = CartridgeTypeConverter.ConvertFromCartridgeTypeToString(cartridgeType);
            this.path = path;
        }

        public DumpFile()
        {
            title = "";
            path = "";
            cartridgeType = "Unsupported";
        }
    }
}
