namespace GameBoyReader.Core.Exceptions
{
    public class PathInputNullException: Exception
    {
        public override string Message => "Path to save dumped file is null. Launch this app again.";
    }
}
