using Bilet1.Models.Common;

namespace Bilet1.Models;

public class Service:BaseEntity
{
    public string Title { get; set; }=string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public Category category { get; set; } = null!;

}

