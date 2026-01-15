using System.ComponentModel.DataAnnotations;

namespace Bilet1.ViewModels.ServiceViewModels;

public class ServiceCreateVM
{

    [Required, MinLength(3)]
    public string Title { get; set; } = string.Empty;
    [Required, MinLength(3)]
    public string Description { get; set; } = string.Empty;
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public IFormFile Image { get; set; } = null!;
}
