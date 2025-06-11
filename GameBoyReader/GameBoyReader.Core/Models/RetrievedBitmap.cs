namespace GameBoyReader.Core.Models
{
    public class RetrievedBitmap
    {
        public List<Byte> Bitmap { get; set; }
        public bool IsBitmapCorrect { get; set; }

        public RetrievedBitmap() {
            Bitmap = new List<Byte>();
            IsBitmapCorrect = false;
        }
    }
}
