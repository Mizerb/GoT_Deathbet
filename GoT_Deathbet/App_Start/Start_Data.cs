using GoT_Deathbet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using SQLite;


namespace GoT_Deathbet
{
    public static class Start_Data
    {
        const string database_name = @"C:\CODE\git\GoT_Deathbet\GoT_Deathbet\App_Data\db.sqlite";
        const string starter_xml = @"C:\CODE\git\GoT_Deathbet\GoT_Deathbet\bin\App_Data\data.xml";

        private static void create_db()
        {
            try
            {
                System.Data.SQLite.SQLiteConnection.CreateFile(database_name);
                var conn = new SQLiteConnection(database_name);
                conn.CreateTable<Candidate>();
            }catch( Exception e)
            {
                Console.WriteLine(e);
                //Log it, when I get there
            }
        }

        public static async void Prepare_Data()
        {
            try
            {
                if (!System.IO.File.Exists(database_name))
                {
                    create_db();
                }
                var db = new SQLiteAsyncConnection(database_name);
                await db.CreateTableAsync<Candidate>();

                XDocument data = XDocument.Load(starter_xml);
                var chars = data.Descendants().Where(x => x.Name == "Character");

                foreach (var person in chars)
                {
                    var cand = new Candidate
                    {
                        name = person.Element("Name").Value,
                        elo = int.Parse(person.Element("Elo").Value),
                        image_URL = person.Element("Image").Value,
                        ID = Guid.NewGuid()
                    };
                    db.InsertAsync(cand);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}