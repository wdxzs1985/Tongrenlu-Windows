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
    public class UserList
    {
        public List<UserBean> list { get; set; }
    }

    public class UserBean : INotifyPropertyChanged, CoverObject
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

        public long id { get; set; }

        public string nickname { get; set; }

        public bool isGuest { get; set; }

        public bool isMemeber { get; set; }

        public string fingerprint { get; set; }
        
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

    public class SignInInfo
    {
        public string salt { get; set; }

        public bool result { get; set; }

        public UserBean loginUser { get; set; }


    }
}
