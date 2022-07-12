using CoffeeShop.Models;
using System.Collections.Generic;

namespace CoffeeShop.Repositories
{
    public interface ICoffeeRepository
    {
        public List<Coffee> GetAll();

        public Coffee Get(int id);

        public void Add(Coffee coffee);

        public void Update(Coffee coffee);

        public void Delete(int id);
    }
}
