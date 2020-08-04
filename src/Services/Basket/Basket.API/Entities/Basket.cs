namespace Basket.API.Entities
{
    using System.Collections.Generic;
    public class Basket
    {
        public string UserName { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

        public Basket()
        {
            
        }

        public Basket(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                foreach (var item in BasketItems)
                {
                    totalPrice += item.Price * item.Quantity;
                }
                return totalPrice;
            }
        }

    }
}
