using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace QRConverter
{
    class Program
    {
        private static QrCodeConverter _converter;

        public static void CreateAndSaveQrCode(string textPath, string qrWritePath, int resolution, string encoding = "utf-8")
        {
            string text = ReadText(textPath, encoding);
            var textSize = text.CountBytes(encoding);
            if (textSize > Constants.LMaxByteSize)
            {
                var textsList = text.Divide(textSize, encoding);
                for (int i = 0; i < textsList.Count; i++)
                {
                    var path = qrWritePath.Insert(qrWritePath.LastIndexOf('.'), "_" + i);
                    var qrBitmap = _converter.Encode(textsList[i], resolution);
                    SaveImage(qrBitmap, path);
                }
            }
            else
            {
                var qrBitmap = _converter.Encode(text, resolution);
                SaveImage(qrBitmap, qrWritePath);
            }
        }

        private static string ReadText(string textPath, string encoding = "utf-8")
        {
            using (var stream = new FileStream(textPath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream, Encoding.GetEncoding(encoding)))
            {
                return reader.ReadToEnd();
            }
        }

        private static void SaveImage(Bitmap imageBitmap, string imagePath)
        {
            try
            {
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageBitmap.Save(stream, ImageFormat.Jpeg);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Access to file is unauthorized. Check folder security settings.");
                throw ex;
            }
        }

        private static void SaveText(string text, string textPath)
        {
            try
            {
                File.WriteAllText(textPath, text);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Access to file is unauthorized. Check folder security settings.");
                throw ex;
            }
        }

        static void Main(string[] args)
        {
            _converter = new QrCodeConverter();
            Console.WriteLine("Mode ('read' to read text from QRCode, 'create' to create QRCode image from text):");
            var mode = Console.ReadLine().ToLower();
            while (mode != "create" & mode != "read")
            {
                Console.WriteLine("Wrong input");
                Console.WriteLine("Mode ('read' to read text from QRCode, 'create' to create QRCode image from text):");
                mode = Console.ReadLine();
            }

            Console.WriteLine(@"Source (full path including name of the file 'c:\temp\source.txt'):");
            var sourceFile = Console.ReadLine();
            Console.WriteLine(@"Destination (full path including name of the file with extension 'c:\temp\destination.jpg'. If file exists it will be overwritten. Otherwise new file will be created): ");
            var destFile = Console.ReadLine();

            if (mode == "create")
            {
                Console.WriteLine(
                    "Resolution in pixels (resolutions over 800px tend to be problematic with typical QR readers):");
                int res;
                while (!int.TryParse(Console.ReadLine(), out res))
                {
                    Console.WriteLine("Wrong input.");
                    Console.WriteLine(
                        "Resolution in pixels (resolutions over 800px tend to be problematic with typical QR readers):");
                }
                CreateAndSaveQrCode(sourceFile, destFile, res);
            }
            else
            {
                var qrBitmap = new Bitmap(Image.FromFile(sourceFile));
                var decodedText = _converter.Decode(qrBitmap);
                SaveText(decodedText, destFile);
            }
        }
    }
}
