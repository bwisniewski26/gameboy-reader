namespace GameBoyReader.Core.Exceptions
{
    internal class BootBitmapException: Exception
    {
        public override string Message => "Retrieved verification data was incorrect. Clean cartridge contacts and try inserting it again.";
    }
}
