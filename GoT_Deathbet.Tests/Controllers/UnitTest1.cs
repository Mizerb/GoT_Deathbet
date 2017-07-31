using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoT_Deathbet;
using Newtonsoft.Json;
using GoT_Deathbet.Controllers;
namespace GoT_Deathbet.Tests.Controllers
{
    [TestClass]
    public class VoteControllerTest
    {
        const string database_name = @"C:\CODE\git\GoT_Deathbet\GoT_Deathbet\App_Data\db.sqlite";

        [TestMethod]
        public void RankingUpdate()
        {
            var vote = new VoteController();

            var result = vote.Get_Duel().Content;

            vote.Post_Result(0, result[0].ID.ToString(), result[1].ID.ToString());

            using (SQLite.SQLiteConnection db = new SQLite.SQLiteConnection(database_name))
            {
                var check_id = result[0].ID;

                var check = db.Table<GoT_Deathbet.Models.Candidate>().Where(x => x.ID == check_id).ToArray();

                Assert.AreNotEqual(0, check.Length);

                if (check[0].elo == result[0].elo)
                {
                    Console.WriteLine("Elo did not change for winner");
                    Assert.Fail();
                }

            }

        }
    }
}
