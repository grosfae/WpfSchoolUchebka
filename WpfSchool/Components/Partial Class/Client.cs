using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSchool.Components.Model
{
    public partial class Client
    {
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName} {Patronymic}";
            }
        }

        public Visibility AdminVisible
        {
            get
            {
                if (App.AdminMode == true)
                {
                    return Visibility.Visible;
                }
                else
                    return Visibility.Collapsed;
            }
        }
    }
}
