using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rezervace
{
    public class Seat
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Uuid { get; set; }
        public int SeatRow { get; set; }
        public int SeatColumn { get; set; }
        public string Stav { get; set; }
    }
}
