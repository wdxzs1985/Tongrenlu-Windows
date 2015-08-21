﻿using System;
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

namespace Tongrenlu_Windows.UI
{
    /// <summary>
    /// Interaction logic for UserSelector.xaml
    /// </summary>
    public partial class UserSelector : UserControl
    {

        public static readonly DependencyProperty UserListProperty = DependencyProperty.Register(
        "UserList",
        typeof(List<LoginUser>),
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
            RoutedEventArgs newEventArgs = new RoutedEventArgs(UserSelector.UserChangedEvent);
            RaiseEvent(newEventArgs);
        }

        public UserSelector()
        {
            InitializeComponent();

            DecreaseIndexButton.IsEnabled = !IsFirst;
            IncreaseIndexButton.IsEnabled = !IsLast;

        }

        public List<LoginUser> UserList
        {
            get
            {
                return (List<LoginUser>)GetValue(UserListProperty);
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

        public LoginUser SelectedUser
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


        public string SelectedUserName
        {
            get
            {
                if(IsEmpty)
                {
                    return "";
                }
                return UserList[SelectedIndex].name;
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
