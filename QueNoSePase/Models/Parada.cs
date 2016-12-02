using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueNoSePase.API.Models
{
    public class Parada : Clasificacion
    {
        //0|43391;AL CENTRO;C4325;-31,385181;-64,201530;23/05/2016 03:05:23|
        public string NumeroParada { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Fecha { get; set; }
    }
}