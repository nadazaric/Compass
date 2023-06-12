using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace travel_agent.Models
{

	public class Arrangement
	{
		[Key] public int Id { get; set; }
		public bool IsDeleted { get; set; }
		public string Name { get; set; }
		public byte[] ImageData { get; set; }
		public List<Place> Places { get; set; }
		public List<ArrangementStep> Steps { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public double TotalDistance { get; set; }
		public decimal Price { get; set; }
		public string Description { get; set; }

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

		public Arrangement()
		{
			Places = new List<Place>();
			Steps = new List<ArrangementStep>();
		}

        public override string ToString()
        {
			String temp = "Aktivan";
			if (IsDeleted) temp = "Uklonjen";
			return Id + " - " + Name + " - " + Price + "RSD - " + temp;
        }
    }
}
