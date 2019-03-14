using Accounting.App.Customers;
using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.App
{
    public partial class frmCustomers : Form
    {
        public frmCustomers()
        {
            InitializeComponent();
        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            BindGrid();
        }
        void BindGrid()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgCustomer.AutoGenerateColumns = false;
                dgCustomer.DataSource = db.CustomerRepository.GetAllCustomer();
            }
        }

        private void btnRefreshCustomer_Click(object sender, EventArgs e)
        {
            txtFilter.Text = null;
            BindGrid();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgCustomer.DataSource = db.CustomerRepository.GetCustomersByFilter(txtFilter.Text);
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dgCustomer.CurrentRow != null)
            {

                using (UnitOfWork db = new UnitOfWork())
                {
                    string name = dgCustomer.CurrentRow.Cells[1].Value.ToString();
                    if (RtlMessageBox.Show($"آیا از حذف {name} مطمئن هستید ؟", "توجه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        int customerid = int.Parse(dgCustomer.CurrentRow.Cells[0].Value.ToString());
                        db.CustomerRepository.DeleteCustomer(customerid);
                        db.save();
                        BindGrid();
                    }

                }
            }
            else
            {
                RtlMessageBox.Show("لطفا خطی را انتخاب نمایید");
            }
        }

        private void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            frmAddOrEditCustomr frmAddOrEditCustomr = new frmAddOrEditCustomr();
            if (frmAddOrEditCustomr.ShowDialog() == DialogResult.OK)
            {
                BindGrid();
            }

        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            if (dgCustomer.CurrentRow != null)
            {
                int customerid = int.Parse(dgCustomer.CurrentRow.Cells[0].Value.ToString());
                frmAddOrEditCustomr frmAddOrEdit = new frmAddOrEditCustomr();
                frmAddOrEdit.customerId = customerid;
                if (frmAddOrEdit.ShowDialog() == DialogResult.OK)
                {
                    BindGrid();
                }

            }
        }
    }
}
