using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tongrenlu_Windows.Tools
{
    public class ImageHelper
    {
        public static void LoadImage(Image image, string path, string placeholder)
        {
            if (File.Exists(path))
            {
                image.Source = LoadBitmapFromFile(path);
            }
            else
            {
                image.Source = LoadBitmapFromResource(placeholder);
            }
        }

        public static BitmapImage LoadBitmapFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(filePath, UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                return src;
            }
            return null;
        }

        public static BitmapImage LoadBitmapFromResource(string pathInApplication, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            if (pathInApplication[0] == '/')
            {
                pathInApplication = pathInApplication.Substring(1);
            }
            return new BitmapImage(new Uri(@"pack://application:,,,/" + assembly.GetName().Name + ";component/" + pathInApplication, UriKind.Absolute));
        }
    }
}
