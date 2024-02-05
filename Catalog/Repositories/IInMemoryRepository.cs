using System;
using System.Collections.Generic;
using Catalog.Entities;

namespace Catalog.Repositories
{
    public interface IInMemoryRepository
    {
        IEnumerable<Item> GetItems();
        Item GetItem(Guid id);
        void CreateItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(Guid id);
    }
}