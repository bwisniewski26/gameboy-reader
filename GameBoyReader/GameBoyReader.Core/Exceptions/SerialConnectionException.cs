namespace GameBoyReader.Core.Exceptions
{
    public class SerialConnectionException: Exception
    {
        public override string Message => "Serial connection with device has failed. Please select different COM port and try again. If this issue persists please launch this app again.";
    }
}
