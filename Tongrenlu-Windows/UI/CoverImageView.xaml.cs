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
    public partial class CoverImageView : UserControl
    {

        public static readonly DependencyProperty CoverObjectProperty = DependencyProperty.Register(
                "Cover",
                typeof(CoverObject),
                typeof(CoverImageView),
                new FrameworkPropertyMetadata(
                        new PropertyChangedCallback(OnCoverUpdate)
                )
        );

        private static void OnCoverUpdate(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CoverImageView self = (CoverImageView)obj;

            var dirName = "cache";
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            var path = self.Cover.CoverPath;
            var url = self.Cover.CoverUrl;
            if (File.Exists(path) || App.HTTP.DownloadBinary(url, path))
            {
                self.CoverImage.Source = ImageHelper.LoadBitmapFromFile(path);
            }
            else
            {
                self.CoverImage.Source = ImageHelper.LoadBitmapFromResource(self.Cover.CoverPlaceholder);
            }
        }

        public CoverImageView()
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
