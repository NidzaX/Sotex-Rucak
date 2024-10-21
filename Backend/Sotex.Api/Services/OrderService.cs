using AutoMapper;
using Sotex.Api.Dto.OrderDto;
using Sotex.Api.Infrastructure;
using Sotex.Api.Interfaces;
using Sotex.Api.Repo;

namespace Sotex.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly ProjectDbContext _projectDbContext;
        private readonly UserRepo _userRepo;
        private readonly OrdersRepo _orderRepo;
        private readonly MenuRepo _menuRepo;
        private readonly OrderedMenuItemsRepo _orderedMenuItemsRepo;

        public OrderService(IMapper mapper, ProjectDbContext dbContext)
        {
            _mapper = mapper;
            _userRepo = new UserRepo(dbContext);
            _orderRepo = new OrdersRepo(dbContext);
            _menuRepo = new MenuRepo(dbContext);
            _orderedMenuItemsRepo = new OrderedMenuItemsRepo(dbContext);
        }
        public Task<int> AddOrderAsync(NewOrderDto orderDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
