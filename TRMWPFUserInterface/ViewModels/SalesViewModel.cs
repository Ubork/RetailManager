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

		private BindingList<ProductModel> _products;
		private int _itemQuantity;
		private BindingList<string> _cart;

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


		public BindingList<string> Cart
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
				// TODO replace with calculation
				return "0,00 Ft";
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
