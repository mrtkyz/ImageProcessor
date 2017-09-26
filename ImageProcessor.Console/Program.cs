using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace ImageProcessor.Console
{
    class Program
    {
        static void Main()
        {
            string html = GetHtml();
            string regex = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
            var matchesImages = Regex.Matches(html, regex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var imagesSrcList = matchesImages.Cast<Match>().Select(m => m.Groups[1].Value).ToList();
            var imageName = SaveImages(imagesSrcList);

            foreach (var item in imagesSrcList)
            {
                html = html.Replace(item, imageName.Replace("\\", "/"));
            }


        }


        static string SaveImages(List<string> items)
        {
            int width = 0;
            int height = 0;
            List<Bitmap> images = new List<Bitmap>();
            foreach (string m in items)
            {
                string href = "C:\\Users\\ExtMuratA\\Documents\\visual studio 2015\\Projects\\ImageSpriter\\ImageProcessor.Console\\" + m;

                Bitmap bitmap = new Bitmap(href);
                width += bitmap.Width;
                height = bitmap.Height > height ? bitmap.Height : height;
                images.Add(bitmap);
            }
            var finalImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(finalImage))
            {
                g.Clear(Color.Transparent);

                int offset = 0;
                foreach (Bitmap image in images)
                {
                    g.DrawImage(image, new Rectangle(offset, 0, image.Width, image.Height));
                    offset += image.Width;
                }
            }

            var fileName = string.Format("\\Content\\img\\{0}.jpg", Guid.NewGuid());
            finalImage.Save("C:\\Users\\ExtMuratA\\Documents\\visual studio 2015\\Projects\\ImageSpriter\\ImageProcessor.Console" + fileName);

            finalImage.Dispose();
            foreach (Bitmap image in images)
            {
                image.Dispose();
            }

            return fileName;
        }
        
        static string GetHtml()
        {
            return
                "<!DOCTYPE html>" +
                "<html>" +
                "<head>" +
                    "<meta charset=\"utf-8\" />" +
                    "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">" +
                    "<title> - My ASP.NET Application</title>" +
                    "<link href=\"/Content/Site.css\" rel=\"stylesheet\" type=\"text/css\" />" +
                    "<link href=\"/Content/bootstrap.min.css\" rel=\"stylesheet\" type=\"text/css\" />" +
                    "<script src=\"/Scripts/modernizr-2.6.2.js\"></script>" +
                "</head>" +
                "<body>" +
                    "<div class=\"container body-content\">" +
                        "<img src=\"/Content/img/1.jpg\" style=\"width:100px;height:100px\" />" +
                        "<img src=\"/Content/img/2.jpg\" style=\"width:150px;height:150px\" />" +
                        "<img src=\"/Content/img/3.jpg\" style=\"width:200px;height:200px\" />" +
                        "<img src=\"/Content/img/4.jpg\" style=\"width:250px;height:250px\" />" +
                        "<hr />" +
                        "<footer>" +
                        "<p>&copy; 2017 - My ASP.NET Application</p>" +
                        "</footer>" +
                    "</div>" +
                    "<script src=\"/Scripts/jquery-1.10.2.min.js\"></script>" +
                    "<script src=\"/Scripts/bootstrap.min.js\"></script>" +
                "</body>" +
                "</html>";
        }
    }
}
