using IncidentPlus.API.App_Start;
using IncidentPlus.Data.ProjectRepository;
using IncidentPlus.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IncidentPlus.API.Controllers
{
    [Authorize(Roles ="Admin")]
    [RoutePrefix("api")]
    public class ProjectController : ApiController
    {
        private ProjectRepository _projectRepo;

        public ProjectController()
        {
            this._projectRepo = ProjectRepository.NewInstance();
        }

        [Route("projects")]
        [HttpGet]
        public IHttpActionResult GetProjects()
        {
            var result = _projectRepo.GetAll();
            return Ok(result);
        }

        [Route("project/{id:int}")]
        [HttpGet]
        public IHttpActionResult GetProject(int id)
        {

            var result = _projectRepo.FindById(id);

            if (result == null)
                return NotFound();
               
            return Ok(result);
        }

        [Route("project/{id:int}/categories")]
        [HttpGet]
        public IHttpActionResult GetCategoriesByProjectId(int id)
        {
            var categories = _projectRepo.GetCategoriesByProjectID(id);
            if(categories == null)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [Route("project/{id:int}/levels")]
        [HttpGet]
        public IHttpActionResult GetLevelsByProjectId(int id)
        {
            var levels = _projectRepo.GetLevelsByProjectID(id);
            if (levels == null)
            {
                return NotFound();
            }

            return Ok(levels);
        }

        [Route("project")]
        [HttpPost]
        public IHttpActionResult AddProject([FromBody] Project project)
        {
           try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Missing data for creating the project");
                }
                var date = DateTime.Now;
                project.Created = date;
                project.Update = date;
                _projectRepo.Add(project);
                return Ok("Project was created successfully");
            }
            catch
            {
                return Json(new { error="Occurred an error while add the project" });
            }
        }

        [Route("project")]
        [HttpPut]
        public IHttpActionResult UpdateProject([FromBody] Project project)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Missing data for updating the project");
                }
                var tempProject = _projectRepo.FindById(project.Id);
                if(tempProject == null)
                {
                    return BadRequest("Project ID do not exit");
                }
                project.Created = tempProject.Created;
                project.Update = DateTime.Now;
                _projectRepo.Update(project);
                return Ok("Project was upadated successfully");
            }
            catch
            {
                return Json(new { error = "Occurred an error while add the project" });
            }
        }

        [Route("project/{id:int}")]
        [HttpDelete]
        public IHttpActionResult DeleteProject(int id)
        {
            try
            {
                var tempProject = _projectRepo.FindById(id);
                if (tempProject == null)
                {
                    return BadRequest("Project ID do not exit");
                }
                _projectRepo.Delete(id);
                return Ok("Project was processed successfully");
            }
            catch
            {
                return Json(new { error = "Occurred an error while delete the project" });
            }
        }

        [Route("project/{id:int}/enable")]
        [HttpGet]
        public IHttpActionResult EnableProject(int id)
        {
            try
            {
                var tempProject = _projectRepo.FindById(id);
                if (tempProject == null)
                {
                    return BadRequest("Project ID do not exit");
                }
                _projectRepo.Enable(id);
                return Ok("Project was enabled successfully");
            }
            catch
            {
                return Json(new { error = "Occurred an error while enable the project" });
            }

        }
    }
}