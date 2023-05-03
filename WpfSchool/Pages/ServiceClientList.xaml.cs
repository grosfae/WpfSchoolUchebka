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
using System.Windows.Threading;
using WpfSchool.Components.Model;

namespace WpfSchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServiceClientList.xaml
    /// </summary>
    public partial class ServiceClientList : Page
    {
        public ServiceClientList()
        {
            InitializeComponent();
            App.PageName = "Список записей";
            var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 30);
            dispatcherTimer.Start();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            IEnumerable<ClientService> clients = App.DB.ClientService.OrderByDescending(x => x.StartTime).Where(x => x.IsDelete != true);
            
            if (TbSearch.Text.Length > 0)
            {
                clients = clients.Where(x => x.Service.Title.ToLower().Contains(TbSearch.Text.ToLower()) || x.Client.FullName.ToLower().Contains(TbSearch.Text.ToLower()));
            }
            
            LvService.ItemsSource = clients.Where(x => x.StartTime  < DateTime.Now + TimeSpan.FromDays(2) && x.StartTime > DateTime.Now.Date).ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
            
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as Button).DataContext as ClientService;
            selectedItem.IsDelete = true;
            App.DB.SaveChanges();
            Refresh();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Updating the Label which displays the current second
            
            Refresh();

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

    }
}
