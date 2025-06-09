using GameBoyReader.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.Core.Models
{
    public class CartridgeInformation
    {
        public string Name { get; set; } = "";
        public CartridgeType Type { get; set; }
        public Byte ROMSize { get; set; }
        public Byte RAMSize { get; set; }
    }
}
