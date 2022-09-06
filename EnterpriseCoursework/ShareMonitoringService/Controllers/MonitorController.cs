using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareMonitoringService.Database;
using ShareMonitoringService.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareMonitoringService.Controllers
{
    [Route("api/monitor")]
    [ApiController]
    public class MonitorController : ControllerBase
    {
        private MonitorContext db;
        public MonitorController(MonitorContext db)
        {
            this.db = db;
        }

        // GET: api/<MonitorController>
        [HttpGet]
        public ActionResult<IEnumerable<Monitor>> Get()
        {
            return Ok(db.Monitors.AsNoTracking().ToList());
        }

        // GET api/<MonitorController>/5
        [HttpGet("{id}")]
        public ActionResult<Monitor> Get(int id)
        {
            Monitor monitor = db.Monitors.Where(a => a.Id == id).First();

            if (monitor != null)
            {
                return Ok(monitor);
            }

            return NotFound();
        }

        // POST api/<MonitorController>
        [HttpPost(Name = "Get")]
        public ActionResult<Monitor> Post([FromBody] Monitor monitor)
        {
            db.Monitors.Add(monitor);
            db.SaveChanges();
            return CreatedAtRoute(nameof(Get), new { Id = monitor.Id }, monitor);
        }

        // PUT api/<MonitorController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Monitor monitor)
        {
            Monitor check = db.Monitors.FirstOrDefault(a => a.Id == id);
            if (check == null)
            {
                return NotFound(id);
            }
            db.Monitors.Update(monitor);
            db.SaveChanges();

            return NoContent();
        }

        // DELETE api/<MonitorController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Monitor check = db.Monitors.FirstOrDefault(a => a.Id == id);
            if (check == null)
            {
                return NotFound(id);
            }

            db.Monitors.Remove(check);
            db.SaveChanges();

            return NoContent();
        }
    }
}
