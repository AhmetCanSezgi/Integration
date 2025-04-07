using Dehasoft.Business.Services;
using Dehasoft.DataAccess.Models;
using Dehasoft.DataAccess.Repositories;

namespace Dehasoft.WinForms
{
    public partial class Form1 : Form
    {
        private readonly IOrderService _orderService;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;

        public Form1(IOrderService orderService, IProductRepository productRepository, IProductService productService)
        {
            InitializeComponent();
            _orderService = orderService;
            _productRepository = productRepository;
            _productService = productService;
        }

        private async void btnSync_Click(object sender, EventArgs e)
        {
            btnSync.Enabled = false;
            try
            {
                lblStatus.Text = "Ýþlem devam ediyor...";
                await _orderService.FetchAndProcessOrdersAsync(1, 10);
                lblStatus.Text = "Sipariþ senkronizasyonu tamamlandý.";
                MessageBox.Show("Sipariþler baþarýyla senkronize edildi.");
                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Bir hata oluþtu.";
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                btnSync.Enabled = true;
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            using var conn = _productRepository.GetDbConnection();
            var products = await _productRepository.GetAllAsync(conn);
            dgvProducts.DataSource = products;
        }

        private async void btnBuy_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir ürün seçin.");
                return;
            }

            var selectedProduct = (Product)dgvProducts.SelectedRows[0].DataBoundItem;
            var quantity = (int)numQuantity.Value;

            if (selectedProduct.Stock < quantity)
            {
                MessageBox.Show("Yetersiz stok.");
                return;
            }

            var orderItem = new OrderItem
            {
                ProductId = selectedProduct.Id,
                Quantity = quantity,
                SalePrice = selectedProduct.Price
            };

            var success = await _productService.ProcessOrderItemAsync(orderItem);

            if (success)
            {
                MessageBox.Show("Satýn alma baþarýlý.\nÜrün: " + selectedProduct.Name +
                    "\nMiktar: " + quantity +
                    "\nKalan Stok: " + (selectedProduct.Stock - quantity));
                await LoadProductsAsync();
            }
            else
            {
                MessageBox.Show("Ýþlem sýrasýnda hata oluþtu.\nÜrün: " + selectedProduct.Name);
            }
        }
    }
}