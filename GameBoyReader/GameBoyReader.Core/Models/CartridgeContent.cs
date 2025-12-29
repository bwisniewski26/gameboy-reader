namespace GameBoyReader.Core.Models
{
    public class CartridgeContent
    {
        public CartridgeInformation CartridgeInformation { get; set; }
        public List<Byte> CartridgeByteContent { get; set; }
        public CartridgeContent()
        {
            CartridgeInformation = new();
            CartridgeByteContent = new();
        }
    }
}
