using Metalama.Bits;
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

        public void DoStuff([SensitiveData]string name)
        {
            _repository.Add(new Person("Nick"));
            _repository.Add(new Person("Bill"));
            var people = _repository.List();
            _repository.Remove(people[0].Id);
            _repository.Update(people[1].Id, new Person("Jill")
            {
                Id = people[1].Id
            });
            var jill = _repository.Get(people[1].Id);
        }
    }
}
