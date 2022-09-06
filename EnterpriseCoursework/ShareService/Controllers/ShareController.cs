using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareService.Database;
using ShareService.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShareController : ControllerBase
    {
        private ShareContext db;
        public ShareController(ShareContext db)
        {
            this.db = db;
        }

        // GET: api/<ShareController>
        [HttpGet]
        public ActionResult<IEnumerable<Share>> Get()
        {
            return Ok(db.Shares.AsNoTracking().ToList());
        }

        // GET api/<ShareController>/5
        [HttpGet("{id}")]
        public ActionResult<Share> Get(int id)
        {
            Share share = db.Shares.Find(id);

            if (share != null)
            {
                return Ok(share);
            }

            return NotFound();
        }

        // POST api/<ShareController>
        [HttpPost(Name = "Get")]
        public ActionResult<Share> Post([FromBody] Share share)
        {
            db.Shares.Add(share);
            db.SaveChanges();
            return CreatedAtRoute(nameof(Get), new { Id = share.Id }, share);
        }

        // PUT api/<ShareController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Share share)
        {
            Share check = db.Shares.FirstOrDefault(a => a.Id == id);
            if (check == null)
            {
                return NotFound(id);
            }
            db.Shares.Update(share);
            db.SaveChanges();

            return NoContent();
        }

        // DELETE api/<ShareController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Share check = db.Shares.FirstOrDefault(a => a.Id == id);
            if (check == null)
            {
                return NotFound(id);
            }

            db.Shares.Remove(check);
            db.SaveChanges();

            return NoContent();
        }
    }
}
