using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Research;
using WpfApp1.Control;
using WpfApp1.Database;

namespace WpfApp1.Research
{
    public enum EmploymentLevel  
    {
        Student,
        A,
        B,
        C,
        D,
        E
    };



    public class Researcher
    {
        public List<Research.Publication> Publications { get; set; }

        private string givenname; 

        public string GivenName
        {
            get
            {
                return givenname;
            }
            set
            {
                if (value != null)
                {
                    givenname = value;
                }
            }
        }

       private string familyname; 

       public string FamilyName
        {
            get
            {
                return familyname;
            }
            set
            {
                if (value != null)
                {
                    familyname = value;
                }
            }
        }

        private int id;
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (value != null)
                {
                    id = value;
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

        private string school; 

        public string School
        {
            get
            {
                return school;
            }
            set
            {
                if (value != null)
                {
                    school = value;
                }
            }
        }

        private string campus; 

        public string Campus
        {
            get
            {
                return campus;
            }
            set
            {
                if (value != null)
                {
                    campus = value;
                }
            }
        }

        private string email; 

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (value != null)
                {
                    email = value;
                }
            }
        }

        private string photoURL; 

        public string PhotoURL
        {
            get
            {
                return photoURL;
            }
            set
            {
                if (value != null)
                {
                    photoURL = value;
                }
            }
        }

        private EmploymentLevel level;
        public EmploymentLevel Level
        {
            get
            {
                return level;
            }
            set
            {
               
                
                level = value;
                
            }
        }
        


        private DateTime startdate; 

        public DateTime StartDate
        {
            get
            {
                return startdate;
            }
            set
            {
                if (value != null)
                {
                    startdate = value;
                }
            }
        }

        private DateTime enddate; 

        public DateTime EndDate
        {
            get
            {
                return enddate;
            }
            set
            {
                if (value != null)
                {
                    enddate = value;
                }
            }
        }

        private DateTime utasstart; 

        public DateTime UtasStart
        {
            get
            {
                return utasstart;
            }
            set
            {
                if (value != null)
                {
                    utasstart = value;
                }
            }
        }

        private DateTime currentstart; 

        public DateTime CurrentStart
        {
            get
            {
                return currentstart;
            }
            set
            {
                if (value != null)
                {
                    currentstart = value;
                }
            }
        }

        public string ToTitle(EmploymentLevel level)
        {
            switch (level) {
                case EmploymentLevel.A:
                    return "Postdoc";
                case EmploymentLevel.B:
                    return "Lecturer";
                case EmploymentLevel.C:
                    return "Senior Lecturer";
                case EmploymentLevel.D:
                    return "Associate Professor";
                case EmploymentLevel.E:
                    return "Professor";
                case EmploymentLevel.Student:
                    return "Student";
                default:
                    return "LEVEL_NOT_FOUND";
            }
        }

       


        public double Tenure()
        {
            DateTime thisday = DateTime.Today;
            TimeSpan t = thisday - UtasStart;
            int t2 = t.Days;
            double t3 = t2 / 365.25;
            return t3;
        }

        public int PublicationsCount()
        {
            PublicationsController P_Cont2 = new PublicationsController();
            List<Publication> p=P_Cont2.LoadPublicationsForID(this.Id);
            return p.Count;
        }

    }
}
