using FakeTourism.API.Helper;
using FakeTourism.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeTourism.API.Services
{
    public interface ITouristRouteRepository
    {
        Task<PaginationList<TouristRoute>> GetTouristRoutesAsync(
            string keyword, string ratingOperator, 
            int? ratingValue, int pageSize, 
            int pageNumber);
        Task<TouristRoute> GetTouristRouteByIdAsync(Guid touristRouteId);
        Task<TouristRoute> GetTouristRouteByTitleAsync(string touristRouteTitle);
        Task<bool> TouristRouteExistsAsync(Guid touristRouteId);
        Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId);
        Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDListAsync(IEnumerable<Guid> ids);
        Task<TouristRoutePicture> GetPictureAsync(int pictureId);
        void AddTouristRoute(TouristRoute touristRoute);
        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);
        void DeleteTouristRoute(TouristRoute touristRoute);
        void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture);
        void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes);
        Task<ShoppingCart> GetShoppingCartByUserId(string userId);
        Task CreateShoppingCart(ShoppingCart shoppingCart);
        Task AddShoppingCartItem(LineItem lineItem);
        Task<LineItem> GetShoppingCartItemByItemId(int lineItemId);
        void DeleteShoppingCartItem(LineItem lineItem);
        Task<IEnumerable<LineItem>> GetShoppingCartItemsByIdListAsync(IEnumerable<int> itemIDs);
        void DeleteSHoppingCartItems(IEnumerable<LineItem> lineItems);
        Task AddOrderAsync(Order order);
        Task<PaginationList<Order>> GetOrdersByUserId(string userId, int pageSize, int pageNumber);
        Task<Order> GetOrderById(Guid orderId);
        Task<bool> SaveAsync();
    }
}
