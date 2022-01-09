using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Research;
using WpfApp1.Control;
using WpfApp1.Database;
using MySql.Data.MySqlClient;


namespace WpfApp1.Control
{
   

    public class PublicationsController
    {

        
        public List<Publication> LoadPublicationsFor(Research.Researcher r)
        {

           return null;
        }

        public List<Research.Publication> LoadPublicationsForID(int id)
        {

            ERDAdapter Adapter1 = new ERDAdapter();
            List<Research.Publication> PublicationList2 = new List<Research.Publication>();
            PublicationList2 = Adapter1.LoadPublications(id);
            List<Publication> sortedPubslist=PublicationList2.OrderBy(p => p.Date).ThenBy(p => p.Title).ToList();
            return sortedPubslist;

        }

       


    }
}
