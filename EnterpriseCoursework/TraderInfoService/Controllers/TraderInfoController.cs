using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraderInfoService.Database;
using TraderInfoService.Database.Entities;

namespace TraderInfoService.Controllers
{
    [Route("api/traderinfo")]
    [ApiController]
    public class TraderInfoController : ControllerBase
    {
        private TraderInfoContext db;
        public TraderInfoController(TraderInfoContext db)
        {
            this.db = db;
        }

        // GET: api/<TraderInfoController>
        [HttpGet]
        public ActionResult<IEnumerable<TraderInfo>> Get()
        {
            return Ok(db.TraderInfos.ToList());
        }

        // GET api/<TraderInfoController>/5
        [HttpGet("{id}")]
        public ActionResult<TraderInfo> Get(int id)
        {
            TraderInfo traderInfo = db.TraderInfos.Find(id);

            if (traderInfo != null)
            {
                return Ok(traderInfo);
            }

            return NotFound();
        }

        // POST api/<TraderInfoController>
        [HttpPost(Name = "Get")]
        public ActionResult<TraderInfo> Post([FromBody] TraderInfo traderInfo)
        {
            db.TraderInfos.Add(traderInfo);
            db.SaveChanges();

            return CreatedAtRoute(nameof(Get), new { Id = traderInfo.Id }, traderInfo);
        }

        // PUT api/<TraderInfoController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] TraderInfo traderInfo)
        {
            TraderInfo check = db.TraderInfos.FirstOrDefault(a => a.Id == id);
            if (check == null)
            {
                return NotFound(id);
            }
            db.TraderInfos.Update(traderInfo);
            db.SaveChanges();

            return NoContent();
        }

        // DELETE api/<TraderInfoController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            TraderInfo check = db.TraderInfos.FirstOrDefault(a => a.Id == id);
            if (check == null)
            {
                return NotFound(id);
            }

            db.TraderInfos.Remove(check);
            db.SaveChanges();

            return NoContent();
        }
    }
}
