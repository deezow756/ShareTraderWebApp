using Microsoft.AspNetCore.Mvc;
using ShareAlertService.Database;
using ShareAlertService.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareAlertService.Controllers
{
    [Route("api/sharealert")]
    [ApiController]
    public class ShareAlertController : ControllerBase
    {
        private ShareAlertContext db;
        public ShareAlertController(ShareAlertContext db)
        {
            this.db = db;
        }

        // GET: api/<ShareAlertController>
        [HttpGet]
        public ActionResult<IEnumerable<ShareAlert>> Get()
        {
            return Ok(db.ShareAlerts.ToList());
        }

        // GET api/<ShareAlertController>/5
        [HttpGet("{id}")]
        public ActionResult<ShareAlert> Get(int id)
        {
            ShareAlert shareAlert = db.ShareAlerts.Find(id);

            if (shareAlert != null)
            {
                return Ok(shareAlert);
            }

            return NotFound();
        }

        // POST api/<ShareAlertController>
        [HttpPost(Name = "Get")]
        public ActionResult<ShareAlert> Post([FromBody] ShareAlert shareAlert)
        {
            db.ShareAlerts.Add(shareAlert);
            db.SaveChanges();

            return CreatedAtRoute(nameof(Get), new { Id = shareAlert.Id }, shareAlert);
        }

        // PUT api/<ShareAlertController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ShareAlert shareAlert)
        {
            ShareAlert check = db.ShareAlerts.FirstOrDefault(a => a.Id == id);
            if (check == null)
            {
                return NotFound(id);
            }
            db.ShareAlerts.Update(shareAlert);
            db.SaveChanges();

            return NoContent();
        }

        // DELETE api/<ShareAlertController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            ShareAlert check = db.ShareAlerts.FirstOrDefault(a => a.Id == id);
            if (check == null)
            {
                return NotFound(id);
            }

            db.ShareAlerts.Remove(check);
            db.SaveChanges();

            return NoContent();
        }
    }
}
