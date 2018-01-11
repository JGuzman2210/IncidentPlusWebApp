using IncidentPlus.Data.UserRepository;
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
    public class UserController : ApiController
    {
        private UserRepository _userRepo;

        public UserController()
        {
            this._userRepo = UserRepository.NewInstance();
        }

        [Route("Users")]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            var result = _userRepo.GetAll();
            return Ok(result);
        }
    }
}