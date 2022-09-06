using InventoryService.Database;
using InventoryService.Database.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryService.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private InventoryContext db;
        public InventoryController(InventoryContext db)
        {
            this.db = db;
        }

        // GET: api/<InventoryController>
        [HttpGet]
        public ActionResult<IEnumerable<Inventory>> Get()
        {
            return Ok(db.Inventories.ToList());
        }

        // GET api/<InventoryController>/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Inventory> Get(int id)
        {
            Inventory inventory = db.Inventories.FirstOrDefault(a => a.Id == id);

            if(inventory != null)
            {
                return Ok(inventory);
            }

            return NotFound();
        }

        // POST api/<InventoryController>
        [HttpPost]
        public ActionResult<Inventory> Post([FromBody] Inventory inventory)
        {
            db.Inventories.Add(inventory);
            db.SaveChanges();

            return CreatedAtRoute(nameof(Get), new { Id = inventory.Id }, inventory);
        }

        // PUT api/<InventoryController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Inventory inventory)
        {
            db.Inventories.Update(inventory);
            db.SaveChanges();

            return NoContent();
        }

        // DELETE api/<InventoryController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Inventory inventory = db.Inventories.FirstOrDefault(a => a.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            db.Inventories.Remove(inventory);
            db.SaveChanges();

            return NoContent();
        }
    }
}
