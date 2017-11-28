using System;
using System.Collections.Generic;
using VoicePay.Helpers;
using VoicePay.Models;

namespace VoicePay.ViewModels.Store
{
    public class CartReviewViewModel : BaseViewModel
    {
        public List<CartItem> Products { get; set; }
        public double Total { get; set; }

        public CartReviewViewModel()
        {
            Products = Cart.Instance.GetAllItems();
            Total = Cart.Instance.Checkout();
        }
    }
}
