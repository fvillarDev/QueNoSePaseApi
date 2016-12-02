using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueNoSePase.API.Models
{
    public class ParadaCercana
    {
        //0|C1040;AL CENTRO;RAFAEL NUÑEZ y JOSE REYNAFE;10;11;12;13;15;17;18;19|C0126;AL BARRIO;AV RAFAEL NUÑEZ y MANUEL PIZARRO;10;11;12;13;15;17;18;19|
        public Parada Parada { get; set; }
        public string Direccion { get; set; }
        public string Calles { get; set; }
        public List<string> Lineas { get; set; }
    }
}