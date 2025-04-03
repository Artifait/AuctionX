using System;
using System.Collections.Generic;
using AucX.Domain.Entities;

namespace AucX.Domain.Entities
{
    public class ColorShop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; } = 5000;  // Цена за цвет
        public string ColorCode { get; set; }  // HEX код цвета
    }

    public class UserColorPurchase
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // Связь с пользователем
        public AppUser User { get; set; }
        public int ColorShopId { get; set; }  // Связь с магазином цветов
        public ColorShop ColorShop { get; set; }
        public DateTime DatePurchased { get; set; }  // Дата покупки
    }
}
