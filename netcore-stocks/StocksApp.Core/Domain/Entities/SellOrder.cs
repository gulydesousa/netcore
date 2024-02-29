using System.ComponentModel.DataAnnotations;

namespace StockMarketSolution.Entities
{
    public class SellOrder
    {
        [Key]
        public Guid SellOrderID { get; set; }
        public string StockSymbol { get; set; } = "";
        [Required(ErrorMessage = "Stock Name can't be null or empty")]
        public string StockName { get; set; } = "";
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000, ErrorMessage = "The maximum price of stock is 100000. Minimum is 1")]
        public double Price { get; set; } = 0;
        [Range(1, 100000, ErrorMessage = "You can buy maximum of 100000 shares in single order. Minimum is 1.")]
        public uint Quantity { get; set; } = 0;
    }
}
