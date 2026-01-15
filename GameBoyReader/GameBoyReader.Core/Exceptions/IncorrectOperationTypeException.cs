namespace GameBoyReader.Core.Exceptions
{
    public class IncorrectOperationTypeException: Exception
    {
        public override string Message => "This operation is not supported by the device.";
    }
}
