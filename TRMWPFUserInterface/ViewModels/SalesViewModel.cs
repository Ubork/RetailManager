using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helper;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<ProductModel> _products;
		private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
		private int _itemQuantity = 1;
		private ProductModel _selectedProduct;
		private CartItemModel _selectedItemToRemove;

		IProductEndpoint _productEndpoint;
		IConfigHelper _configHelper;
		ISaleEndpoint _saleEndpoint;
		public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndpoint saleEndpoint)
		{
			_productEndpoint = productEndpoint;
			_configHelper = configHelper;
			_saleEndpoint = saleEndpoint;
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

		public CartItemModel SelectedItemToRemove
		{
			get { return _selectedItemToRemove; }
			set 
			{
				_selectedItemToRemove = value;
				NotifyOfPropertyChange(() => SelectedItemToRemove);
				NotifyOfPropertyChange(() => CanRemoveFromCart);
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
				return CalculateSubTotal().ToString("C");
			}
		}

		private decimal CalculateSubTotal()
		{
			decimal subTotal = 0;
			foreach (var item in Cart)
			{
				subTotal += (item.Product.RetailPrice * item.QuantityInCart);
			}
			return subTotal;
		}

		public string Tax
		{
			get
			{
				return CalculateTax().ToString("C");
			}
		}

		private decimal CalculateTax()
		{
			decimal taxAmount = 0;
			decimal taxRate = _configHelper.GetTaxRate();

			taxAmount = Cart.Where(x => x.Product.IsTaxable)
				.Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate * (decimal)0.01);

			//foreach (var item in Cart)
			//{
			//	if (item.Product.IsTaxable)
			//	{
			//		taxAmount += (item.Product.RetailPrice * item.QuantityInCart * taxRate * (decimal)0.01);
			//	}
			//}
			
			return taxAmount;
		}

		public string Total
		{
			get
			{
				decimal total = CalculateSubTotal() + CalculateTax();
				return total.ToString("C");
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
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);

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
			CartItemModel remainerOfItemToRemove = new CartItemModel
			{
				Product = SelectedItemToRemove.Product,
				QuantityInCart = SelectedItemToRemove.QuantityInCart - ItemQuantity
			};

			if (ItemQuantity < SelectedItemToRemove.QuantityInCart)
			{
				Cart.Add(remainerOfItemToRemove);
			}

			ProductModel restockItem = Products.FirstOrDefault(x => x.Equals(SelectedItemToRemove.Product));
			Cart.Remove(SelectedItemToRemove);
			SelectedProduct.QuantityInStock += ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;

				//Make sure something is in the cart
				//Make sure the required item quantity is available
				if (SelectedItemToRemove?.QuantityInCart >= ItemQuantity)
				{
					output = true;
				}
				return output;
			}
		}
		public bool CanCheckOut
		{
			get
			{
				bool output = false;

				//Make sure checkout is possible
				if (Cart.Count > 0)
				{
					output = true;
				}
				return output;
			}
		}

		public async Task CheckOut()
		{
			SaleModel sale = new SaleModel();

			foreach (var item in Cart)
			{
				sale.SaleDetails.Add(new SaleDetailModel
				{
					ProductId = item.Product.Id,
					Quantity = item.QuantityInCart
				});
			}

			await _saleEndpoint.PostSale(sale);
		}


	}
}
