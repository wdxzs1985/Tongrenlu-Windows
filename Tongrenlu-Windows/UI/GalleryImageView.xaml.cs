using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tongrenlu_Windows.Data;
using Tongrenlu_Windows.Tools;

namespace Tongrenlu_Windows.UI
{
    /// <summary>
    /// Interaction logic for CoverImage.xaml
    /// </summary>
    public partial class GalleryImageView : UserControl
    {

        public static readonly DependencyProperty CoverObjectProperty = DependencyProperty.Register(
                "Cover",
                typeof(CoverObject),
                typeof(GalleryImageView),
                new FrameworkPropertyMetadata(
                        new PropertyChangedCallback(OnCoverUpdate)
                )
        );

        private static async void OnCoverUpdate(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            GalleryImageView self = (GalleryImageView)obj;

            var path = self.Cover.CoverPath;
            var url = self.Cover.CoverUrl;

            self.CoverImage.Source = ImageHelper.LoadBitmapFromResource(self.Cover.CoverPlaceholder);

            var isDownloaded = await Task.Run(() => {

                if (File.Exists(path))
                {
                    return true;
                }

                var dirName = "cache";
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }

                return App.HTTP.DownloadBinary(url, path);
            });
            
            if (isDownloaded)
            {
                try
                {
                    self.CoverImage.Source = ImageHelper.LoadBitmapFromFile(path);
                }
                catch
                {
                    self.CoverImage.Source = ImageHelper.LoadBitmapFromResource(self.Cover.CoverPlaceholder);
                }
            }
        }

        public GalleryImageView()
        {
            InitializeComponent();
        }

        public CoverObject Cover
        {
            get
            {
                return (CoverObject)GetValue(CoverObjectProperty);
            }
            set
            {
                SetValue(CoverObjectProperty, value);
            }
        }
    }
}
