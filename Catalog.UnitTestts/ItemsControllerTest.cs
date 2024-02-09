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