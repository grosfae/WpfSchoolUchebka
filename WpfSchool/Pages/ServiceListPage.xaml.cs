using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ServiceListPage.xaml
    /// </summary>
    public partial class ServiceListPage : Page
    {
        int maxPage = 0;
        public ServiceListPage()
        {
            InitializeComponent();
            CbDiscount.SelectedIndex = 0;
            CbSort.SelectedIndex = 0;
            App.PageName = "Список услуг";
            CbListCount.SelectedIndex = 0;
        }

        int numberPage = 0;
        int count = 3;

        int fakePage = 1;
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            IEnumerable<Service> services = App.DB.Service.Where(x => x.IsDelete != true);
            if (CbSort.SelectedIndex == 0)
                services = services.OrderBy(x => x.CostDiscount);
            else if (CbSort.SelectedIndex == 1)
                services = services.OrderByDescending(x => x.CostDiscount);
            if (CbDiscount.SelectedIndex == 0)
            {
                
            }

            else if (CbDiscount.SelectedIndex == 1)
            {
                services = services.Where(x => x.Discount >= 0 && x.Discount < 0.05 || x.Discount == null).ToList();
            }
            else if (CbDiscount.SelectedIndex == 2)
            {
                services = services.Where(x => x.Discount >= 0.05 && x.Discount < 0.15).ToList();
            }
            else if (CbDiscount.SelectedIndex == 3)
            {
                services = services.Where(x => x.Discount >= 0.15 && x.Discount < 0.30).ToList();
            }
            else if (CbDiscount.SelectedIndex == 4)
            {
                services = services.Where(x => x.Discount >= 0.30 && x.Discount < 0.70).ToList();
            }
            else if (CbDiscount.SelectedIndex == 5)
            {
                services = services.Where(x => x.Discount >= 0.70 && x.Discount < 1).ToList();
            }

            if (CbListCount.SelectedIndex == 0)
            {
                count = 3;

            }
            if (CbListCount.SelectedIndex == 1)
            {
                count = 5;
            }
            if (CbListCount.SelectedIndex == 2)
            {
                count = 10;
            }

            if (TbSearch.Text.Length > 0)
            {
                services = services.Where(x => x.Title.ToLower().Contains(TbSearch.Text.ToLower()));
            }

            if (services.Count() > count)
            {
                maxPage = services.Count() / count;
            }
            else
            {
                maxPage = 1;
            }
            if (fakePage > maxPage)
            {
                fakePage = maxPage;
            }
            services = services.Skip(count * numberPage).Take(count).ToList();

            LvService.ItemsSource = services.ToList();
            TbCounter.Text = $"{fakePage}/{maxPage}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach(var item in App.DB.Service)
            {
                item.MainImagePath = item.MainImagePath.Trim();
                item.Logo = File.ReadAllBytes($"C:/Users/progWeb/Desktop/{item.MainImagePath}");
            }
            App.DB.SaveChanges();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            maxPage = App.DB.Service.Count(x => x.IsDelete != true);
            Refresh();
            if(App.AdminMode == true)
            {
                AddBtn.Visibility = Visibility.Visible;
            }
        }

        private void WriteClientBtn_Click(object sender, RoutedEventArgs e)
        {
                var selectedItem = (sender as Button).DataContext as Service;
                NavigationService.Navigate(new ClientServiceAddEdit(selectedItem));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as Button).DataContext as Service;
            if (MessageBox.Show("Сохранить изменения?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                selectedItem.IsDelete = true;
                App.DB.SaveChanges();
                Refresh();
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as Button).DataContext as Service;
            NavigationService.Navigate(new ServiceAddEdit(selectedItem));
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServiceAddEdit(new Service()));
        }

        private void CbListCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
            numberPage = 0;
            fakePage = 1;
            Refresh();
        }

        private void LeftBtn_Click(object sender, RoutedEventArgs e)
        {
            numberPage--;
            fakePage--;
            if (numberPage < 0)
                numberPage = 0;
            if (fakePage < 1)
                fakePage = 1;
            Refresh();
        }

        private void RightBtn_Click(object sender, RoutedEventArgs e)
        {
            numberPage++;
            fakePage++;
            if (numberPage == maxPage)
            {
                numberPage = maxPage - 1;
                fakePage--;
            }

            if (fakePage < maxPage + 1)
            {
                Refresh();
            }
        }
    }
}
