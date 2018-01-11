using IncidentPlus.Data.CategoryRepository;
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
    public class CategoryController : ApiController
    {

        private CategoryRepository _catRepo;

        public CategoryController()
        {
            _catRepo = CategoryRepository.NewInstance();
        }

       [Route("category")]
       [HttpPost]
       public IHttpActionResult AddCategory([FromBody] Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Missing data for creating the Category");
                }
                var date = DateTime.Now;
                category.Created = date;
                category.Update = date;
                _catRepo.Add(category);
                return Ok("Category was created successfully");
            }
            catch
            {
                return Json(new { error = "Occurred an error while add the category" });
            }
        }
    }
}