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
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        int idnv = 0;
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (checkAccount(txtTaiKhoan.Text,txtMatKhau.Text))
            {
                frmMain frmMain = new frmMain(idnv);
                frmMain.ShowDialog();
                this.Close();
            }  
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu sai", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }    
        }
        bool checkAccount(string taikhoan, string matkhau)
        {
            string query = $"SELECT * FROM NHANVIEN WHERE TAIKHOAN = '{taikhoan}' AND MATKHAU = '{matkhau}'";
            DataProvider provider = new DataProvider();
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                idnv = int.Parse(row["IDNV"].ToString());
                return true;
            }
            return false;
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}