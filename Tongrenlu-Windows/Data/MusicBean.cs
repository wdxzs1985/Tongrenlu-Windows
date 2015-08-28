using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tongrenlu_Windows.Data
{
    public class MusicBean : INotifyPropertyChanged, CoverObject
    {
        #region Interface
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public string id { get; set; }
        public long articleId { get; set; }
        public string title { get; set; }

        public string CoverPath
        {
            get
            {
                var path = String.Format("cache/m{0}_cover.jpg", id);
                return path;
            }
        }

        public string CoverUrl
        {
            get
            {
                var url = String.Format("http://files.tongrenlu.info/m{0}/cover_400.jpg", id);
                return url;
            }
        }

        public string CoverPlaceholder
        {
            get
            {
                return "Assert/cover.jpg";
            }
        }
    }

    public class MusicPage : PageSupport
    {
        public List<MusicBean> items;
    }

    public class MusicResultModel
    {
        public MusicPage page { get; set; }
    }
}
