using Catalog.Entities;
using Catalog.DTOs;

namespace Catalog.Extensions
{
    public static class Extension
    {
        public static ItemDTO AsDTO(this Item item)
        {
            return new ItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                DateCreated = item.DateCreated
            };
        }
    }
}