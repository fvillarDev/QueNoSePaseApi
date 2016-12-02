using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueNoSePase.API.Models
{
    public class Horario
    {
        public string Direccion { get; set; }
        public string Hora { get; set; }
        public int Minutos { get; set; }

        public DateTime HoraSort
        {
            get
            {
                var hora = Hora;
                if (string.IsNullOrEmpty(Hora))
                    hora = "00";
                return DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy " + hora + ":00"));
            }
        }

        public string Linea { get; set; }

        public bool Llegando { get; set; }
    }
}