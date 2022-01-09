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
    

    public class Publication
    {

        public enum OutputType
    {
        
        Conference,
        Journal,
        Other
    }

        private string doi; 

        public string DOI
        {
            get
            {
                return doi;
            }
            set
            {
                if (value != null)
                {
                    doi = value;
                }
            }
        }

       private string title; 

       public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (value != null)
                {
                    title = value;
                }
            }
        }

       private string authors; 

       public string Authors
        {
            get
            {
                return authors;
            }
            set
            {
                if (value != null)
                {
                    authors = value;
                }
            }
        }

        private int date;
        public int Date
        {
            get
            {
                return date;
            }
            set
            {
                if (value != null)
                {
                    date = value;
                }
            }
        }

        private OutputType type; 

        public OutputType Type
        {
            get
            {
                return type;
            }
            set
            {
                if (value != null)
                {
                    type = value;
                }
            }
        }

        private string citeas; 

        public string CiteAs
        {
            get
            {
                return citeas;
            }
            set
            {
                if (value != null)
                {
                    citeas = value;
                }
            }
        }

        private DateTime availabledate; 

        public DateTime AvailableDate
        {
            get
            {
                return availabledate;
            }
            set
            {
                if (value != null)
                {
                    availabledate = value;
                }
            }
        }

        

        public int Age()
        {
            DateTime today=DateTime.Today;
            int timespan = (today - AvailableDate).Days;
            return timespan;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }


    }
}
