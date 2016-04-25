# QrCodeConverter
**QrCodeConverter** is a simple cmdlet which combines two functionalities: 

* Encodes text into QR code image.
* Decodes QR code image into text (image is captured either from webcam or from the specified file).

**QrCodeConverter** employs  the following cmd line `parameters`:

* -o: Full output path including file name. If file exists it's overwritten. Otherwise new file is created. 
* -m: Mode ('read' to read text from QR code, 'create' to create QR code image from text, 'webcam' to capture and decode image from webcam). 
* -s: [optional] Full path to source image or text including file name. 
* -e: [optional] Encoding of text used to generate QR code. Default: utf-8. 
* -r: [optional] Resolution of generated QR code in px. Default: 300.
* -h: Help for this application. 

Converter was built using [ZXing.Net](http://zxingnet.codeplex.com/) for QR code encoding \ decoding, [Nlog](https://github.com/NLog/NLog) for logging purposes, [AForge.NET](http://www.aforgenet.com/framework) for working with webcam and [Command Line Parser](http://commandline.codeplex.com/). 

## What's next
* GUI with options to choose Error Correction level for generated code and whether the decoded image is a pure QR code image. 
