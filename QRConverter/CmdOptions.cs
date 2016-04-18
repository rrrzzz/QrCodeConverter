﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace QRConverter
{
    class CmdOptions
    {
        [Option('e', "encoding", Required = false, HelpText = "Encoding of text used to generate QR code.", DefaultValue = "utf-8")]
        public string Encoding { get; set; }

        [Option('r', "resolution", Required = false, HelpText = "Resolution of generated QR code in px. Resolutions over 800px tend to be problematic with typical QR readers", DefaultValue = 300)]
        public int Res { get; set; }

        [Option('o', "output", Required = true, HelpText = "Full output path including file name. If file exists it's overwritten. Otherwise new file is created.")]
        public string Output { get; set; }

        [Option('s', "source", Required = true, HelpText = "Full path to source image or text including file name.")]
        public string Source { get; set; }
        
        [Option('m', "mode", Required = true, HelpText = "Mode ('read' to read text from QR code, 'create' to create QR code image from text).")]
        public string Mode { get; set; }

        [Option('h', "help", Required = false,
            HelpText = "Help for QRConverter.exe")]
        
        public static string HelpInfo
        {
            get
            {
                return "QRConverter is a cmdlet to encode text to QR code or decode text from QR code \n" +
                "Use following Parameters:\n" +
                "\t-e:\t[optional] Encoding of text used to generate QR code. Default: utf-8.\n" +
                "\t-r:\t[optional] Resolution of generated QR code in px. Resolutions over 800px tend to be problematic with typical QR readers. Default: 300.\n" +
                "\t-o:\tFull output path including file name. If file exists it's overwritten. Otherwise new file is created.\n" +
                "\t-s:\tFull path to source image or text including file name.\n" +
                "\t-m:\tMode ('read' to read text from QR code, 'create' to create QR code image from text).\n" +
                "\t-h:\tHelp for this application\n";
            }
        }
    }
}
