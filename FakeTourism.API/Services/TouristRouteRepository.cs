using FakeTourism.API.Database;
using FakeTourism.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeTourism.API.Services
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRouteRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<TouristRoute> GetTouristRouteByIdAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefaultAsync(n => n.Id == touristRouteId);
        }

        public async Task<TouristRoute> GetTouristRouteByTitleAsync(string touristRouteTitle)
        {
            return await _context.TouristRoutes.FirstOrDefaultAsync(n => n.Title.Equals(touristRouteTitle));
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue)
        {
            // include vs join
            IQueryable<TouristRoute> result = _context
                .TouristRoutes
                .Include(t => t.TouristRoutePictures);
            if (!string.IsNullOrWhiteSpace(keyword)) 
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword));
            }
            if (ratingValue >= 0) 
            {
                result = ratingOperator switch
                {
                    "largerThan" => result.Where(t => t.Rating >= ratingValue),
                    "lessThan" => result.Where(t => t.Rating <= ratingValue),
                    _ => result.Where(t => t.Rating == ratingValue),
                };
            }
            return await result.ToListAsync();
        }

        public async Task<bool> TouristRouteExistsAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.AnyAsync(t => t.Id == touristRouteId);
        }
        public async Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutePictures
                .Where(p => p.TouristRouteId == touristRouteId).ToListAsync();
        }
        public async Task<TouristRoutePicture> GetPictureAsync(int pictureId)
        {
            return await _context.TouristRoutePictures.Where(picture => picture.Id == pictureId).FirstOrDefaultAsync();
        }

        public void AddTouristRoute(TouristRoute touristRoute) 
        {
            if (touristRoute == null) 
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }
            _context.TouristRoutes.Add(touristRoute);
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            if (touristRouteId == Guid.Empty) 
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if (touristRoutePicture == null) 
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }

            touristRoutePicture.TouristRouteId = touristRouteId;
            _context.TouristRoutePictures.Add(touristRoutePicture);

        }

        public void DeleteTouristRoute(TouristRoute touristRoute) 
        {
            _context.TouristRoutes.Remove(touristRoute);
        }

        public void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes) 
        {
            _context.TouristRoutes.RemoveRange(touristRoutes);
        }

        public void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture) 
        {
            _context.TouristRoutePictures.Remove(touristRoutePicture);
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDListAsync(IEnumerable<Guid> ids) 
        {
            return await _context.TouristRoutes.Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        public async Task<ShoppingCart> GetShoppingCartByUserId(string userId) 
        {
            return await _context.ShoppingCarts
                .Include(shoppingCart => shoppingCart.User)
                .Include(shoppingCart => shoppingCart.ShoppingCartItems).ThenInclude(lineItem => lineItem.TouristRoute)
                .Where(shoppingCart => shoppingCart.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task CreateShoppingCart(ShoppingCart shoppingCart) 
        {
            await _context.ShoppingCarts.AddAsync(shoppingCart);
        }

        public async Task AddShoppingCartItem(LineItem lineItem) 
        {
            await _context.LineItems.AddAsync(lineItem);
        }

        public async Task<LineItem> GetShoppingCartItemByItemId(int lineItemId) 
        {
            return await _context.LineItems
                .Where(lineItem => lineItem.Id == lineItemId)
                .FirstOrDefaultAsync();
        }

        public void DeleteShoppingCartItem(LineItem lineItem) 
        {
            _context.LineItems.Remove(lineItem);
        }

        public async Task<IEnumerable<LineItem>> GetShoppingCartItemsByIdListAsync(IEnumerable<int> itemIDs) 
        {
            return await _context.LineItems.Where(lineItem => itemIDs.Contains(lineItem.Id)).ToListAsync();
        }
        public void DeleteSHoppingCartItems(IEnumerable<LineItem> lineItems) 
        {
            _context.LineItems.RemoveRange(lineItems);
        }

        public async Task AddOrderAsync(Order order) 
        {
            await _context.Orders.AddAsync(order);

        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(string userId) 
        {
           return await _context.Orders.Where(orders => orders.UserId == userId).ToListAsync();
        }

        public async Task<Order> GetOrderById(Guid orderId) 
        {
            return await _context.Orders
                .Include(order => order.OrderItems).ThenInclude(orderItem => orderItem.TouristRoute)
                .Where(order => order.Id == orderId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAsync() 
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
