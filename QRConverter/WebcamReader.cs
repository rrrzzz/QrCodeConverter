using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using AForge.Video;
using AForge.Video.DirectShow;

namespace QRConverter
{
    class WebcamReader
    {
        public Bitmap Frame { get; set; }
        public VideoCaptureDevice VideoSource { get; set; }

        private FilterInfoCollection GetWebcams() => new FilterInfoCollection(FilterCategory.VideoInputDevice);
        private VideoCaptureDevice CreateVideoSource(FilterInfoCollection devices) =>
            new VideoCaptureDevice(devices[0].MonikerString);

        private void AttachFrameHandler(VideoCaptureDevice source) => source.NewFrame += video_NewFrame;

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Frame = (Bitmap)eventArgs.Frame.Clone();
        }

        public WebcamReader()
        {
            var cams = GetWebcams();
            if (cams.Count == 0)
            {
                throw new SystemException("Couldn't find a webcam");
            }
            VideoSource = CreateVideoSource(cams);
            AttachFrameHandler(VideoSource);
        }
    }
}
