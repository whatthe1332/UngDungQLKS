using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CNPMQLKS.DAO;

namespace CNPMQLKS
{
    public partial class frmForgetPass : DevExpress.XtraEditors.XtraForm
    {
        public frmForgetPass()
        {
            InitializeComponent();
        }
        frmMain objMain = (frmMain)Application.OpenForms["frmMain"];
        private void btnDoiPass_Click(object sender, EventArgs e)
        {
            if (txtNewPass.Text != "" && txtNewPass.Text != "")
            {
                string query = "SELECT * FROM dbo.NHANVIEN WHERE IDNV = " + objMain._idnv;
                DataProvider provider = new DataProvider();
                DataTable dt = new DataTable();
                dt = provider.ExecuteQuery(query);
                foreach (DataRow row in dt.Rows)
                {
                    if (lblTaiKhoan.Text == row["TAIKHOAN"].ToString() &&  txtOldPass.Text == row["MATKHAU"].ToString())
                    {
                        string query2 = $"UPDATE NHANVIEN SET MATKHAU = '{txtNewPass.Text}' WHERE IDNV = {objMain._idnv}";
                        provider.ExecuteQuery(query2);
                        MessageBox.Show("Cập nhật mật khẩu mới thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu bị sai", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }    
                }
            }    
        }

        private void frmForgetPass_Load(object sender, EventArgs e)
        {
            string query = "SELECT * FROM dbo.NHANVIEN WHERE IDNV = " + objMain._idnv;
            DataProvider provider = new DataProvider();
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                lblTaiKhoan.Text = row["TAIKHOAN"].ToString();
            }    
        }
    }
}