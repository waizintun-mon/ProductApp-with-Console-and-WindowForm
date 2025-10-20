using ProductConsoleApp;
using System.Drawing.Text;

namespace ProductWindFormApp
{
    public partial class FrmProduct : Form
    {
        public FrmProduct()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
          
        }

        public async void FrmProduct_Load(object sender, EventArgs e)
        {
            ProductService productService = new ProductService();
            var lst = await productService.GetProducts();
            dataGridView1.DataSource = lst;

        }

       
    }
}
