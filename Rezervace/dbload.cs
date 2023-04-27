using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Rezervace
{
    internal class dbload
    {
        public SQLiteConnection DBLoad()
        {
            string path = System.IO.Path.GetFullPath("database.db");

            if (System.IO.File.Exists("database.db"))
            {
                var db = new SQLiteConnection(path);
                return db;
            }
            else
            {
                var db = new SQLiteConnection("database.db");

                db.CreateTable<Reservation>();
                db.CreateTable<Seat>();

                return db;
            }
        }
    }
}
