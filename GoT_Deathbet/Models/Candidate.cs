using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite;

namespace GoT_Deathbet.Models
{
    public class Candidate
    {
        public string name { get; set; }
        [PrimaryKey]
        public Guid ID { get; set; }
        public string image_URL { get; set; }
        public int elo { get; set; }
    }
}