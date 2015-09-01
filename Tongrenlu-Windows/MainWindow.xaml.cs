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
        public const string HOST = "http://127.0.0.1";
        public const string USERFILE = "user.json";
        public const string FINGERPRINT = "fingerprint";

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

        private async void initLoginPanel()
        {
            _viewModel.LoginUserList = await Task.Run(() =>
            {
                var userList = LoadUserList();
                userList.Add(new UserBean());
                return userList;
            });

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
                var url = String.Format("{0}/fm/library", HOST);
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
            UserBean loginUser = null;

            //_client = new HttpClient();
            _client.clearCookie();

            if (!UserSelector.IsEmpty && UserSelector.SelectedUser.fingerprint != null)
            {
                var fingerprint = UserSelector.SelectedUser.fingerprint;
                _client.addCookie(HOST, FINGERPRINT, fingerprint);
            }

            try
            {
                loginUser = await Task.Run(() => {
                    _client.Get(HOST);

                    var url = String.Format("{0}/signin", HOST);
                    var result = _client.Get(url);

                    var data = JsonConvert.DeserializeObject<SignInInfo>(result);

                    if (data.result)
                    {
                        isLogin = true;
                        return data.loginUser;
                    }
                    return null;
                });

                if (!isLogin)
                {
                    var email = EmailBox.Text;
                    var password = PasswordBox.Password;

                    if (!"".Equals(email) && !"".Equals(password))
                    {
                        var salt = await Task.Run(() => {
                            var url = String.Format("{0}/signin/salt", HOST);
                            var result = _client.Get(url);

                            var data = JsonConvert.DeserializeObject<SignInInfo>(result);
                            return data.salt;
                        });

                        if (salt != null)
                        {
                            loginUser = await Task.Run(() => {
                                var url = String.Format("{0}/signin", HOST);
                                var result = _client.Post(url, new Dictionary<string, string>() {
                            { "email" , email },
                            { "password", CryptHelper.Md5Hex(CryptHelper.Md5Hex(password) + salt) },
                            { "autoLogin", "1" }
                        });

                                var data = JsonConvert.DeserializeObject<SignInInfo>(result);

                                if (data.result)
                                {
                                    isLogin = true;
                                    return data.loginUser;
                                }
                                return null;
                            });
                            
                        }
                    }
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

                    Cookie cookie = _client.findCookie(HOST, FINGERPRINT);
                    if (cookie != null)
                    {
                        loginUser.fingerprint = cookie.Value;
                        if (log.IsDebugEnabled)
                        {
                            log.Debug("fingerprint = " + loginUser.fingerprint);
                        }
                    }

                    _viewModel.LoginUser = loginUser;
                    _viewModel.IsLogin = true;

                    LoginPanel.Visibility = Visibility.Collapsed;
                    MainPanel.Visibility = Visibility.Visible;
                    
                    StartMusicPanel();

                    var userId = loginUser.id;
                    //download user avatar
                    await Task.Run(() => {
                        DownloadUserAvatar(userId);
                    });
                    
                    ImageHelper.LoadImage(UserAvatarButtonImage, String.Format("cache/u{0}_avatar.jpg", userId), "Assert/ic_account_circle_white_48dp.png");

                    await Task.Run(() => {
                        var newUser = new UserBean();
                        newUser.id = loginUser.id;
                        newUser.nickname = loginUser.nickname;
                        newUser.fingerprint = loginUser.fingerprint;
                        
                        SaveUser(newUser);
                    });
                }
            }
        }

        private void UserSelector_UserChanged(object sender, RoutedEventArgs e)
        {
            if(UserSelector.SelectedUserId == 0)
            {
                EmailBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Visible;
            }
            else
            {
                EmailBox.Visibility = Visibility.Hidden;
                PasswordBox.Visibility = Visibility.Hidden;
            }
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        public bool DownloadUserAvatar(long userId)
        {
            var dirname = "cache";
            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }

            var filename = String.Format("u{0}_avatar.jpg", userId);
            var pathString = System.IO.Path.Combine(dirname, filename);
            
            if (File.Exists(pathString))
            {
                return true;
            }

            var url = String.Format("http://files.tongrenlu.info/u{0}/cover_400.jpg", userId);
            return _client.DownloadBinary(url, pathString);
        }


        private List<UserBean> LoadUserList()
        {
            var json = FileHelper.readFileToString(USERFILE);
            if(json == null)
            {
                return new List<UserBean>();
            }

            var userList = JsonConvert.DeserializeObject<List<UserBean>>(json);
            return userList;
        }

        private void SaveUser(UserBean loginUser)
        {
            var userList = LoadUserList().Where(user => user.id != loginUser.id).ToList();
            userList.Insert(0, loginUser);

            string json = JsonConvert.SerializeObject(userList);
            FileHelper.WriteStringToFile(USERFILE, json);
        }

    }
}
