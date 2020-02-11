using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Library.Api;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<ProductModel> _products;
		private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
		private int _itemQuantity = 1;
		
		IProductEndpoint _productEndpoint;
		public SalesViewModel(IProductEndpoint productEndpoint)
		{
			_productEndpoint = productEndpoint;
		}

		protected override async void OnViewLoaded(object view)
		{
			await LoadProducts();
			base.OnViewLoaded(view);
		}

		private async Task LoadProducts()
		{
			var productList = await _productEndpoint.GetAll();
			
			Products = new BindingList<ProductModel>(productList);
		}

		public BindingList<ProductModel> Products
		{
			get { return _products; }
			set 
			{ 
				_products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}

		private ProductModel _selectedProduct;

		public ProductModel SelectedProduct
		{
			get { return _selectedProduct; }
			set
			{
				_selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set 
			{
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}


		public BindingList<CartItemModel> Cart
		{
			get { return _cart; }
			set 
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		public string SubTotal 
		{
			get
			{
				decimal subTotal = 0;
				foreach(var item in Cart) 
				{
					subTotal += (item.Product.RetailPrice * item.QuantityInCart);
				}
				return subTotal.ToString("C");
			}
		}

		public string Tax
		{
			get
			{
				// TODO replace with calculation
				return "0,00 Ft";
			}
		}
		public string Total
		{
			get
			{
				// TODO replace with calculation
				return "0,00 Ft";
			}
		}

		public void AddToCart()
		{
			CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

			if (existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;

				//HACK - there should be a better way to update qty for existingItem
				Cart.Remove(existingItem);
				Cart.Add(existingItem);
			}
			else
			{
				CartItemModel item = new CartItemModel
				{
					Product = SelectedProduct,
					QuantityInCart = ItemQuantity
				};
				Cart.Add(item);
			}
			SelectedProduct.QuantityInStock -= ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange(() => SubTotal);
		}

		public bool CanAddToCart
		{
			get
			{
				bool output = false;

				//Make sure something is in the cart
				//Make sure the required item quantity is available
				if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
				{
					output = true;
				}
				return output;
			}
		}

		public void RemoveFromCart()
		{

		}

		//public bool CanRemoveFromCart
		//{
		//	get
		//	{
		//		bool output = false;

		//		//Make sure something is selected in the cart
		//		//if ()
		//		//{
		//		//	output = true;
		//		//}
				//NotifyOfPropertyChange(() => SubTotal);
		//		//return output;
		//	}
		//}

		public void CheckOut()
		{

		}

		//public bool CanCheckOut
		//{
		//	get
		//	{
		//		bool output = false;

		//		//Make sure checkout is possible
		//		if (Cart.Count > 0)
		//		{
		//			output = true;
		//		}
		//		return output;
		//	}
		//}

	}
}
