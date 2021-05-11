﻿namespace DragonSouvenirs.Web.ViewModels.Components.OrderProductsComponent
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels.Cart;

    public class CartProductsViewModel : IMapFrom<CartProduct>
    {
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public string ProductDescriptionShort
        {
            get
            {
                if (this.ProductDescription == null)
                {
                    return string.Empty;
                }

                var content = this.ProductDescription.Length > 30
                    ? this.ProductDescription.Substring(0, 30) + "..."
                    : this.ProductDescription;
                return content;
            }
        }

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }

        public int ProductHeight { get; set; }

        public int ProductWidth { get; set; }

        public IEnumerable<CartProductImageViewModel> ProductImages { get; set; }

        public IEnumerable<CartProductCategoryViewModel> ProductProductCategories { get; set; }

        public decimal TotalPrice => this.Quantity * this.ProductPrice;
    }
}
