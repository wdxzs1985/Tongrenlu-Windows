using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
using Tongrenlu_Windows.Http;
using Tongrenlu_Windows.Tools;

namespace Tongrenlu_Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string HOST = "127.0.0.1";

        public static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));

        private MainWindowViewModel _viewModel;
        
        private HttpClient _client;
        
        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainWindowViewModel();
            this.DataContext = _viewModel;

            _client = App.HTTP;

            initLoginPanel();
        }

        private void initLoginPanel()
        {
            _viewModel.LoginUserList = new List<UserBean>();
            
            LoginPanel.Visibility = Visibility.Visible;
            MainPanel.Visibility = Visibility.Collapsed;
            
        }


        public void StartMusicPanel()
        {
            LoadMusicList();
        }

        private async void LoadMusicList()
        {
            await Task.Run(() => {
                var url = String.Format("http://{0}/fm/library", HOST);
                var result = _client.Get(url);

                var data = JsonConvert.DeserializeObject<MusicResultModel>(result);
                
                _viewModel.MusicList = data.page.items;
                
                foreach(var musicBean in data.page.items)
                {

                }
            });
        }

        private void TrackList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            log.Debug(e.ToString());

            if (e.AddedItems.Count > 0)
            {
                TrackBean track = (TrackBean)e.AddedItems[0];
                log.Debug(track.checksum);


                TrackList.UnselectAll();
            }
        }


        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;
            var isLogin = false;
            try
            {
                var salt = await Task.Run(() => {
                    var url = String.Format("http://{0}/signin/salt", HOST);
                    var result = _client.Get(url);

                    var data = JsonConvert.DeserializeObject<SignInInfo>(result);
                    return data.salt;
                });

                log.Debug("salt = " + salt);

                if (salt != null)
                {
                    var email = "foobar@foobar.com";
                    var password = "111111";

                    var loginUser = await Task.Run(() => {
                        var url = String.Format("http://{0}/signin", HOST);
                        var result = _client.Post(url, new Dictionary<string, string>() {
                            { "email" , email },
                            { "password", CryptHelper.Md5Hex(CryptHelper.Md5Hex(password) + salt) },
                            { "autoLogin", "1" }
                        });

                        var data = JsonConvert.DeserializeObject<SignInInfo>(result);

                        if(data.result)
                        {
                            return data.loginUser;
                        }
                        return null;
                    });

                    Cookie cookie = _client.findCookie(String.Format("http://{0}", HOST), "fingerprint");
                    if (cookie != null)
                    {
                        loginUser.fingerprint = cookie.Value;
                    }

                    _viewModel.LoginUser = loginUser;
                    isLogin = true;
                }
            }
            catch
            {
                isLogin = false;
            }
            finally
            {
                LoginButton.IsEnabled = true;

                if (isLogin)
                {
                    LoginPanel.Visibility = Visibility.Collapsed;
                    MainPanel.Visibility = Visibility.Visible;

                    _viewModel.IsLogin = true;

                    StartMusicPanel();

                    var userId = _viewModel.LoginUser.id;
                    //download user avatar
                    await Task.Run(() => {
                        DownloadUserAvatar(userId);
                    });
                    
                    ImageHelper.LoadImage(UserAvatarButtonImage, String.Format("cache/u{0}/cover_120.jpg", userId), "Assert/ic_account_circle_white_48dp.png");
                }
            }
        }
        
        private void UserSelector_UserChanged(object sender, RoutedEventArgs e)
        {

        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        public bool DownloadUserAvatar(long userId)
        {
            var dirname = String.Format("cache/u{0}", userId);
            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }

            var filename = "cover_120.jpg";
            var pathString = System.IO.Path.Combine(dirname, filename);

            if (File.Exists(pathString))
            {
                return true;
            }

            var url = String.Format("http://files.tongrenlu.info/u{0}/cover_120.jpg", userId);
            return _client.DownloadBinary(url, pathString);
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
