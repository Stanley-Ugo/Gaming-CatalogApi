using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Entities;
using Catalog.Extensions;
using Catalog.DTOs;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IInMemoryRepository _inMemoryRepository;
        private readonly ILogger<ItemsController> _logger;
        public ItemsController(IInMemoryRepository inMemoryRepository, ILogger<ItemsController> logger)
        {
            _inMemoryRepository = inMemoryRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDTO>> GetItemsAsync()
        {
            var items = (await _inMemoryRepository.GetItemsAsync()).Select(item => item.AsDTO());
            
            _logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items.Count()} items");
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetItemAsync(Guid id)
        {
            var item = await _inMemoryRepository.GetItemAsync(id);
            if(item is null)
               return NotFound();
            return Ok(item.AsDTO());
        }

        [HttpPost]
        public async Task<ActionResult<ItemDTO>> CreateItemAsync(CreateItemDTO createItemDTO)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = createItemDTO.Name,
                Price = createItemDTO.Price,
                DateCreated = DateTimeOffset.UtcNow,
            };

            await _inMemoryRepository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync), new {id = item.Id}, item.AsDTO());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDTO updateItemDTO)
        {
            var existingItem = await _inMemoryRepository.GetItemAsync(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            Item updatedItem = existingItem with
            {
                Name = updateItemDTO.Name,
                Price = updateItemDTO.Price
            };

            await _inMemoryRepository.UpdateItemAsync(updatedItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await _inMemoryRepository.GetItemAsync(id);
            if (existingItem is null)
            {
                return NotFound();
            }

            await _inMemoryRepository.DeleteItemAsync(id);

            return NoContent();
        }
    }
}