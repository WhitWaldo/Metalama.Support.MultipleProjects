using Microsoft.Extensions.Logging;
using Models;

namespace Runner
{
    internal class Worker
    {
        private readonly Repository<Person> _repository;

        public Worker(Repository<Person> repository, ILogger<Worker> logger)
        {
            _repository = repository;
        }

        public void DoStuff(string name)
        {
            _repository.Add(new Person("Nick"));
            _repository.Add(new Person("Bill"));
            var people = _repository.List();
            _repository.Remove(people[0].Id);
        }
    }
}
