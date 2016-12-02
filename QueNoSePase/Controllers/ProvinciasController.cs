﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using QueNoSePase.API.Models;
using QueNoSePase.API.WebDataClient;

namespace QueNoSePase.API.Controllers
{
    public class ProvinciasController : ApiController
    {
        // GET: api/Provincias/{idPais}
        public string Get(string id)
        {
            try
            {
                var servicios = new ServicioWebHorariosProximos();
                var data = servicios.RecuperarTodasProvinciasPorPaisAndroid(Int32.Parse(id), Constants.USUARIO, Constants.CLAVE);
                if (string.IsNullOrEmpty(data))
                {
                    return JsonConvert.SerializeObject(new Exception("Data retrieved is null."));
                }

                var spl = data.Split('|');
                if (spl.Length > 0 && spl[0] == "0")
                {
                    var paises = (from s in spl
                                  where !string.IsNullOrEmpty(s) && s.Contains(";")
                                  select new Provincia
                                  {
                                      Id = Int32.Parse(s.Split(';')[0]),
                                      Nombre = s.Split(';')[1]
                                  }).ToList();
                    return JsonConvert.SerializeObject(paises);
                }

                return JsonConvert.SerializeObject(new Exception("Error trying to parse data."));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex);
            }
        }
    }
}
