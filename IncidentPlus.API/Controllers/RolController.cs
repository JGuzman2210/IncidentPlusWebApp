using IncidentPlus.Data.RolRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IncidentPlus.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api")]
    public class RolController : ApiController
    {
        private RolRepository _rolRepository;
        public RolController()
        {
            _rolRepository = RolRepository.NewInstance();
        }
        [Route("Roles")]
        [HttpGet]
        public IHttpActionResult GetAllRoles()
        {
            return Ok(_rolRepository.GetAll());
        }
    }
}