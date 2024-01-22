using System;
using ImageMagick;

namespace CoreLibrary.Extensions
{
    public static class ImageCompressExtension
    {
        public static byte[] CompressAsync(byte[] defImage)
        {
            try
            {
                if (defImage.Length / 1024f / 1024f > 0)
                {
                    using MagickImage img = new MagickImage(defImage);
                    img.Format = MagickFormat.WebP;
                    img.Quality = 40;
                    int width = img.Width;
                    int height = img.Height;

                    // if (img.Width > 1024)
                    // {
                    //     width = 1024;
                    //     height = (1024 * img.Height) / img.Width;
                    // }
                    
                    if (img.Height != height || img.Width != width)
                    {
                        decimal result_ratio = (decimal)height / (decimal)width;
                        decimal current_ratio = (decimal)img.Height / (decimal)img.Width;

                        bool preserve_width = current_ratio > result_ratio;

                        int new_width = 0;
                        int new_height = 0;
                        if (preserve_width)
                        {
                            new_width = width;
                            new_height = (int)Math.Round((decimal)(current_ratio * new_width));
                        }
                        else
                        {
                            new_height = height;
                            new_width = (int)Math.Round((decimal)(new_height / current_ratio));
                        }
                        
                        String geomStr = width.ToString() + "x" + height.ToString();
                        String newGeomStr = new_width.ToString() + "x" + new_height.ToString();

                        MagickGeometry intermediate_geo = new MagickGeometry(newGeomStr);
                        MagickGeometry final_geo = new MagickGeometry(geomStr);


                        img.Resize(intermediate_geo);
                        img.Crop(final_geo);
                    }

                    return img.ToByteArray();
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}