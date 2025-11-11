using GameBoyReader.Core.Enums;
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
        public CartridgeType cartridgeType;
        public string path;

        public DumpFile(string title, CartridgeType cartridgeType, string path)
        {
            this.title = title;
            this.cartridgeType = cartridgeType;
            this.path = path;
        }
    }
}
