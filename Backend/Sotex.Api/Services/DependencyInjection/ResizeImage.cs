using System.Drawing;
using System.Drawing.Imaging;


namespace Sotex.Api.Services.DependencyInjection
{
    public class ResizeImage
    {
        public string Resize(Stream imageStream, int maxWidth, int maxHeight)
        {
            using var originalImage = Image.FromStream(imageStream);

            int newWidth = originalImage.Width;
            int newHeight = originalImage.Height;

            if (originalImage.Width > maxWidth || originalImage.Height > maxHeight)
            {
                float widthRatio = (float)maxWidth / originalImage.Width;
                float heightRatio = (float)maxHeight / originalImage.Height;
                float scaleRatio = Math.Min(widthRatio, heightRatio);
                newWidth = (int)(originalImage.Width * scaleRatio);
                newHeight = (int)(originalImage.Height * scaleRatio);
            }

            using var resizedImage = new Bitmap(originalImage, newWidth, newHeight);
            using var ms = new MemoryStream();

            resizedImage.Save(ms, ImageFormat.Jpeg);

            byte[] imageBytes = ms.ToArray();
            return Convert.ToBase64String(imageBytes);
        }


    }
}
