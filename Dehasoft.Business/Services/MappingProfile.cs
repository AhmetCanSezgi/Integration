using AutoMapper;
using Dehasoft.Business.DTOs;
using Dehasoft.DataAccess.Models;

namespace Dehasoft.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.EntryId, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => decimal.Parse(src.total)))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.Parse(src.order_date)))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.user_id))
                .ForMember(dest => dest.Oid, opt => opt.MapFrom(src => src.oid));

            CreateMap<OrderItemDto, OrderItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.product_id))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.quantity))
                .ForMember(dest => dest.SalePrice, opt => opt.MapFrom(src => decimal.Parse(src.sale_price)));

            CreateMap<ProductBasicDto, Product>()
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.barcode))
                .ForMember(dest => dest.StockCode, opt => opt.MapFrom(src => src.stockcode))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name));
        }
    }
}
