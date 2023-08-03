using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace QlySanBong.ResourceXAML
{
    public class Converter
    {
        private static Converter instance;

        public static Converter Instance
        {
            get { if (instance == null) instance = new Converter(); return Converter.instance; }
            private set { Converter.instance = value; }
        }
        private Converter()
        {

        }

        public Byte[] ConvertImageToBytes(string imageFileName)
        {
            FileStream fs = new FileStream(imageFileName, FileMode.Open, FileAccess.Read);

            //Initialize a byte array with size of stream
            byte[] imgByteArr = new byte[fs.Length];

            //Read data from the file stream and put into the byte array
            fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

            //Close a file stream
            fs.Close();
            return imgByteArr;
        }
        public BitmapImage ConvertByteToBitmapImage(Byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        public byte[] ConvertBitmapImageToBytes(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }


    }

}
