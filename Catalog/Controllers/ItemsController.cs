using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Catalog.Entities;
using Catalog.Extensions;
using Catalog.ItemDTOS;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IInMemoryRepository _inMemoryRepository;
        public ItemsController(IInMemoryRepository inMemoryRepository)
        {
            _inMemoryRepository = inMemoryRepository;
        }

        [HttpGet]
        public IEnumerable<ItemDTO> GetItems()
        {
            var items = _inMemoryRepository.GetItems().Select(item => item.AsDTO());
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDTO> GetItem(Guid id)
        {
            var item = _inMemoryRepository.GetItem(id);
            if(item is null)
               return NotFound();
            return Ok(item.AsDTO());
        }

        [HttpPost]
        public ActionResult<ItemDTO> CreateItem(CreateItemDTO createItemDTO)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = createItemDTO.Name,
                Price = createItemDTO.Price,
                DateCreated = DateTimeOffset.UtcNow,
            };

            _inMemoryRepository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem), new {id = item.Id}, item.AsDTO());
        }

        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDTO updateItemDTO)
        {
            var existingItem = _inMemoryRepository.GetItem(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            Item updatedItem = existingItem with
            {
                Name = updateItemDTO.Name,
                Price = updateItemDTO.Price
            };

            _inMemoryRepository.UpdateItem(updatedItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = _inMemoryRepository.GetItem(id);
            if (existingItem is null)
            {
                return NotFound();
            }

            _inMemoryRepository.DeleteItem(id);

            return NoContent();
        }
    }
}