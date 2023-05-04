using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfSchool.Components.Model;

namespace WpfSchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientServiceAddEdit.xaml
    /// </summary>
    public partial class ClientServiceAddEdit : Page
    {
        Service contextClientService;
        public ClientServiceAddEdit(Service clientService)
        {
            InitializeComponent();
            contextClientService = clientService;
            DataContext = contextClientService;
            CbClient.ItemsSource = App.DB.Client.ToList();
            DtDate.Text = DateTime.Now.ToString();
            TbTimeOfTable.Text = DateTime.Now.ToString("t");
            App.PageName = "Запись клиента";

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string errorMessage = "";
                if (CbClient.SelectedItem == null)
                {
                    errorMessage += "Выберите клиента\n";
                }
                if (string.IsNullOrWhiteSpace(TbTimeOfTable.Text))
                {
                    errorMessage += "Введите корректное время\n";
                }
            
                string TimeItog = DtDate.Text + " " + TbTimeOfTable.Text;
                if (string.IsNullOrWhiteSpace(DtDate.Text) || DateTime.Parse(TimeItog) < DateTime.Now.AddMinutes(2))
                {
                    errorMessage += "Введите корректную дату или время\n";
                }

                if (string.IsNullOrWhiteSpace(errorMessage) == false)
                {
                    MessageBox.Show(errorMessage);
                    return;
                }

           
                if (MessageBox.Show("Сохранить изменения?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (contextClientService.ID != 0)
                    {
                        ClientService clientService = new ClientService();
                        clientService.ServiceID = contextClientService.ID;
                        var client = CbClient.SelectedItem as Client;
                        clientService.ClientID = client.ID;
                        clientService.StartTime = DateTime.Parse(TimeItog);
                        App.DB.ClientService.Add(clientService);
                    }
                    App.DB.SaveChanges();
                    NavigationService.GoBack();
                }
            }
            catch
            {
                MessageBox.Show("Вы выбрали не верное время");
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void Numbers_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[0-9:]") == false)
            {
                e.Handled = true;
            }
        }

        private void Date_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[0-9.]") == false)
            {
                e.Handled = true;
            }
        }
    }
}
