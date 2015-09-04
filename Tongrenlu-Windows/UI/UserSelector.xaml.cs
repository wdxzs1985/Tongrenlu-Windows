using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UserSelector.xaml
    /// </summary>
    public partial class UserSelector : UserControl
    {

        public static readonly DependencyProperty UserListProperty = DependencyProperty.Register(
                "UserList",
                typeof(List<UserBean>),
                typeof(UserSelector),
                new FrameworkPropertyMetadata(
                        new PropertyChangedCallback(UserSelector.OnUserChanged)
                )
        );

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
                "SelectedIndex",
                typeof(int),
                typeof(UserSelector),
                new FrameworkPropertyMetadata(
                        new PropertyChangedCallback(UserSelector.OnUserChanged)
                )
        );

        private static void OnUserChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UserSelector self = (UserSelector)obj;
            self.User.Content = self.SelectedUserName;

            var path = String.Format("./cache/u{0}_avatar.jpg", self.SelectedUserId);
            ImageHelper.LoadImage(self.Avatar, path, "Assert/ic_account_circle_white_48dp.png");
            
            self.DecreaseIndexButton.IsEnabled = !self.IsFirst;
            self.IncreaseIndexButton.IsEnabled = !self.IsLast;
            
            self.RaiseUserChangedEvent();
        }

        public static readonly RoutedEvent UserChangedEvent = EventManager.RegisterRoutedEvent(
        "UserChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserSelector));

        public event RoutedEventHandler UserChanged
        {
            add { AddHandler(UserChangedEvent, value); }
            remove { RemoveHandler(UserChangedEvent, value); }
        }

        void RaiseUserChangedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(UserChangedEvent);
            RaiseEvent(newEventArgs);
        }

        public UserSelector()
        {
            InitializeComponent();

            DecreaseIndexButton.IsEnabled = !IsFirst;
            IncreaseIndexButton.IsEnabled = !IsLast;

        }

        public List<UserBean> UserList
        {
            get
            {
                return (List<UserBean>)GetValue(UserListProperty);
            }
            set
            {
                SetValue(UserListProperty, value);
            }
        }

        [Browsable(true)]
        public int SelectedIndex
        {
            get
            {
                return (int)GetValue(SelectedIndexProperty);
            }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }

        public UserBean SelectedUser
        {
            get
            {
                if (IsEmpty)
                {
                    return null;
                }
                return UserList[SelectedIndex];
            }
        }


        public long SelectedUserId
        {
            get
            {
                if (IsEmpty)
                {
                    return 0;
                }
                return SelectedUser.id;
            }
        }

        public string SelectedUserName
        {
            get
            {
                if(SelectedUserId == 0)
                {
                    return "new User";
                }
                return String.Format("{0}#{1}", SelectedUser.nickname, SelectedUser.id);
            }
        }

        public bool IsEmpty
        {
            get
            {
                return UserList == null || UserList.Count == 0;
            }
        }

        public bool IsFirst
        {
            get
            {
                if (IsEmpty)
                {
                    return true;
                }
                return SelectedIndex <= 0;
            }
        }

        public bool IsLast
        {
            get
            {
                if (IsEmpty)
                {
                   return true;
                }
                return SelectedIndex >= UserList.Count - 1;
            }
        }

        private void DecreaseIndexButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFirst)
            {
                SelectedIndex--;
            }
        }

        private void IncreaseIndexButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsLast)
            {
                SelectedIndex++;
            }
        }
    }

    
}
