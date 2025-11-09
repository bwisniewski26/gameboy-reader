namespace GameBoyReader.Core.Models
{
    public class CartridgeRAMContent
    {
        public CartridgeInformation CartridgeInformation { get; set; }
        public List<Byte> CartridgeRAMByteContent { get; set; }
        public bool IsRAMPresent { get; set; }

        public CartridgeRAMContent() 
        {
            CartridgeInformation = new();
            CartridgeRAMByteContent = new();
            IsRAMPresent = false;
        }

    }
}
