namespace GameBoyReader.Core.Exceptions
{
    public class PathInputNullException: Exception
    {
        public override string Message => "Provided path to file is null. Launch this app again.";
    }
}
