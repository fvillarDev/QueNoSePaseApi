namespace QueNoSePase.API.Models
{
    public class Clasificacion
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
    }

    public class Linea : Clasificacion
    {
        
    }

    public class Calle : Clasificacion
    {

    }

    public class Interseccion : Clasificacion
    {

    }
}