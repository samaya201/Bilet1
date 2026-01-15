using System.ComponentModel.DataAnnotations;

namespace Bilet1.ViewModels.ServiceViewModels;

public class ServiceUpdateVM
{
    [Required]
    public int Id { get; set; }
    [Required,MinLength(3)]
    public string Title { get; set; } = string.Empty;
    [Required, MinLength(3)]
    public string Description { get; set; } = string.Empty;
    [Required]
    public int CategoryId { get; set; } 
    public IFormFile? Image{ get; set; }

}
