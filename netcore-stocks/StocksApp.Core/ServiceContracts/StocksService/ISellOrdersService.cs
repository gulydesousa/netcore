using ServiceContracts.DTO;

namespace ServiceContracts.StocksService
{
    public interface ISellOrdersService
    {

        //CreateSellOrder: Inserts a new sell order into the database table called 'SellOrders'.
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

        //GetSellOrders: Returns the existing list of sell orders retrieved from database table called 'SellOrders'.
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
