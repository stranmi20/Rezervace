using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Rezervace
{
    // TŘÍDA PRO VYTVOŘENÍ NEBO LOADNUTÍ DATABASE
    internal class dbload
    {
        public SQLiteConnection DBLoad()
        {
            // CESTA K DB
            string path = System.IO.Path.GetFullPath("database.db");

            // POKUD EXISTUJE
            if (System.IO.File.Exists("database.db"))
            {
                // ZÍSKÁNÍ DB
                var db = new SQLiteConnection(path);

                // VRACENENÍ DB
                return db;
            }
            // POKUD NEEXISTUJE
            else
            {
                // VYTVOŘENÍ DB
                var db = new SQLiteConnection("database.db");

                // VYTVOŘENÍ TABULEK
                db.CreateTable<Reservation>();
                db.CreateTable<Seat>();

                // VRACENENÍ DB
                return db;
            }
        }
    }
}
