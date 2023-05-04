using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
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
        int maxPage = 0;
        public ServiceAddEdit(Service service)
        {
            InitializeComponent();
            contextService = service;
            DataContext = contextService;
            LvAdditionImages.ItemsSource = contextService.ServicePhoto;
            App.PageName = "Редактирование услуги";
            TbInMinutes.Text = contextService.DurationInMinutes.ToString();
            TbDicsount.Text = contextService.DiscountInDouble.ToString();
        }

        int numberPage = 0;
        int count = 4;

        int fakePage = 1;
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServiceListPage());
        }

        private void ImageAddBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            {
                dialog.Filter = "*.png|*.png|*.jpg|*.jpg|*.jpeg|*.jpeg";
            };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                contextService.Logo = System.IO.File.ReadAllBytes(dialog.FileName);
                DataContext = null;
                DataContext = contextService;
            }
        }
        private void Letters_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[A-zА-я]") == false)
            {
                e.Handled = true;
            }
        }
        private void Numbers_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[0-9]") == false)
            {
                e.Handled = true;
            }
        }
        private void ForSpaces_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = "";
            if (string.IsNullOrWhiteSpace(contextService.Title))
            {
                errorMessage += "Введите название\n";
            }
            if (contextService.Cost <= 0 || contextService.Cost >999999)
            {
                errorMessage += "Введите корректную цену\n";
            }
            if (TbInMinutes.Text == String.Empty || int.Parse(TbInMinutes.Text) <= 0 || int.Parse(TbInMinutes.Text) > 240)
            {
                errorMessage += "Введите корректное время\n";
            }

            if (TbDicsount.Text == String.Empty)
            {

            }
            if (string.IsNullOrWhiteSpace(errorMessage) == false)
            {
                MessageBox.Show(errorMessage);
                return;
            }
            if (contextService.ID == 0)
            {
                foreach (var item in App.DB.Service)
                {
                    if (item.Title == contextService.Title)
                    {
                        MessageBox.Show("Услуга с таким названием уже существует");
                        return;
                    }

                }
            }

            if (MessageBox.Show("Сохранить изменения?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                contextService.DurationInSeconds = int.Parse(TbInMinutes.Text)  * 60;
                contextService.Discount = int.Parse(TbDicsount.Text) / 100;
                if (contextService.ID == 0)
                {
                    App.DB.Service.Add(contextService);
                }
                App.DB.SaveChanges();
                NavigationService.Navigate(new ServiceListPage());

            }
        }

        private void AddImageAddition_Click(object sender, RoutedEventArgs e)
        {
            if (contextService.ID == 0)
            {
                MessageBox.Show("Сохраните данные о корпусе перед добавлением доп. изображений");
                return;
            }
            ServicePhoto serPhoto = new ServicePhoto();
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                serPhoto.PhotoPath = File.ReadAllBytes(dialog.FileName);
                serPhoto.ServiceID = contextService.ID;
                App.DB.ServicePhoto.Add(serPhoto);
                App.DB.SaveChanges();
                
            }
            Refresh();
        }

        private void RemoveAdditionImage_Click(object sender, RoutedEventArgs e)
        {
            var selectedPhoto = LvAdditionImages.SelectedItem as ServicePhoto;
            if (selectedPhoto == null)
            {
                MessageBox.Show("Выберите дополнительное изображение");
                return;
            }
            if (MessageBox.Show("Удалить фото?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                App.DB.ServicePhoto.Remove(selectedPhoto);
                App.DB.SaveChanges();
                
            }
            numberPage = 0;
            fakePage = 1;
            Refresh();
        }

        private void Refresh()
        {
            var services = contextService.ServicePhoto;
            maxPage = services.Count();
            if (services.Count() > count)
            {
                if (services.Count() % count > 0)
                {
                    maxPage = (services.Count() / count) + 1;
                }
                else
                {
                    maxPage = services.Count() / count;
                }
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

            LvAdditionImages.ItemsSource = services.ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            maxPage = contextService.ServicePhoto.Count();
            Refresh();
        }

        private void BtnLeft_Click(object sender, RoutedEventArgs e)
        {
            numberPage--;
            fakePage--;
            if (numberPage < 0)
                numberPage = 0;
            if (fakePage < 1)
                fakePage = 1;
            Refresh();
        }
        private void BtnRight_Click(object sender, RoutedEventArgs e)
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
