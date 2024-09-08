using IndustryConnect_Week5_WebApi.Dtos;
using IndustryConnect_Week5_WebApi.Models;

namespace IndustryConnect_Week5_WebApi.Mappers
{
    public static class SaleMapper
    {
        public static SaleDto EntityToDto(Sale sale)
        {
            return new SaleDto
            {
                
                StoreName = sale.Store.Name,
                CustomerName = $"{sale.Customer.FirstName} {sale.Customer.LastName}",
                ProductName = sale.Product.Name
            };
        }
    }
}
