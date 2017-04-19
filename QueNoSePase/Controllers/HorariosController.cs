using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Http;
using Newtonsoft.Json;
using QueNoSePase.API.Models;
using QueNoSePase.API.WebDataClient;
using System.Net;
using QueNoSePase.API.swandroidcuandollegasmp;
using System.Text.RegularExpressions;
using System.Linq;
using log4net;

namespace QueNoSePase.API.Controllers
{
    public class HorariosController : ApiController
    {
        //0| 22,MARQUES A KENNEDY12:53//22,MARQUES A KENNEDY13:08//22,MARQUES A KENNEDY13:23//22,MARQUES A KENNEDY13:38//22,MARQUES A KENNEDY13:53//
        /*
        [Route("api/horarios/{parada}")]
        public string Get(string parada)
        {
            try
            {
                ServicioWebHorariosProximos a = new ServicioWebHorariosProximos();
                var aux = a.RecuperarHorarioCuandoLlegaDosAndroid(parada, Constants.USER_ID, "TODOS", Constants.USUARIO,
                    Constants.CLAVE, "-1");

                var list = new List<Horario>();

                if (aux.StartsWith("0|"))
                {
                    if(aux.Contains("Sin datos"))
                    {
                        return JsonConvert.SerializeObject(new Exception("No se encontraron horarios para esta parada"));
                    }
                    var data = aux.Split('|')[1];
                    foreach (string s in data.Split(new[] { "//" }, StringSplitOptions.None))
                    {
                        if (string.IsNullOrEmpty(s)) continue;

                        var horario = new Horario();

                        var spl = s.Split(',');
                        horario.Linea = spl[0];
                        string ax = spl[1];
                        if (ax.Contains(":"))
                        {
                            int index = ax.IndexOf(":");
                            horario.Hora = ax.Substring((index - 2));
                            horario.Direccion = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ax.Replace(horario.Hora, "").ToLower());
                        }
                        else
                        {
                            horario.Direccion = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(spl[1].ToLower());
                            if (spl[2] == "0")
                            {
                                horario.Llegando = true;
                                horario.Minutos = 0;
                            }
                            else
                                horario.Minutos = Int32.Parse(spl[2]);
                        }
                        list.Add(horario);
                    }
                }

                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex);
            }
        }
        */

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static TresPropiedades[] _lineasCache;
        swandroidcuandollegasmp.Paradas service;

        public HorariosController()
        {
            service = new swandroidcuandollegasmp.Paradas();
            service.UserDetailsValue = new swandroidcuandollegasmp.UserDetails();
            service.UserDetailsValue.password = "PsAnCL3280.";
            service.UserDetailsValue.userName = "UsAnCL3280.";

            _lineasCache = service.RecuperarLineaPorLocalidad("CÓRDOBA", "CÓRDOBA", "ARGENTINA");

            Log.Debug("HorariosController initialized");
        }

        [Route("api/horarios/{parada}/{linea}")]
        public string Get(string parada, string linea)
        {
            try
            {
                Log.InfoFormat("Parada: {0} , Linea: {1}", parada, linea);

                if (service == null)
                {
                    service = new swandroidcuandollegasmp.Paradas();
                    service.UserDetailsValue = new swandroidcuandollegasmp.UserDetails();
                    service.UserDetailsValue.password = "PsAnCL3280.";
                    service.UserDetailsValue.userName = "UsAnCL3280.";
                }

                if(_lineasCache == null)
                {
                    _lineasCache = service.RecuperarLineaPorLocalidad("CÓRDOBA", "CÓRDOBA", "ARGENTINA");
                }
                

                //1278  linea 10 cba

                ///working
                //var calles = p.RecuperarCallesPrincipalPorLinea(1278);
                //var inters = p.RecuperarInterseccionPorLineaYCalle(1278, int.Parse(calles[0].codigo));
                //var locs = p.RecuperarLocalidadesPorEntidad(15);
                //var lineas = p.RecuperarLineaPorLocalidad("CÓRDOBA", "CÓRDOBA", "ARGENTINA");
                //
                //var paradas2 = p.RecuperarParadasPorCalleEInterseccion(23733, 23734);
                //var paradas3 = p.RecuperarParadasPorLineaCalleEInterseccion(1278, 23733, 23734);
                //var horarios = p.RecuperarProximosArribosSMP("C0094", "1278", 0, "CÓRDOBA");
                ////con más datos   (??)
                //var horariosCL = p.RecuperarProximosArribosCLCompleto("C0094", 1278, 0, 15, "CÓRDOBA");

                //buscar entre las lineas la {linea}
                TresPropiedades _linea = null;
                for (int i = 0; i < _lineasCache.Length; i++)
                {
                    if(_lineasCache[i].descripcionB == linea.Trim())
                    {
                        _linea = _lineasCache[i];
                        break;                    }
                }
                if (_linea == null)
                {
                    Log.ErrorFormat("LINEA NOT FOUND. Parada: {0} , Linea: {1}", parada, linea);
                    return JsonConvert.SerializeObject(new List<Horario>());
                }

                //obtener  codigo
                //var codigo = int.Parse(_linea.descripcionA);
                var codigo = _linea.descripcionA;

                var horarios = service.RecuperarProximosArribosSMP(parada, codigo, 0, "CÓRDOBA");
                var list = new List<Horario>();

                foreach (ProximoArriboSMP item in horarios)
                {
                    var h = new Horario
                    {
                        Direccion = item.bandera,
                        Llegando = item.arribo.Contains("Arribando"),
                        Linea = item.linea,
                        Hora = ""
                    };
                    var regex = new Regex(@"\d+");
                    var matches = regex.Matches(item.arribo);
                    if(matches.Count > 0)
                    {
                        h.Minutos = int.Parse(matches[0].Value);
                    }
                    list.Add(h);
                }
                
                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                Log.Error("Horarios 'GET' error", ex);
                return JsonConvert.SerializeObject(ex);
            }
}
    }
}
