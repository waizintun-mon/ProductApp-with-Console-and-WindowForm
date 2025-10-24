using ProductConsoleApp;
using System.Drawing.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ProductConsoleApp.ProductService;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace ProductWindFormApp
{
    public partial class FrmProduct : Form
    {
        private readonly ProductRestService productRestService;
        private readonly ProductService productService;
        public FrmProduct()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            productService = new ProductService();
            productRestService = new ProductRestService();
        }


        private async Task<List<ProductService.ProductModel>> BindDataAsync()
        {

            ProductService productService = new ProductService();
            var lst = await productService.GetProducts();
            dataGridView1.DataSource = lst;
            return lst;
        }


        public async void FrmProduct_Load(object sender, EventArgs e)
        {
            await BindDataAsync();

        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            CheckValidation();

            var model = await productService.CreateProduct(new ProductService.ProductCreateRequestModel
            {
                ProductCode = txtProductCode.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                Price = Convert.ToDecimal(txtPrice.Text.Trim()),
                Quantity = Convert.ToInt32(txtQuantity.Text.Trim())
            });
            string message = model.Message;
            MessageBox.Show(message);
            await BindDataAsync();
        }
        private bool CheckValidation()
        {
            string code = txtProductCode.Text.Trim();
            string name = txtProductName.Text.Trim();
            // decimal price = Convert.ToDecimal(txtPrice.Text.Trim());
            //int qty = Convert.ToInt32(txtQuantity.Text.Trim());  
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Please enter product code", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter product name", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(txtPrice.Text.Trim()))
            {
                MessageBox.Show("Please enter price", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(txtQuantity.Text.Trim()))
            {
                MessageBox.Show("Please enter product quantity", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }
        string id = null;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 && e.RowIndex == -1)
            {
                return;
            }
            if (e.ColumnIndex == dataGridView1.Columns["colEdit"].Index)
            {
                var productId = dataGridView1.Rows[e.RowIndex].Cells["colProductId"].Value.ToString();
                txtPrice.Text = dataGridView1.Rows[e.RowIndex].Cells["colPrice"].Value.ToString();
                txtProductCode.Text = dataGridView1.Rows[e.RowIndex].Cells["colProductCode"].Value.ToString();
                txtProductName.Text = dataGridView1.Rows[e.RowIndex].Cells["colProductName"].Value.ToString();
                txtQuantity.Text = dataGridView1.Rows[e.RowIndex].Cells["colQuantity"].Value.ToString();
                id = productId!;
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    MessageBox.Show("Please select a product to update.");
                    return;
                }

                var updateRequest = new ProductUpdateRequestModel
                {
                    ProductId = id,
                    ProductCode = txtProductCode.Text.Trim(),
                    ProductName = txtProductName.Text.Trim(),
                    Price = decimal.Parse(txtPrice.Text),
                    Quantity = int.Parse(txtQuantity.Text)
                };

                var result = await productService.UpdateProduct(updateRequest);
                MessageBox.Show(result.Message, "Update Product", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await BindDataAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    MessageBox.Show("Please select a product to Delete.");
                    return;
                }
                var deleteRequest = new ProductDeleteRequestModel
                {
                    ProductId = id,
                };

                DialogResult confirm = MessageBox.Show("Are you sure want to Delete?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return; 

                var result = await productRestService.DeleteProduct(deleteRequest);
                MessageBox.Show(result.Message, "Delete Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await BindDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Deleting product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           }
    }
}
