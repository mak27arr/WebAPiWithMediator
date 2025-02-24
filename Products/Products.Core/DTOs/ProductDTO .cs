using System.ComponentModel.DataAnnotations;

namespace Products.Core.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public double? CurrentPrice { get; set; }

        public int? CurrencyId { get; set; }

        public CurrencyDTO? Currency { get; set; }
    }
}
