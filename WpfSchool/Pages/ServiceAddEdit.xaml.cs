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
using WpfSchool.Components.Model;

namespace WpfSchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServiceAddEdit.xaml
    /// </summary>
    public partial class ServiceAddEdit : Page
    {
        Service contextService;
        public ServiceAddEdit(Service service)
        {
            InitializeComponent();
            contextService = service;
            DataContext = contextService;
        }

        private void LeftBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RightBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Content= new ServiceListPage();
        }
    }
}
