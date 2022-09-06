using BrokerService.Database;
using BrokerService.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrokerService.Controllers
{
    [Route("api/broker")]
    [ApiController]
    public class BrokerController : ControllerBase
    {
        private BrokerContext db;
        public BrokerController(BrokerContext db)
        {
            this.db = db;
        }

        // GET: api/<BrokerController>
        [HttpGet]
        public ActionResult<IEnumerable<Broker>> Get()
        {
            return Ok(db.Brokers.ToList());
        }

        // GET api/<BrokerController>/5
        [HttpGet("{id}")]
        public ActionResult<Broker> Get(int id)
        {
            Broker broker = db.Brokers.Find(id);

            if (broker != null)
            {
                return Ok(broker);
            }

            return NotFound();
        }

        // POST api/<BrokerController>
        [HttpPost(Name = "Get")]
        public ActionResult<Broker> Post([FromBody] Broker broker)
        {
            db.Brokers.Add(broker);
            db.SaveChanges();

            return CreatedAtRoute(nameof(Get), new { Id = broker.Id }, broker);
        }

        // PUT api/<BrokerController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Broker broker)
        {
            Broker check = db.Brokers.FirstOrDefault(a => a.Id == id);
            if (check == null)
            {
                return NotFound(id);
            }
            db.Brokers.Update(broker);
            db.SaveChanges();

            return NoContent();
        }

        // DELETE api/<BrokerController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Broker check = db.Brokers.FirstOrDefault(a => a.Id == id);
            if (check == null)
            {
                return NotFound(id);
            }

            db.Brokers.Remove(check);
            db.SaveChanges();

            return NoContent();
        }
    }
}
