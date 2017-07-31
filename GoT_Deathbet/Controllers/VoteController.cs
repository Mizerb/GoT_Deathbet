using GoT_Deathbet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace GoT_Deathbet.Controllers
{
    public class VoteController : ApiController
    {
        int K_Factor = 32;
        const string database_name = @"C:\CODE\git\GoT_Deathbet\GoT_Deathbet\App_Data\db.sqlite";
        //private SQLite.SQLiteAsyncConnection db = new SQLite.SQLiteAsyncConnection(database_name);
        private Random dude = new Random();
        //private SortedList<int, Candidate> Rankings = (new SQLite.SQLiteAsyncConnection(database_name)).Table<Candidate>().ToListAsync()
        
        [HttpGet]
        public JsonResult<Candidate[]> Get_Duel()
        {
            using (SQLite.SQLiteConnection db = new SQLite.SQLiteConnection(database_name))
            {
                var Ret = new Candidate[2];
                int count = db.Table<Candidate>().Count();
                int left = dude.Next(count);
                int right;
                do
                {
                    right = dude.Next(count);
                } while (right == left);
                Ret[0] = db.Table<Candidate>().ElementAt(right);

                Ret[1] = db.Table<Candidate>().ElementAt(left);

                return Json(Ret);

            }
            /*
        return Json(new Candidate[]
        {
            new Candidate
            {
                name = "Bran Stark",
                image_URL= "https://upload.wikimedia.org/wikipedia/en/f/fa/Bran_Stark_-_Isaac_Hempstead-Wright.jpeg"
            },
            new Candidate
            {
                name = "Ayra Stark",
                image_URL = "https://upload.wikimedia.org/wikipedia/en/3/39/Arya_Stark-Maisie_Williams.jpg"
            }
        });
        */
        }
        
        public string Post_Result(int id, string Winner_id, string Loser_id)
        {
            try
            {
                using (SQLite.SQLiteConnection db = new SQLite.SQLiteConnection(database_name))
                {
                    //Udpate 
                    //do some maths
                    var Winner_guid = Guid.Parse(Winner_id);
                    var Loser_guid = Guid.Parse(Loser_id);


                    var Winner = db.Table<Candidate>().Where(x => x.ID == Winner_guid).FirstOrDefault();
                    var Loser = db.Table<Candidate>().Where(x => x.ID == Loser_guid).FirstOrDefault();

                    int Win_Elo = Winner.elo; // Going to write function
                    int Loser_Elo = Loser.elo; // Going to write function

                    var r1 = Math.Pow(10, Win_Elo / 400);
                    var r2 = Math.Pow(10, Loser_Elo / 400);

                    var e1 = r1 / (r1 + r2);
                    var e2 = r2 / (r2 + r1);
                    
                    Winner.elo = (int)(Win_Elo + K_Factor * (1 - e1));
                    Loser.elo = (int)(Loser_Elo + K_Factor * (0 - e2));

                    //Task.Factory.StartNew(() =>
                    //{
                        db.Update(Winner);
                        db.Update(Loser);
                    //});
                    /*
                    var check_win = db.Table<Candidate>().Where(x => x.ID == Winner_guid).FirstOrDefault();
                    if(check_win.elo != Winner.elo)
                    {
                        throw new Exception("FUFUUFUFUFCKKK");
                    }
                    */

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return "done";
            //Update_Elos();
        }


    }
}
