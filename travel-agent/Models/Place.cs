using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Windows.Media.Imaging;

namespace travel_agent.Models
{
    public class Place
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public byte[] ImageData { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public PlaceType Type { get; set; }

        [NotMapped]
        public BitmapImage Image
        {
            get
            {
                using (MemoryStream memoryStream = new MemoryStream(ImageData))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.EndInit();
                    return bitmapImage;
                }
            }
            set
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(value));
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    encoder.Save(memoryStream);
                    ImageData = memoryStream.ToArray();
                }
            }
        }

        public enum PlaceType { ATRACTION, RESTAURANT, ACCOMMODATION }
    }
}
