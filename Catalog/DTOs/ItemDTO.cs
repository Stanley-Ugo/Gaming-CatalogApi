using System;

namespace Catalog.ItemDTOS
{
    public record ItemDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset DateCreated { get; init; }
    }
}