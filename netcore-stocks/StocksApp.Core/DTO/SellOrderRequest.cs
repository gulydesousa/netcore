﻿using StockMarketSolution.Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public  class SellOrderRequest : IValidatableObject, IOrderRequest
    {
        [Required(ErrorMessage = "Stock Symbol can't be null or empty")]
        public string StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name can't be null or empty")]
        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "You can buy maximum of 100000 shares in single order. Minimum is 1.")]
        public double Price { get; set; } = 0;

        [Range(1, 10000, ErrorMessage = "The maximum price of stock is 10000. Minimum is 1.")]
        public uint Quantity { get; set; } = 0;

        public SellOrder ToSellOrder()
        {
            return new SellOrder()
            {
                StockSymbol = this.StockSymbol,
                StockName = this.StockName,
                DateAndTimeOfOrder = this.DateAndTimeOfOrder,
                Price = this.Price,
                Quantity = this.Quantity
            };

        }


        /// <summary>
        /// Model class-level validation using IValidatableObject
        /// </summary>
        /// <param name="validationContext">ValidationContext to validate</param>
        /// <returns>Returns validation errors as ValidationResult</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            //Date of order should be less than Jan 01, 2000
            if (DateAndTimeOfOrder < Convert.ToDateTime("2000-01-01"))
            {
                results.Add(new ValidationResult("Date of the order should not be older than Jan 01, 2000."));
            }

            return results;
        }

    }
}
