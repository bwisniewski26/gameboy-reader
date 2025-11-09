using GameBoyReader.Core.Enums;

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
