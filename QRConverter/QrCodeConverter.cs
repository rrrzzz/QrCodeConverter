using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace QRConverter
{
    public class QrCodeConverter
    {
        public void CreateQrCode(string textPath, int resolution, string qrWritePath)
        {
            string text = ReadText(textPath);
            var textSize = Encoding.UTF8.GetByteCount(text);
            if (textSize > Constants.LMaxByteSize)
            {
                var stringsList = StringDivider.DivideString(text, textSize);
                CreateCode(stringsList, resolution, qrWritePath);
            }
            else
                CreateCode(text, resolution, qrWritePath);
        }

        public void ReadQrCode(string qrCodePath, string textWritePath)
        {
            var qrBitmap = ReadImage(qrCodePath);
            var decodedText = DecodeQrCode(qrBitmap);
            SaveText(decodedText, textWritePath);
        }

        private void CreateCode(string text, int resolution, string qrWritePath)
        {
            var qrBitmap = EncodeQrCode(text, resolution);
            SaveImage(qrBitmap, qrWritePath);
        }

        private void CreateCode(List<string> stringsList, int resolution, string qrWritePath)
        {
            for (int i = 0; i < stringsList.Count; i++)
            {
                var path = qrWritePath.Insert(qrWritePath.LastIndexOf('.') - 1, "_" + i);
                CreateCode(stringsList[i], resolution, path);
            }
        }

        private Bitmap EncodeQrCode(string textToConvert, int resolution)
        {
            IBarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions()
                {
                    ErrorCorrection = ErrorCorrectionLevel.L,
                    CharacterSet = "utf-8",
                    Width = resolution,
                    Height = resolution,
                }
            };
            return writer.Write(textToConvert);
        }

        private string DecodeQrCode(Bitmap bitmap)
        {
            var qrReader = new QRCodeReader();
            var binaryBitmap = ConvertToBinaryBitmap(bitmap);
            return qrReader.decode(binaryBitmap).Text;
        }

        private BinaryBitmap ConvertToBinaryBitmap(Bitmap bitmap)
        {
            var lumSource = new BitmapLuminanceSource(bitmap);
            var binarizer = new HybridBinarizer(lumSource);
            return new BinaryBitmap(binarizer);
        }

        private string ReadText(string textPath)
        {
            using (var stream = new FileStream(textPath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private Bitmap ReadImage(string imgPath)
        {
            return new Bitmap(Image.FromFile(imgPath));
        }

        private void SaveImage(Bitmap imageBitmap, string imagePath)
        {
            try
            {
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageBitmap.Save(stream, ImageFormat.Jpeg);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Access to file is unauthorized. Check folder security settings.");
            }
        }

        private void SaveText(string text, string textPath)
        {
            try
            {
                File.WriteAllText(textPath, text);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Access to file is unauthorized. Check folder security settings.");
            }
        }
    }
}

