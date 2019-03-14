using Accounting.DataLayer.Context;
using Accounting.DataLayer;
using Accounting.DataLayer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationComponents;
using System.IO;

namespace Accounting.App.Customers
{
    public partial class frmAddOrEditCustomr : Form
    {
        public int customerId = 0;
        UnitOfWork db = new UnitOfWork();
        public frmAddOrEditCustomr()
        {
            InitializeComponent();
        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pcCustomer.ImageLocation = openFile.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                string imagName = Guid.NewGuid().ToString() + Path.GetExtension(pcCustomer.ImageLocation);   //نام و پسوند عکس
                string path = Application.StartupPath + "/Images/" ;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                pcCustomer.Image.Save(path+imagName);
                Accounting.DataLayer.Customers customers = new Accounting.DataLayer.Customers()
                {
                    Address = txtAddress.Text,
                    Email = txtEmail.Text,
                    FullName = txtName.Text,
                    Mobile = txtMobile.Text,
                    CustomerImage = imagName
                };
                if (customerId == 0)
                {
                    db.CustomerRepository.InsertCustomer(customers);
                   
                }
                else
                {
                    customers.CustomerID = customerId;
                    db.CustomerRepository.UpdateCustomer(customers);
                   
                }
                db.save();
                DialogResult = DialogResult.OK;

            }
        }

        private void frmAddOrEditCustomr_Load(object sender, EventArgs e)
        {
            if (customerId != 0)
            {
                this.Text = "ویرایش شخص !";
                btnSave.Text = "ویرایش";
                var customer = db.CustomerRepository.GetCustomerById(customerId);
                txtName.Text = customer.FullName;
                txtEmail.Text = customer.Email;
                txtMobile.Text = customer.Mobile;
                txtAddress.Text = customer.Address;
                pcCustomer.ImageLocation = Application.StartupPath + "/Images/" + customer.CustomerImage;
            }
        }
    }
}
