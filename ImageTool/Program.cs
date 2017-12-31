using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTool
{
    class Program
    {
        public static string ImagePath = @"C:\Users\Miko\Documents\GitHub\Simple2DGame\QuickGame1\Content\Textures\";

        static void Main(string[] args)
        {
            var skeleton = Stack("Skeleton Attack.png", "Skeleton Dead.png", "Skeleton Hit.png", "Skeleton Idle.png", "Skeleton React.png", "Skeleton Walk.png");
            skeleton.Save(ImagePath + "skeleton.png");
        }


        public static Bitmap Stack(params string[] fileNames)
        {
            return Stack(fileNames.Select(p => new Bitmap(ImagePath + p)));
        }



        public static Bitmap Stack(IEnumerable<Bitmap> images)
        {
            var ret = new Bitmap(images.Max(p => p.Width), images.Sum(p => p.Height));
            int y = 0;
            foreach(var img in images)
            {
                using (var g = Graphics.FromImage(ret))
                {
                    g.DrawImage(img, new Point(0, y));
                    y += img.Height;
                }
            }

            return ret;

        }
    }
}
