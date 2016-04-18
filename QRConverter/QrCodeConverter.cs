using System.Collections.Generic;
using System.Drawing;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace QRConverter
{
    public class QrCodeConverter
    {
        public Bitmap Encode(string textToConvert, int resolution, string encoding = "utf-8")
        {
            IBarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions()
                {
                    ErrorCorrection = ErrorCorrectionLevel.L,
                    CharacterSet = encoding.ToLower(),
                    Width = resolution,
                    Height = resolution,
                }
            };
            return writer.Write(textToConvert);
        }

        public string Decode(Bitmap bitmap)
        {
            var options = new DecodingOptions
            {
                TryHarder = true,
                PureBarcode = false,
                PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }
            };

            IBarcodeReader reader = new BarcodeReader{Options = options};
            return reader.Decode(bitmap).Text;
        }
    }
}

