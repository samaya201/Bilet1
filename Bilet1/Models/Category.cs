using Bilet1.Models.Common;

namespace Bilet1.Models;

public class Category : BaseEntity 
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Service> Services { get; set; } = [];
}

