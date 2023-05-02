using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSchool.Components.Model
{
    public partial class ClientService
    {
        public string Color
        {
            get
            {

                
                if ( (StartTime - DateTime.Now) < TimeSpan.FromHours(1))
                {
                    return "red";
                }
                else
                {
                    return "white";
                }
            }
        }

        public TimeSpan TimeHas
        {
            get
            {
                TimeSpan Timer = new TimeSpan();
                 Timer = (StartTime - DateTime.Now);
  
                return Timer;


            }
        }
    }
}
