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
        public ServiceAddEdit(Service service)
        {
            InitializeComponent();
            contextService = service;
            DataContext = contextService;
            contextService.DurationInSeconds /= 60;
            contextService.Discount *= 100;
            LvAdditionImages.ItemsSource = contextService.ServicePhoto;
            App.PageName = "Редактирование услуги";
        }


        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            contextService.DurationInSeconds *= 60;
            contextService.Discount /= 100;
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
            if (contextService.DurationInSeconds <= 0 || contextService.DurationInSeconds > 999)
            {
                errorMessage += "Введите корректное время\n";
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
                contextService.DurationInSeconds *= 60;
                contextService.Discount /= 100;
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
                LvAdditionImages.ItemsSource = contextService.ServicePhoto;
            }
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
                LvAdditionImages.ItemsSource = contextService.ServicePhoto;
            }
        }
    }
}
