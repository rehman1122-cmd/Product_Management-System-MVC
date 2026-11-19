using System.ComponentModel.DataAnnotations;

namespace Lab9.Models;

public sealed class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    [Display(Name = "Watch name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(60, MinimumLength = 2)]
    public string Brand { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "1000000")]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    [Range(0, 1000000)]
    [Display(Name = "Units in stock")]
    public int Stock { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
}
