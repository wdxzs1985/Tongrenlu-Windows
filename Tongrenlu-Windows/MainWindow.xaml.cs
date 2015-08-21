using System;
using System.Collections.Generic;
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

namespace Tongrenlu_Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;

            initLoginPanel();
        }

        private void initLoginPanel()
        {
            viewModel.LoginUserList = new List<LoginUser>();
            
            LoginPanel.Visibility = Visibility.Visible;
            MainPanel.Visibility = Visibility.Collapsed;
        }

        private void TrackList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(e.ToString());

            if (e.AddedItems.Count > 0)
            {
                Track track = (Track)e.AddedItems[0];
                Console.WriteLine(track.checksum);


                TrackList.UnselectAll();
            }
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            MainPanel.Visibility = Visibility.Visible;
        }


        private void UserSelector_UserChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
