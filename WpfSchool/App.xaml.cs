﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfSchool.Components.Model;

namespace WpfSchool
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SchoolLanguageEntities DB = new SchoolLanguageEntities();
        public static string PageName;
        public static bool AdminMode = false;
    }
}
