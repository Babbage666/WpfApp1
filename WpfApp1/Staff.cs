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
   
    public class Staff : Researcher
    {


        public double calc3yrAvg(List<Research.Publication> inputList)
        {

            DateTime thisday = DateTime.Today;
            int thisyear = thisday.Year;
            int count = 0;
            foreach (Publication p in inputList)
            {
                if (thisyear - p.Date < 3)
                    count = count + 1;

            }
           
            return (double)(count / 3);
        }


        public double performance(EmploymentLevel lvl, double threeYavg)
        {
            double expected;
            if (lvl == EmploymentLevel.A)
            {
                expected = 0.5;
            }
            else if (lvl == EmploymentLevel.B)
            {
                expected = 1;
            }
            else if (lvl == EmploymentLevel.C)
            {
               expected = 2;
            }
            else if (lvl == EmploymentLevel.D)
            {
                expected = 3.2;
            }
            else
            {
                expected = 4;
            }

            return (threeYavg / expected) * 100;
        }


    }
}
