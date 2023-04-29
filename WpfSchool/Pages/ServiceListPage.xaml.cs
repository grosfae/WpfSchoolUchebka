﻿using System;
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
    /// Логика взаимодействия для ServiceListPage.xaml
    /// </summary>
    public partial class ServiceListPage : Page
    {
        public ServiceListPage()
        {
            InitializeComponent();
            LvService.ItemsSource = App.DB.Service.ToList();
            CbDiscount.SelectedIndex = 0;
            CbSort.SelectedIndex = 0;
            App.PageName = "Список услуг";   
        }

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
            IEnumerable<Service> services = App.DB.Service;
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
            
            
            if (TbSearch.Text.Length > 0)
            {
                services = services.Where(x => x.Title.ToLower().StartsWith(TbSearch.Text.ToLower()) || x.Description.ToLower().StartsWith(TbSearch.Text.ToLower()));
            }

            LvService.ItemsSource = services.ToList();
        }
    }
}
