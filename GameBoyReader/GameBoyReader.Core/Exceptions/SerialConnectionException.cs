namespace GameBoyReader.Core.Exceptions
{
    public class SerialConnectionException: Exception
    {
        public override string Message => "Serial connection with device has failed. Try launching this app again.";
    }
}
