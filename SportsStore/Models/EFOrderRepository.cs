using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EFOrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IQueryable<Order> Orders => _dbContext.Orders.Include(o => o.Lines).ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {
            _dbContext.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderId == 0)
                _dbContext.Orders.Add(order);

            _dbContext.SaveChanges();
        }
    }
}
