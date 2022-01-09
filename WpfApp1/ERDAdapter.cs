using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using WpfApp1.Research;
using WpfApp1.Control;
using WpfApp1.Database;

namespace WpfApp1.Database
{
    public class ERDAdapter
    {

        public static MySqlConnection conn { get; set; }
        static string db = "kit206";
        static string user = "kit206";
        static string pass = "kit206";
        static string server = "alacritas.cis.utas.edu.au";

        public static T ParseEnum<T>(string value)
        {
           return (T)Enum.Parse(typeof(T), value.Replace(" ","_"));
        }


        public ObservableCollection<Research.Researcher> fetchBasicResearcherDetails()
        {
            conn = GetConnection();
           
            ObservableCollection<Researcher> ResearcherList = new ObservableCollection<Researcher>();
            MySqlDataReader rdr = null;
            

            try
            {
                conn.Open();
                
                MySqlCommand cmd = new MySqlCommand("select id, given_name, family_name, level from researcher", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    // Add the Researchers into ResearcherList as either Staff or Student:
                    // Default is Staff:
                    Staff r = new Staff();
                    r.GivenName = rdr.GetString(1);
                    r.FamilyName = rdr.GetString(2);
                    r.Id = rdr.GetInt32(0);
                   
                    try
                    {
                        r.Level = ERDAdapter.ParseEnum<EmploymentLevel>(rdr.GetString(3));
                        

                    }catch (Exception e)
                    // Note: this works because in the DB, student level is NULL.
                    // So if we get the null exception, make the researcher a Student:
                    {
                        Student r_student=new Student();
                        r_student.Level = EmploymentLevel.Student;
                        r_student.GivenName=rdr.GetString(1);
                        r_student.FamilyName = rdr.GetString(2);
                        r_student.Id = rdr.GetInt32(0);
                        ResearcherList.Add(r_student);
                    }

                    if (r.Level != EmploymentLevel.Student)
                    {
                        r.Level=ERDAdapter.ParseEnum<EmploymentLevel>(rdr.GetString(3));
                        ResearcherList.Add(r);
                    }
                   
                   
                   

                }
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            

            return ResearcherList;

        }

       
        

        public Research.Researcher fullResearcherDetails(int id_num)
        {
            
            MySqlDataReader rdr = null;
            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select id, given_name, family_name, level, title, unit, campus, email, photo, degree, supervisor_id, utas_start, current_start from researcher where id=?id_num", conn);
                cmd.Parameters.AddWithValue("id_num",id_num);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    Staff r = new Staff();
                    
                   
                    try
                    {
                        r.Level = ERDAdapter.ParseEnum<EmploymentLevel>(rdr.GetString(3));
                        

                    }catch (Exception e)
                    // Note: this works because in the DB, student level is NULL.
                    // So if we get the null exception, make the researcher a Student:
                    {
                        Student r_student=new Student();
                        r_student.Level = EmploymentLevel.Student;
                        r_student.GivenName=rdr.GetString(1);
                        r_student.FamilyName = rdr.GetString(2);
                        r_student.Id = rdr.GetInt32(0);
                        r_student.Id=rdr.GetInt32(0);
                        r_student.GivenName = rdr.GetString(1);
                        r_student.FamilyName = rdr.GetString(2);
                        r_student.Title = rdr.GetString(4);
                        r_student.School = rdr.GetString(5);
                        r_student.Campus = rdr.GetString(6);
                        r_student.Email = rdr.GetString(7);
                        r_student.PhotoURL = rdr.GetString(8);
                        r_student.UtasStart=rdr.GetDateTime(11);
                        r_student.CurrentStart=rdr.GetDateTime(12);
                        r_student.Degree=rdr.GetString(9);
                        r_student.SupervisorID=rdr.GetString(10);
                        
                        return r_student;
                    }

                    if (r.Level != EmploymentLevel.Student)
                    {
                         r.Id=rdr.GetInt32(0);
                         r.GivenName = rdr.GetString(1);
                         r.FamilyName = rdr.GetString(2);
                         r.Title = rdr.GetString(4);
                         r.School = rdr.GetString(5);
                         r.Campus = rdr.GetString(6);
                         r.Email = rdr.GetString(7);
                         r.PhotoURL = rdr.GetString(8);
                         r.UtasStart=rdr.GetDateTime(11);
                         r.CurrentStart=rdr.GetDateTime(12);
                         r.Level=ERDAdapter.ParseEnum<EmploymentLevel>(rdr.GetString(3));
                         return r;
                    }
                   
                   
                   


                }
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public List<Research.Publication> LoadPublications(int Id)
        {
            conn = GetConnection();
            List<Research.Publication> TestPubList = new List<Research.Publication>();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

               
                // This is a clunky way of doing it, but I don't have time to tighten up the code.
                MySqlCommand cmd = new MySqlCommand("select pub.doi, title, authors,  year, type, cite_as, available from publication as pub, researcher_publication as respub where pub.doi = respub.doi and researcher_id=?id", conn);
                cmd.Parameters.AddWithValue("id", Id);

                // 

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    TestPubList.Add(new Publication
                        { DOI=rdr.GetString(0), Title = rdr.GetString(1), Authors=rdr.GetString(2), Date=rdr.GetInt32(3) , Type=Publication.ParseEnum<Publication.OutputType>(rdr.GetString(4)), CiteAs=rdr.GetString(5), AvailableDate=rdr.GetDateTime(6) });//
                                                                                                                                                     
                }
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
           

            return TestPubList;
        }


        public List<Research.Publication> fetchBasicPublicationDetails(Research.Researcher r)
        {
             conn = GetConnection();
           
            List<Publication> PublicationsList = new List<Publication>();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select * from researcher_publication where id=?id_num", conn);
                cmd.Parameters.AddWithValue("id_num",r.Id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PublicationsList.Add(new Publication { DOI=rdr.GetString(1) });
                   
                }
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return PublicationsList;
            
        }
        
        public Publication completePublicationDetails(string doiString)
        {
            conn = GetConnection();
            Publication p_out = new Publication();
            Console.WriteLine("Entered the method.\n");
            //Console.WriteLine("The Title is {0}", p.Title);
            Console.WriteLine("The DOI is " + doiString);
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();
                Console.WriteLine("Trying...");
               
                MySqlCommand cmd = new MySqlCommand("select * from publication where doi=?doi_string", conn);
               

                cmd.Parameters.AddWithValue("doi_string", doiString);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    Console.WriteLine("rdr has rows");
                }
                while (rdr.Read())
                {
                   
                    p_out.Title = rdr.GetString(0);
                    Console.WriteLine("p_out.Title is:", p_out.Title);
                   
                    Console.WriteLine("Iterating...");
                }

            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }


            return p_out;

        }

        public Publication fetchPublicationCounts(DateTime from, DateTime to)
        {
            return null;
        }

        private static MySqlConnection GetConnection()
        {
            if (conn == null)
            {
                string connectionString = String.Format("Database={0};Data Source={1};User Id={2}; Password={3}", db, server, user, pass);
                conn = new MySqlConnection(connectionString);
            }
            return conn;
        }



        
    public void getSupervisions(int id_num)
        {
            
            MySqlDataReader rdr = null;
            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select given_name, family_name from researcher where supervisor_id=?id_num", conn);
                cmd.Parameters.AddWithValue("id_num",id_num);
                rdr = cmd.ExecuteReader();
                Console.WriteLine("Students under supervision by this Researcher:");
                while (rdr.Read())
                {

                   
                       Console.WriteLine(rdr.GetString(0)+" "+rdr.GetString(1));
                        

                }
                  
                
            }
            finally
            {
                 if (rdr != null)
                 {
                     rdr.Close();
                 }
                 if (conn != null)
                 {
                     conn.Close();
                 }
            }
           
        }


        public void getPastPositions(int id_num)
        {
            
            MySqlDataReader rdr = null;
            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select start,end, level from position where id=?id_num", conn);
                cmd.Parameters.AddWithValue("id_num",id_num);
                rdr = cmd.ExecuteReader();
                Console.WriteLine("Previous positions of this Researcher:");
                while (rdr.Read())
                {

                       Console.WriteLine("Position:" + ERDAdapter.ParseEnum<EmploymentLevel>(rdr.GetString(2)));
                       Console.WriteLine("Start:" + rdr.GetString(0));
                      
                       if (!rdr.IsDBNull(1))
                        {
                          Console.WriteLine("End:"+rdr.GetString(1));
                        }
                       else
                       {
                        Console.WriteLine("Still in that position.");
                       }
                   
                }
                    

                
            }
            finally
            {
                 if (rdr != null)
                 {
                     rdr.Close();
                 }
                 if (conn != null)
                 {
                     conn.Close();
                 }
            }
           
        }
    }


}
