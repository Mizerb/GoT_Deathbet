using GoT_Deathbet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace GoT_Deathbet.Controllers
{
    public class VoteController : ApiController
    {
        int K_Factor = 32;
        const string database_name = @"C:\CODE\git\GoT_Deathbet\GoT_Deathbet\App_Data\db.sqlite";
        private SQLite.SQLiteAsyncConnection db = new SQLite.SQLiteAsyncConnection(database_name);
        private Random dude = new Random();
        //private SortedList<int, Candidate> Rankings = (new SQLite.SQLiteAsyncConnection(database_name)).Table<Candidate>().ToListAsync()

        [HttpGet]
        public JsonResult<Candidate[]> Get(string input)
        {
            var Ret = new Candidate[2];
            var a = db.Table<Candidate>().ToListAsync().Result;

            int left = dude.Next(a.Count);
            int right;
            do
            {
                right = dude.Next(a.Count);
            } while (right == left);
            Ret[0] = a[dude.Next(left)];

            Ret[1] = a[dude.Next(right)];

            return Json(Ret);
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
        [HttpPost]
        public void Post(Candidate Winner, Candidate Loser)
        {
            //Udpate 
            //do some maths
            int Win_Elo = Winner.elo; // Going to write function
            int Loser_Elo = Loser.elo; // Going to write function

            var r1 = Math.Pow(10, Win_Elo / 400);
            var r2 = Math.Pow(10, Loser_Elo / 400);

            var e1 = r1 / (r1 + r2);
            var e2 = r2 / (r2 + r1);

            Winner.elo = Win_Elo + K_Factor * (int)(1 - e1);
            Loser.elo = Loser_Elo + K_Factor * (int)(0 - e1);

            db.UpdateAsync(Winner);
            db.UpdateAsync(Loser);



            //Update_Elos();
        }


    }
}
