using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NLog;


namespace QRConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            _converter = new QrCodeConverter();



            _converter = new QrCodeConverter();
            var options = new CmdOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                logger.Info(CmdOptions.HelpInfo);
                return;
            }

            options.Mode = options.Mode.ToLower();
            if (options.Mode != "create" && options.Mode != "read" && options.Mode != "webcam")
            {
                logger.Info("Wrong mode input.");
                logger.Info(CmdOptions.HelpInfo);
                return;
            }

            logger.Info("Working...");
            if (options.Mode == "create")
            {
                if (ValidateFormat(options.Output))
                {
                    CreateAndSaveQrCode(options.Source, options.Output, options.Res, options.Encoding);
                    logger.Info("Done!");
                    return;
                }
                logger.Info("Image format is not supported. Supported formats: .jpeg, .bmp, .png, .emf, .exif, .gif, .tiff, .wmf");
            }
            else if (options.Mode == "read")
            {
                if (ValidateFormat(options.Source))
                {
                    var qrBitmap = new Bitmap(Image.FromFile(options.Source));
                    var decodedText = _converter.Decode(qrBitmap);
                    SaveText(decodedText, options.Output);
                    logger.Info("Done!");
                    return;
                }
                logger.Info("Image format is not supported. Supported formats: .jpeg, .bmp, .png, .emf, .exif, .gif, .tiff, .wmf");
            }
            else
                ReadFromWebcam(options.Output);
            Environment.Exit(0);
        }

        private static Logger logger = InitLogger.GetLogger("Program");
        private static QrCodeConverter _converter;

        private static void ReadFromWebcam(string output)
        {
            WebcamReader webcam;
            try
            {
                webcam = new WebcamReader();
            }
            catch (SystemException ex)
            {
                logger.Info(ex.Message);
                return;
            }

            webcam.VideoSource.Start();
            while (!webcam.VideoSource.IsRunning){}
            for (int i = 0; i < 5; i++)
            {
                System.Threading.Thread.Sleep(2000);
                try
                {
                    var currentFrame = webcam.Frame;
                    var result = _converter.Decode(currentFrame);
                    logger.Info($"QR code text: {result}");
                    SaveText(result, output);
                    return;
                }
                catch (NullReferenceException)
                {
                    logger.Info($"Couldn't capture QR code. Making another attempt... {4-i} left");
                }
            }
            logger.Info("Failed to capture QR code within 10 sec time limit.");
        }

        private static void CreateAndSaveQrCode(string textPath, string qrWritePath, int resolution, string encoding = "utf-8")
        {
            string text = ReadText(textPath, encoding);
            var textSize = text.CountBytes(encoding);
            if (textSize > Constants.LMaxByteSize)
            {
                var textsList = text.Divide(textSize, encoding);
                for (int i = 0; i < textsList.Count; i++)
                {
                    var path = qrWritePath.Insert(qrWritePath.LastIndexOf('.'), "_" + i);
                    var qrBitmap = _converter.Encode(textsList[i], resolution, encoding);
                    SaveImage(qrBitmap, path);
                }
            }
            else
            {
                var qrBitmap = _converter.Encode(text, resolution, encoding);
                SaveImage(qrBitmap, qrWritePath);
            }
        }

        private static string ReadText(string textPath, string encoding)
        {
            try
            {
                using (var stream = new FileStream(textPath, FileMode.Open, FileAccess.Read))
                using (var reader = new StreamReader(stream, Encoding.GetEncoding(encoding)))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (ArgumentException ex)
            {
                logger.Error(ex, "Invalid encoding specified");
                logger.Info("Invalid encoding specified.");
                throw ex;
            }
        }

        private static bool ValidateFormat(string imgPath)
        {
            var formats = new[] { "jpeg", "jpg", "png", "emf", "exif", "gif", "tiff", "wmf", "bmp" };
            var extension = imgPath.Substring(imgPath.LastIndexOf('.') + 1).ToLower();
            return formats.Contains(extension);
        }

        private static void SaveImage(Bitmap imageBitmap, string imagePath)
        {
            var extension = imagePath.Substring(imagePath.LastIndexOf('.') + 1).ToLower();
            extension = extension != "jpg" ? extension.Replace(extension[0], char.ToUpper(extension[0])) : "Jpeg";
            var format = (ImageFormat) typeof (ImageFormat).GetProperty(extension).GetValue(null);
            
            try
            {
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageBitmap.Save(stream, format);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.Info("Access to file is unauthorized. Check folder security settings.");
                logger.Error(ex, "Unauthorized file access");
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
                logger.Info("Access to file is unauthorized. Check folder security settings.");
                logger.Error(ex, "Unauthorized file access");
                
                throw ex;
            }
        }
    }
}
