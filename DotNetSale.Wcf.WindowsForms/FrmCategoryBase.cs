using System;
using System.Windows.Forms;

namespace DotNetSale.Wcf.WindowsForms
{
    public partial class FrmCategoryBase : Form
    {
        public FrmCategoryBase()
        {
            InitializeComponent();
        }

        private void FrmCategoryBase_Load(object sender, EventArgs e)
        {
            DisplayData();
        }

        private void DisplayData()
        {
            var client = new CategoryBaseServiceReference.BreadShopOf_CategoryBaseClient();

            this.ctlCategoryBaseList.DataSource = client.Read();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CategoryBaseServiceReference.CategoryBase model = 
                new CategoryBaseServiceReference.CategoryBase();
            model.CategoryName = txtCategoryName.Text;

            var client = 
                new CategoryBaseServiceReference.BreadShopOf_CategoryBaseClient();
            client.Add(model);

            MessageBox.Show("추가하였습니다.");

            DisplayData(); 
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var client = new CategoryBaseServiceReference.BreadShopOf_CategoryBaseClient();

            var model = client.Browse(int.Parse(txtCategoryId.Text));

            if (model != null)
            {
                txtCategoryName.Text = model.CategoryName; 
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var client = new CategoryBaseServiceReference.BreadShopOf_CategoryBaseClient();

            CategoryBaseServiceReference.CategoryBase model =
                new CategoryBaseServiceReference.CategoryBase();
            model.CategoryId = Convert.ToInt32(txtCategoryId.Text);
            model.CategoryName = txtCategoryName.Text;

            client.Edit(model);

            MessageBox.Show("수정되었습니다.");

            txtCategoryId.Text = "";
            txtCategoryName.Clear(); 

            DisplayData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var client = new CategoryBaseServiceReference.BreadShopOf_CategoryBaseClient();

            var r = client.Delete(Convert.ToInt32(txtCategoryId.Text));

            if (r)
            {
                MessageBox.Show("삭제되었습니다.");
                DisplayData(); 
            }
            else
            {
                MessageBox.Show("삭제되지 않았습니다.");
            }
        }
    }
}
