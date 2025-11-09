namespace GameBoyReader.Core.Exceptions
{
    public class CartridgeNotSupportedException: Exception
    {
        public override string Message => "This cartridge is unsupported by this device.";
    }
}
