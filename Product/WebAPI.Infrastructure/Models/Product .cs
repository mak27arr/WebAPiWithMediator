using System.ComponentModel.DataAnnotations;

namespace WebAPI.Infrastructure.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public double? CurrentPrice { get; set; }

        public int? CurrencyId { get; set; }

        public Currency? Currency { get; set; }
    }
}
