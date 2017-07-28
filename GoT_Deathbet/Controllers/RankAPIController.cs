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
    public class RankAPIController : ApiController
    {
        const string database_name = @"C:\CODE\git\GoT_Deathbet\GoT_Deathbet\App_Data\db.sqlite";

        public JsonResult<Candidate[]> GetRanks()
        {
            using (SQLite.SQLiteConnection db = new SQLite.SQLiteConnection(database_name))
            {
                return Json(db.Table<Candidate>().OrderByDescending(x => x.elo).Take(20).ToArray());
            }
        }

    }
}
