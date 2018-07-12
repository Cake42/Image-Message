using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

public static class ImageMessage
{
    public static string Decode(string source)
    {
        Bitmap bitmap = null;
        string path = "Message";
        try
        {
            if (Uri.TryCreate(source, UriKind.Absolute, out Uri uri) && uri.Scheme == Uri.UriSchemeHttps)
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(uri, path);
                    bitmap = new Bitmap(path);
                }
            }
            else
            {
                bitmap = new Bitmap(source);
            }

            byte[] message = new byte[bitmap.Width];
            for (int j = 0; j < bitmap.Width; j++)
            {
                message[j] = bitmap.GetPixel(j, 0).R;
            }

            return Encoding.ASCII.GetString(message);
        }
        finally
        {
            bitmap?.Dispose();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    public static void Encode(string contents, string path)
    {
        byte[] pixels = Encoding.ASCII.GetBytes(contents);

        int width = pixels.Length;
        int height = 1;
        
        using (Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed))
        {
            ColorPalette palette = bitmap.Palette;

            for (int j = 0; j < 256; j++)
            {
                palette.Entries[j] = Color.FromArgb(j, j, j);
            }

            bitmap.Palette = palette;
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
            bitmap.UnlockBits(data);

            if (Directory.Exists(path))
            {
                path = Path.Combine(path, "Encoded.png");
            }
            else if (!Path.HasExtension(path))
            {
                path = path + ".png";
            }

            bitmap.Save(path, ImageFormat.Png);
        }
    }
}
