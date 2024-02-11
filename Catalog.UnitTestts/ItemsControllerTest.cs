namespace Catalog.UnitTestts;

public class ItemsControllerTest
{
    private readonly Mock<IInMemoryRepository> respositoryStub = new Mock<IInMemoryRepository>();
    private readonly Mock<ILogger<ItemsController>> loggerStub = new Mock<ILogger<ItemsController>>();
    private readonly Random rand = new Random();

    [Fact]
    public async Task GetItemsAsync_WithUnExistingItem_ReturnsNotFound()
    {
        //Arrange
        respositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                   .ReturnsAsync((Item)null);

        var controller = new ItemsController(respositoryStub.Object, loggerStub.Object);

        //Act
        var result = await controller.GetItemAsync(Guid.NewGuid());

        //Assert
        //Assert.IsType<NotFoundResult>(result.Result);
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetItemsAsync_WithExistingItem_ReturnsExpectedItem()
    {
        //Arrange
        var expectedItem = CreateRandonItem();

        respositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(expectedItem);

        var controller = new ItemsController(respositoryStub.Object, loggerStub.Object); 

        //Act
        Assert.IsType<NotFoundResult>(result.Result);

        //Assert
        result.Value.Should().BeEquivalentTo(
            expectedItem,
            options => options.ComparingByMembers<Item>());
    }
    
    [Fact]
    public async Task GetItemsAsync_WithExistingItem_ReturnsAllItem()
    {
        //Arrange
        var expectedItem = new[]{ CreateRandonItem(), CreateRandonItem(), CreateRandonItem() };

        respositoryStub.Setup(repo => repo.GetItemsAsync())
                   .ReturnsAsync(expectedItem);

        var controller = new ItemsController(respositoryStub.Object, loggerStub.Object);

        //Act
        var actualItems = await controller.GetItemsAsync();

        //Assert
        actualItems.Should().BeEquivalentTo(
            existingItem,
            options => options.ComparingByMembers<Item>());
    }

    [Fact]
    public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
    {
        //Arrange
        var itemToCreate = new CreateItemDTO()
        {
            Name = Guid.NewGuid().ToString(),
            Price = rand.Next(1000)
        }

        var controller = new ItemsController(respositoryStub.Object, loggerStub.Object);

        //Act
        var result = await controller.CreatItemAsync(itemToCreate);

        //Assert
        var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDTO;
        itemToCreate.Should().BeEquivalentTo(
            createdItem,
            options => options.ComparingByMembers<ItemDTO>().ExcludingMissingMembers()
        );
        createdItem.Id.Should().NotBeEmpty();
        createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffSet.UtcNow, 1000);
    }

    [Fact]
    public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
    {
        //Arrange
        var existingItem = CreateRandonItem();

        respositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(expectedItem);

        var itemId = existingItem.Id;
        var itemToUpdate = new UpdateItemDTO()
        {
            Name = Guid.NewGuid().ToString(),
            Price = existingItem.Price + 3
        }

        var controller = new ItemsController(respositoryStub.Object, loggerStub.Object);

        //Act
        var result = await controller.UpdateItemAsync(itemId, itemToUpdate);

        //Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
    {
        //Arrange
        var existingItem = CreateRandonItem();

        respositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(expectedItem);

        var controller = new ItemsController(respositoryStub.Object, loggerStub.Object);

        //Act
        var result = await controller.DeleteItemAsync(existingItem.Id);

        //Assert
        result.Should().BeOfType<NoContentResult>();
    }


    private Item CreateRandonItem()
    {
        return new Item
        {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            Price = rand.Next(1000),
            CreatedDate = DateTimeOffset.UtcNow;
        }
    }
}