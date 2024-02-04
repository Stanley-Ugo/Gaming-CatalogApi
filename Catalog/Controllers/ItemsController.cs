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
    }
}