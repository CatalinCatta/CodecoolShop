namespace Codecool.CodecoolShop.Models;

public abstract class BaseModel
{
    public int Id { get; set; }
    public string Name { get; init; }
    public string Description { get; init; }
}