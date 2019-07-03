using OrderApi.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class OrderItem
    {
        public int Id{ get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }
        public int ProductId { get; set; }
        protected OrderItem() { }
        public Order Order { get; set; }
        public int OrderId { get; set; }

        public OrderItem(int productId, string productName, decimal unitPrice, string pictureUrl, int units = 1)
        {
            if(units <= 0)
            {
                throw new OrderingDomainException("Invalid number of units");
            }

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Units = units;
            PictureUrl = pictureUrl;
        }

        public void SetPictureUri(string pictureUrl)
        {
            if (!String.IsNullOrWhiteSpace(pictureUrl))
            {
                PictureUrl = pictureUrl;
            }
        }
    }
}
