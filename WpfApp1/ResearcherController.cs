using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Research;
using WpfApp1.Control;
using WpfApp1.Database;
using MySql.Data.MySqlClient;



namespace WpfApp1.Control
{
   

    public class ResearcherController
    {

        //List<Researcher> ResearcherList=new List<Researcher>();
        //List<Researcher> filteredList = new List<Researcher>();
        public ObservableCollection<Researcher> ResearcherList = new ObservableCollection<Researcher>();
        public ObservableCollection<Researcher> filteredList = new ObservableCollection<Researcher>();

        public ResearcherController()
        {
                  ResearcherList = LoadResearchers();
        }

        public Researcher Use (int id)
        {
            foreach (Researcher currentResearcher in ResearcherList)
            {
                if (currentResearcher.Id == id)
                {
                    return currentResearcher;
                }
            }

            return null;
        }


        public void Display()
        {
            foreach(Researcher r in ResearcherList)
            {
                Console.WriteLine("{0}  {1}   {2}   {3}", r.Id, r.GivenName, r.FamilyName, r.Level);
            }
        }

        public void DisplayFilteredList()
        {
            foreach(Researcher r in filteredList)
            {
                Console.WriteLine("{0}  {1}   {2}   {3}", r.Id, r.GivenName, r.FamilyName, r.Level);
            }
        }

        public void DisplayDetails(Researcher res1)
        {
            Console.WriteLine("ID:"+res1.Id);
            Console.WriteLine("Family Name:"+res1.FamilyName);
            Console.WriteLine("Given Name:"+res1.GivenName);
            Console.WriteLine("Title:"+res1.Title);
            Console.WriteLine("Campus:"+res1.Campus);
            Console.WriteLine("School:"+res1.School);
            Console.WriteLine("Email:"+res1.Email);
            Console.WriteLine("PhotoURL:"+res1.PhotoURL);
            Console.WriteLine("Level:"+res1.ToTitle(res1.Level));
            Console.WriteLine("Began current role: {0}", res1.CurrentStart);
            Console.WriteLine("Tenure:" + (res1.Tenure()).ToString("N2") + " years");
            Console.WriteLine("Total publications:" + res1.PublicationsCount());
            if (res1.Level==EmploymentLevel.Student)
            {
                Console.WriteLine("Degree:" + ((Student)res1).Degree);
                Console.WriteLine("Supervisor ID:" + ((Student)res1).SupervisorID);
                int superID = Int32.Parse(((Student)res1).SupervisorID);
                var superName = from rs in ResearcherList
                                    where rs.Id == superID
                                    select rs;
                
                superName.ToList().ForEach(a => Console.WriteLine("Supervisor Name: " + a.GivenName + " "+ a.FamilyName));
               
            }
           

           
        }

        public ObservableCollection<Researcher> LoadResearchers()
        { 
            
            ERDAdapter Adapter1 = new ERDAdapter();
            ObservableCollection<Researcher> tempList = new ObservableCollection<Researcher>();
            tempList = Adapter1.fetchBasicResearcherDetails();

            return tempList;
        }

        public void FilterByLevel(Research.EmploymentLevel level)
        {
            filteredList.Clear();
            foreach (Researcher r in ResearcherList)
            {
                if (r.Level == level)
                {
                    filteredList.Add(r);
                }
            }
            return;
        }

        public void FilterByName(string name)
        {
            filteredList.Clear();
            foreach (Researcher r in ResearcherList)
            {
                string fullName = r.GivenName + r.FamilyName;
                if (fullName.IndexOf(name, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    filteredList.Add(r);
                }
            }
            return;
        }

        public void LoadResearcherDetails(int IDnum)
        {
            ERDAdapter ad1=new ERDAdapter();
            Researcher r2=ad1.fullResearcherDetails(IDnum);
            PublicationsController P_Cont2 = new PublicationsController();
            r2.Publications=P_Cont2.LoadPublicationsForID(IDnum);
            
            DisplayDetails(r2);
            if (r2.Level != EmploymentLevel.Student)
            {
                double threeYrAvg = ((Staff)r2).calc3yrAvg(r2.Publications);
                double staff_perf = ((Staff)r2).performance(r2.Level,threeYrAvg);

                Console.WriteLine("Performance for this staff member:" +  staff_perf.ToString("N1") + "%");

            }

            if (r2 is WpfApp1.Research.Staff)
                { 
                    Console.WriteLine("\nWould you like full list of students being supervised by this researcher? (y)es or (n)o.");
                    string option2 = Console.ReadLine();
                    if (option2 == "y")
                    {      
                        ad1.getSupervisions(r2.Id);
                
                    }


                    Console.WriteLine("\nWould you like the previous positions of this Staff member? (y)es or (n)o.");
                    string option3=Console.ReadLine();
                    if (option3=="y")
                    {
                       ad1.getPastPositions(r2.Id);
                    }

                }

            Console.WriteLine("\nPublications of Researcher ID: {0} \n", IDnum);
            foreach (Publication p in r2.Publications)
            {
                Console.WriteLine("{0} : {1}", p.Date, p.Title);
            }

            Console.WriteLine("Would you like to view this list in reverse order? (y)es or (n)o.");
            string option12 = Console.ReadLine();
            if (option12 == "y")
            {
                //List<Publication> reversedPubs=new List<Publication>();
                r2.Publications.Reverse();
                foreach (Publication p in r2.Publications)
                {
                    Console.WriteLine("{0} : {1}", p.Date, p.Title);
                }
            }

            Console.WriteLine("\nWould you like to filter this list by year? (y)es or (n)o.");
            string option11 = Console.ReadLine();
            if (option11 == "y")
            {
                Console.WriteLine("Enter the year:");
                int year_selection = Convert.ToInt32(Console.ReadLine());
                var pubs_year_selected=from publ in r2.Publications
                                 where publ.Date == year_selection
                                 select publ;

                pubs_year_selected.ToList().ForEach(a => Console.WriteLine("Date: " + a.Date + "\nTitle:"+ a.Title));
            }

            Console.WriteLine("\nWould you like full details of one of these publications? (y)es or (n)o.");
            string option1 = Console.ReadLine();
            if (option1 == "y")
            { 
                // Full details for a publication:
                Console.WriteLine("Choose a publication to view full details: Choose 1 to {0}", r2.Publications.Count);
            
                int pub_selection = Convert.ToInt32(Console.ReadLine());
                pub_selection=pub_selection-1;
                Console.WriteLine("You selected: {0}", pub_selection);
                Console.WriteLine("Its DOI is: {0}", (r2.Publications[pub_selection]).DOI);
                var pub_selected=from publ in r2.Publications
                                 where publ.DOI == (r2.Publications[pub_selection]).DOI
                                 select publ;

                
                pub_selected.ToList().ForEach(a => Console.WriteLine("DOI: " + a.DOI + "\nTitle:"+ a.Title + "\nAuthors:"+a.Authors + "\nPublication Year:"+a.Date + "\nType:"+a.Type+"\nCitation:"+a.CiteAs+"\nAvailable:"+a.AvailableDate));
                Console.WriteLine("Age is:"+(r2.Publications[pub_selection]).Age()+" days.\n");
               
            }


            Console.WriteLine("\nWould you like the Cumulative Count? (y)es or (n)o.");
            string option = Console.ReadLine();
            if (option == "y")
            {
                CumulativeCount(r2);
            }
        }

        public void CumulativeCount(Researcher r, int startYear = 2015, int endYear = 2021)
        {
            Console.WriteLine("Cumulative Count of Research by Year:\n");

            for (int i = 0; i <= endYear-startYear; i++)
            {
                List<Publication> rPubs = r.Publications;
                var counter =
                    from pub in rPubs
                    where pub.Date == startYear + i
                    select pub;
                int count = counter.Count();
               
                Console.WriteLine("Year: {0}           |  Count: {1}       ", startYear+i, count);
            }

            return;
        }

        public void AchievementReport()
        {
            double minval=0;
            double maxval=70;
            string category="";
            
            Console.WriteLine("Please Select a Performance option: \n 1:Poor \n 2:Below expectations \n 3:Meeting minimum  \n 4:Star performers \n 0: Quit");
            string option = Console.ReadLine();
            switch(option)
            {
                case "1":
                    minval=-1;
                    maxval=70;
                    category="Poor";
                    break;
                case "2":
                    minval=70;
                    maxval=110;
                    category="Below expectations";
                    break;
                case "3":
                    minval=110;
                    maxval=200;
                    category="Meeting minimum";
                    break;
                case "4":
                    minval=200;
                    maxval=1000;
                    category="Star Performer";
                    break;
                case "0":
                    return;
                    break;
                default:
                    Console.WriteLine("Not a valid option, please enter a valid option \n");
                    break;
            }
            PublicationsController P_Cont3 = new PublicationsController();
            Console.WriteLine("\nAchievement Report");
            Console.WriteLine(category + " performance:\n");
            foreach (Researcher r in ResearcherList)
            {
                if (r.Level != EmploymentLevel.Student)
                {
                    
                    r.Publications = P_Cont3.LoadPublicationsForID(r.Id);
                    double threeYAvg = ((Staff)r).calc3yrAvg(r.Publications);
                    double perf = ((Staff)r).performance(r.Level, threeYAvg);
                    
                    if (perf > minval && perf <= maxval)
                    { 
                        Console.WriteLine("Name:" + r.GivenName + " " + r.FamilyName);
                        Console.WriteLine("Performance Metric:" + perf + "\n");
                    }
                }

            }
        }





    }
}