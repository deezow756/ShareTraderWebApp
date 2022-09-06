using CommanderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommanderService.Data
{
    public class MockCommanderRepo : ICommanderRepo
    {
        IEnumerable<Command> commands = new List<Command>
            {
                new Command() { Id = 0, HowTo = "Boil an egg", Line = "Boil water", Platform = "Kettle & Pan" },
                new Command() { Id = 1, HowTo = "Cut bread", Line = "Get a knife", Platform = "Knife & Chopping Board" },
                new Command() { Id = 2, HowTo = "Make cup of tea", Line = "Place teabag in cup", Platform = "Kettle & Cup" }
            };

        public void CreateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public void DeleteCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands()
        {           

            return commands;
        }

        public Command GetCommandById(int id)
        {
            return commands.First<Command>(a => a.Id == id);
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }
    }
}
