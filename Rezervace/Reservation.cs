using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Rezervace
{
    // TŘÍDO PRO VYTVOŘENÍ TABULKY DO DATABÁZE "REGISTRACE"
    internal class Reservation
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Uuid { get; set; }
        public int SeatRow { get; set; }
        public int SeatColumn { get; set; }
    }
}
