using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AE_TestTask.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            Bitmap image1 = new Bitmap(@"C:\Users\pb\Downloads\AE_TestTask\AE_TestTask\image1.png");
            Bitmap image2 = new Bitmap(@"C:\Users\pb\Downloads\AE_TestTask\AE_TestTask\image2.png");
            Bitmap result;

            if (image1.Size != image2.Size)
            {
                return View();
            }
            else
            {
                result = new Bitmap(image2.Width, image2.Height);
            }

            Int32 image1Pixel, image2Pixel, diffRGBValue = 0;
            Double diffPercentage = 0.0;
            Boolean isDiffPreviousPixel = false;
            IList<Rectangle> rectangles = new List<Rectangle>();

            for (Int32 y = 0; y < image1.Height; y++)
            {
                for (Int32 x = 0; x < image1.Width; x++)
                {
                    image1Pixel = image1.GetPixel(x, y).ToArgb();
                    image2Pixel = image2.GetPixel(x, y).ToArgb();
                    diffRGBValue = Math.Abs(image1Pixel - image2Pixel);

                    if (diffRGBValue > 0)
                    {
                        //isDiffPreviousPixel = true;
                        //isDiffPreviousPixel = false;
                    }

                    diffPercentage = (Double)diffRGBValue / image1Pixel;

                    if (Math.Abs(diffPercentage) > 0.1)
                    {
                        Rectangle rectangle = rectangles.FirstOrDefault(r => (r.Y == y) // && r.X + r.Width <= x && x <= r.X + r.Width + 100)
                                                                             || (r.X - 50 <= x && x <= r.X + r.Width && r.Y + 25 >= y));

                        if (rectangle.IsEmpty)
                        {
                            rectangle = new Rectangle(x, y, 1, 1);
                        }
                        else
                        {
                            rectangles.Remove(rectangle);

                            if (rectangle.Y == y)
                            {
                                rectangle.Width = x - rectangle.X;
                            }
                            else
                            {
                                rectangle.Height = y - rectangle.Y;
                            }
                        }

                        rectangles.Add(rectangle);
                    }
                    else
                    {
                        //Rectangle rect = new Rectangle();
                    }
                }
            }

            // Load the image (probably from your stream)
            Image image = Image.FromFile(@"C:\Users\pb\Downloads\AE_TestTask\AE_TestTask\image2.png");

            using (Graphics g = Graphics.FromImage(image))
            {
                Pen rectPen = new Pen(Color.Red);
                foreach (Rectangle rectangle in rectangles)
                {
                    g.DrawRectangle(rectPen, rectangle);
                }
            }

            image.Save(@"C:\Users\pb\Downloads\AE_TestTask\AE_TestTask\image3.png");

            return View();
        }

    }
}