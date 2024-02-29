using ServiceContracts.DTO;

namespace ServiceContracts.StocksService
{
    public interface IBuyOrdersService
    {
        //CreateBuyOrder: Inserts a new buy order into the database table called 'BuyOrders'.
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

        //GetBuyOrders: Returns the existing list of buy orders retrieved from database table called 'BuyOrders'.
        Task<List<BuyOrderResponse>> GetBuyOrders();
    }
}
