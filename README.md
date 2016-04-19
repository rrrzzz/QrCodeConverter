# QrCodeConverter
**QrCodeConverter** is a simple cmdlet which combines two functionalities: 

* Encodes text into QR code image.
* Decodes QR code image into text.

**QrCodeConverter** employs  the following cmd line `parameters`:

* -o: Full output path including file name. If file exists it's overwritten. Otherwise new file is created. 
* -s: Full path to source image or text including file name. 
* -m: Mode ('read' to read text from QR code, 'create' to create QR code image from text). 
* -e: [optional] Encoding of text used to generate QR code. Default: utf-8. 
* -r: [optional] Resolution of generated QR code in px. Default: 300.
* -h: Help for this application. 

Converter was built using [ZXing.Net](http://zxingnet.codeplex.com/) for QR code encoding \ decoding, [Nlog](https://github.com/NLog/NLog) for logging purposes and [Command Line Parser](http://commandline.codeplex.com/). 

## What's next
* QR code capture from webcam video stream. 
* GUI with options to choose Error Correction level for generated code and whether the decoded image is a pure QR code image. 
