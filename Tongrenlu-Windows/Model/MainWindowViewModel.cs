using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tongrenlu_Windows.Data;

namespace Tongrenlu_Windows.Model
{
    public class MainWindowViewModel : INotifyPropertyChanged
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

        private List<UserBean> _loginUserList;
        public List<UserBean> LoginUserList
        {
            get
            {
                return _loginUserList;
            }
            set
            {
                _loginUserList = value;
                NotifyPropertyChanged("LoginUserList");
            }
        }

        private List<MusicBean> _musicList;
        public List<MusicBean> MusicList
        {
            get
            {
                return _musicList;
            }
            set
            {
                _musicList = value;
                NotifyPropertyChanged("MusicList");
            }
        }

        private List<TrackBean> _trackList;
        public List<TrackBean> TrackList
        {
            get
            {
                return _trackList;
            }
            set
            {
                _trackList = value;
                NotifyPropertyChanged("TrackList");
            }
        }

        private bool _isLogin;
        public bool IsLogin
        {
            get
            {
                return _isLogin;
            }
            set
            {
                _isLogin = value; NotifyPropertyChanged("IsLogin");
            }
        }

        private UserBean _loginUser;
        public UserBean LoginUser
        {
            get
            {
                return _loginUser;
            }
            set
            {
                _loginUser = value;
                NotifyPropertyChanged("LoginUser");
            }
        }

        public ICommand ItemClickedCommand { get; private set; }
    }
}
