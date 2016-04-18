using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QRConverter;
using ZXing;


namespace QRConverterTests
{
   
    [TestFixture]
    public class EncoderTests
    {
        [Test]
        public void ConvertSmallText()
        {
            var converter = new QrCodeConverter();
            var text = StringGen.GetRandomString(40);
            var bitmap = converter.Encode(text, 400);
            var expected = bitmap != null;
            Assert.IsTrue(expected);
        }

        [Test]
        public void ConvertMaxLengthMinusOne()
        {
            var converter = new QrCodeConverter();
            var text = StringGen.GetRandomString(Constants.LMaxByteSize - 1);
            var bitmap = converter.Encode(text, 400);
            var expected = bitmap != null;
            Assert.IsTrue(expected);
        }

        [Test]
        public void ConvertMaxLength()
        {
            var converter = new QrCodeConverter();
            var text = StringGen.GetRandomString(Constants.LMaxByteSize);
            var bitmap = converter.Encode(text, 400);
            var expected = bitmap != null;
            Assert.IsTrue(expected);
        }

        [Test]
        public void ConvertMaxLenghPlusOne()
        {
            var converter = new QrCodeConverter();
            Assert.Throws<WriterException>(() => converter.Encode(StringGen.GetRandomString(Constants.LMaxByteSize + 1), 300));
        }

    }

    [TestFixture]
    public class DecoderTests
    {
        [Test]
        public void DecodeSmallResolution()
        {
            var converter = new QrCodeConverter();
            var text = StringGen.GetRandomString(Constants.LMaxByteSize);
            var bitmap = converter.Encode(text, 400);
            var actual = converter.Decode(bitmap);
            var expected = text;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DecodeLargeResolution()
        {
            var converter = new QrCodeConverter();
            var text = StringGen.GetRandomString(Constants.LMaxByteSize);
            var bitmap = converter.Encode(text, 5000);
            var actual = converter.Decode(bitmap);
            var expected = text;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DecodeRussianAndSpecialCharacters()
        {
            var converter = new QrCodeConverter();
            var text = StringGen.GetRandomString("йцукенгшщзхэж  №;%:?*())_+@", 10);
            var bitmap = converter.Encode(text, 400);
            var actual = converter.Decode(bitmap);
            var expected = text;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DecodeRussianAndSpecialCharactersLargeResolution()
        {
            var converter = new QrCodeConverter();
            var text = StringGen.GetRandomString("йцукенгшщ  я№;%:?*())_+@", 10);
            var bitmap = converter.Encode(text, 5000);
            var actual = converter.Decode(bitmap);
            var expected = text;
            Assert.AreEqual(expected, actual);
        }
    }

    [TestFixture]
    public class ExtensionTests
    {
        [Test]
        public void DivideMaxTextPlusOne()
        {
            var text = new string('a', Constants.LMaxByteSize + 1);
            var actual = text.Divide(text.CountBytes("utf-8"), "utf-8");
            var expected = new List<string> {text.Substring(0, text.Length - 1), text.Last().ToString()};
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void DivideDoubleMaxTextMinusOne()
        {
            var text = new string('a', Constants.LMaxByteSize * 2 - 1);
            var actual = text.Divide(text.CountBytes("utf-8"), "utf-8");
            var expected = new List<string> { text.Substring(0, Constants.LMaxByteSize), text.Substring(Constants.LMaxByteSize) };
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void DivideDoubleMaxText()
        {
            var text = new string('a', Constants.LMaxByteSize * 2);
            var actual = text.Divide(text.CountBytes("utf-8"), "utf-8");
            var expected = new List<string> { text.Substring(0, Constants.LMaxByteSize), text.Substring(Constants.LMaxByteSize) };
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void DivideMaxText()
        {
            var text = new string('a', Constants.LMaxByteSize);
            Assert.Throws<ArgumentException>(() => text.Divide(text.CountBytes("utf-8"), "utf-8"));
        }
    }
}
