﻿using StockMarketSolution.Entities;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse : IOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        public string StockSymbol { get; set; } = "";
        public string StockName { get; set; } = "";
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; } = 0;
        public double Price { get; set; } = 0;
        public double TradeAmount { get; set; } = 0;

        public OrderType TypeOfOrder => OrderType.BuyOrder;


        /// <summary>
        /// Checks if the current object and other (parameter) object values match
        /// </summary>
        /// <param name="obj">Other object of BuyOrderResponse class, to compare</param>
        /// <returns>True or false determines whether current object and other objects match</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not BuyOrderResponse) return false;

            BuyOrderResponse other = (BuyOrderResponse)obj;
            return BuyOrderID == other.BuyOrderID 
                && StockSymbol == other.StockSymbol
                && StockName == other.StockName 
                && DateAndTimeOfOrder == other.DateAndTimeOfOrder 
                && Quantity == other.Quantity 
                && Price == other.Price;
        }

        /// <summary>
        /// Returns an int value that represents unique stock id of the current object
        /// </summary>
        /// <returns>unique int value</returns>
        public override int GetHashCode()
        {
            return StockSymbol.GetHashCode();
        }

        /// <summary>
        /// Converts the current object into string which includes the values of all properties
        /// </summary>
        /// <returns>A string with values of all properties of current object</returns>
        public override string ToString()
        {
            return $"Buy Order ID: {BuyOrderID}, " +
                $"Stock Symbol: {StockSymbol}, " +
                $"Stock Name: {StockName}, " +
                $"Date and Time of Buy Order: {DateAndTimeOfOrder.ToString("dd MMM yyyy hh:mm ss tt")}, " +
                $"Quantity: {Quantity}, " +
                $"Buy Price: {Price}, " +
                $"Trade Amount: {TradeAmount}";
        }

    }


    public static class BuyOrderExtensions
    {
        /// <summary>
        /// An extension method to convert an object of BuyOrder class into BuyOrderResponse class
        /// </summary>
        /// <param name="buyOrder">The BuyOrder object to convert</param>
        /// <returns>Returns the converted BuyOrderResponse object</returns>
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse()
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                Price = buyOrder.Price,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                TradeAmount = buyOrder.Price * buyOrder.Quantity
            };
        }
    }
}
