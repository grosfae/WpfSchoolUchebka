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
using WpfSchool.Pages;

namespace WpfSchool
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ServiceListPage());
            TbTitlePage.Text = App.PageName.ToString();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(App.AdminMode == true)
            {
                MessageBox.Show("Админка уже включена");
                return;
            }    
            if(TbCode.Text == "0000")
            {
                MessageBox.Show("Админка включена");
                App.AdminMode = true;
                MainFrame.Navigate(new ServiceListPage());
                ServiceClientListBtn.Visibility = Visibility.Visible;
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            TbTitlePage.Text = App.PageName.ToString();
        }

        private void ServiceListBtn_Click(object sender, RoutedEventArgs e)
        {
            App.PageName = "Список услуг";
            MainFrame.Navigate(new ServiceListPage());
        }

        private void ServiceClientListBtn_Click(object sender, RoutedEventArgs e)
        {
            App.PageName = "Список записей";
            MainFrame.Navigate(new ServiceClientList());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
