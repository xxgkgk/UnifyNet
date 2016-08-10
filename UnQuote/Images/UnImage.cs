using System;
using System.Drawing;
using System.IO;
using ThoughtWorks.QRCode.Codec;
using UnCommon.Files;
using UnCommon.Config;
using System.Drawing.Drawing2D;

namespace UnQuote.Images
{
    /// <summary>
    /// 图片类
    /// </summary>
    public class UnImage
    {

        /// <summary>
        /// 生成二维码图片流
        /// </summary>
        /// <param name="strCode">二维码内容</param>
        /// <param name="version">等级</param>
        /// <param name="errorCorrect">容错等级</param>
        /// <returns></returns>
        public static Bitmap createQrcCode(string strCode, int version, UnImageQRCEtr etr, int scale)
        {
            QRCodeEncoder qr = new QRCodeEncoder();
            qr.QRCodeVersion = version;
            switch (etr)
            {
                case UnImageQRCEtr.L:
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                    break;
                case UnImageQRCEtr.M:
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    break;
                case UnImageQRCEtr.Q:
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    break;
                case UnImageQRCEtr.H:
                    qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
            }
            qr.QRCodeScale = scale;
            return qr.Encode(strCode, UnInit.getEncoding());
        }

        /// <summary>
        /// 生成二维码路径
        /// </summary>
        /// <param name="strCode">二维码内容</param>
        /// <param name="version">等级(默认0)</param>
        /// <param name="errorCorrec">容错率</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns></returns>
        public static UnFileInfo createQrcPath(string strCode, int version, UnImageQRCEtr etr, int scale)
        {
            FileInfo f = UnFile.createFile(UnFileEvent.tmp, ".png", true);
            using (Bitmap b = createQrcCode(strCode, version, etr, scale))
            {
                b.Save(f.FullName);
            }
            return new UnFileInfo(f.FullName);
        }

        /// <summary>
        /// 生成二维码路径
        /// </summary>
        /// <param name="strCode">二维码内容</param>
        /// <param name="version">等级(默认0)</param>
        /// <param name="errorCorrec">容错率</param>
        /// <returns></returns>
        public static UnFileInfo createQrcPath(string strCode, int version, UnImageQRCEtr etr)
        {
            return createQrcPath(strCode, version, etr, 4);
        }


        /// <summary>
        /// 创建验证码并返回流
        /// </summary>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        public static MemoryStream createVerCode(string checkCode)
        {
            MemoryStream ms = null;
            if (checkCode == null || checkCode.Trim() == String.Empty)
            {
                return ms;
            }
            Bitmap image = new Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器 
                Random random = new Random();
                //清空图片背景色 
                g.Clear(Color.White);
                //画图片的背景噪音线 
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景噪音点 
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
            return ms;
        }

        /// <summary>  
        /// Resize图片  
        /// </summary>  
        /// <param name="bmp">原始Bitmap</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        /// <returns>处理以后的Bitmap</returns>  
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return b;
            }
            catch
            {
                return null;
            }
        } 
    
    }
}
