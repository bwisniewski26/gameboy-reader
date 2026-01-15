namespace GameBoyReader.Core.Exceptions
{
    public class CartridgeNotSupportedException: Exception
    {
        public override string Message => "Inserted cartridge is not supported by this device.";
    }
}
