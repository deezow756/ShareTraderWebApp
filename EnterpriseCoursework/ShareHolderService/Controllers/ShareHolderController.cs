using Microsoft.AspNetCore.Mvc;
using ShareHolderService.Database;
using ShareHolderService.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareHolderService.Controllers
{
    [Route("api/shareholder")]
    [ApiController]
    public class ShareHolderController : ControllerBase
    {
        private ShareHolderContext db;
        public ShareHolderController(ShareHolderContext db)
        {
            this.db = db;
        }

        // GET: api/<ShareHolderController>
        [HttpGet]
        public ActionResult<IEnumerable<ShareHolder>> Get()
        {
            return Ok(db.ShareHolders.ToList());
        }

        // GET api/<ShareHolderController>/5
        [HttpGet("{id}")]
        public ActionResult<ShareHolder> Get(int id)
        {
            ShareHolder shareHolder = db.ShareHolders.Find(id);

            if (shareHolder != null)
            {
                return Ok(shareHolder);
            }

            return NotFound();
        }

        // POST api/<ShareHolderController>
        [HttpPost(Name = "Get")]
        public ActionResult<ShareHolder> Post([FromBody] ShareHolder shareHolder)
        {
            db.ShareHolders.Add(shareHolder);
            db.SaveChanges();

            return CreatedAtRoute(nameof(Get), new { Id = shareHolder.Id }, shareHolder);
        }

        // PUT api/<ShareHolderController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ShareHolder shareHolder)
        {
            ShareHolder check = db.ShareHolders.FirstOrDefault(a => a.Id == id);
            if (check == null)
            {
                return NotFound(id);
            }
            db.ShareHolders.Update(shareHolder);
            db.SaveChanges();

            return NoContent();
        }

        // DELETE api/<ShareHolderController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            ShareHolder check = db.ShareHolders.FirstOrDefault(a => a.Id == id);
            if (check == null)
            {
                return NotFound(id);
            }

            db.ShareHolders.Remove(check);
            db.SaveChanges();

            return NoContent();
        }
    }
}
