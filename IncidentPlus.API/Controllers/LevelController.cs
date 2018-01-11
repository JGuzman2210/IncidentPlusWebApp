using IncidentPlus.Data.LevelRepository;
using IncidentPlus.Entity.Entities;
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
    public class LevelController : ApiController
    {
        private LevelRepository _levelRepo;

        public LevelController()
        {
            _levelRepo = LevelRepository.NewInstance();
        }

        [Route("level")]
        [HttpPost]
        public IHttpActionResult AddLevel([FromBody] Level level)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Missing data for creating the Level");
                }

                if (level.IsDefault)
                {
                    var result = _levelRepo.ExitDefaultLevel(level.ProjectID);
                    if (result)
                        return BadRequest("Already there is a default level");
                }

                var date = DateTime.Now;
                level.Created = date;
                level.Update = date;
                _levelRepo.Add(level);
                return Ok("Level was created successfully");
            }
            catch
            {
                return Json(new { error = "Occurred an error while add the level" });
            }
        }
    }
}