using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Research;
using WpfApp1.Control;
using WpfApp1.Database;

namespace WpfApp1.Research
{

    public class Student : Researcher
    {
   
        private string degree; 

        public string Degree
        {
            get
            {
                return degree;
            }
            set
            {
                if (value != null)
                {
                    degree = value;
                }
            }
        }

        private string supervisorID; 

        public string SupervisorID
        {
            get
            {
                return supervisorID;
            }
            set
            {
                if (value != null)
                {
                    supervisorID = value;
                }
            }
        }

    }
}
