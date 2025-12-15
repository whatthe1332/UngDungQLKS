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
    public partial class frmDichVu : DevExpress.XtraEditors.XtraForm
    {
        public frmDichVu()
        {
            InitializeComponent();
        }
        bool _them;
        string _idDV;
        private void frmDichVu_Load(object sender, EventArgs e)
        {
            loadData();
            showHideControl(true);
            _enebled(false);
        }
        void loadData()
        {
            string query = "SELECT * FROM dbo.DICHVU";
            DataProvider provider = new DataProvider();
            gcDanhSach.DataSource = provider.ExecuteQuery(query);
            gvDanhSach.OptionsBehavior.Editable = false;
        }
        void _enebled(bool t)
        {
            txtTenDichVu.Enabled = t;
            txtDonGia.Enabled = t;
        }
        void _reset()
        {
            txtTenDichVu.Text = "";
            txtDonGia.Text = "";
        }
        void showHideControl(bool t)
        {
            btnThem.Visible = t;
            btnSua.Visible = t;
            btnXoa.Visible = t;
            btnThoat.Visible = t;
            btnLuu.Visible = !t;
            btnBoQua.Visible = !t;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            _them = true;
            showHideControl(false);
            _enebled(true);
            _reset();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            _them = false;
            showHideControl(false);
            _enebled(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string query = "Delete from DICHVU where IDDV =" + _idDV;
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                    loadData();
                }
                catch (Exception)
                {
                    MessageBox.Show("Chưa chọn dịch vụ đễ xóa");
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tendichvu = txtTenDichVu.Text;
            string dongia = txtDonGia.Text;
            if (_them)
            {
                try
                {
                    string query = "Insert into DICHVU values (N'" + tendichvu + "', " + dongia + ")";
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                }
                catch (Exception err)
                {
                    MessageBox.Show("Tên dịch vụ không được trùng nhau");
                }
            }
            else
            {
                try
                {
                    string query = "UPDATE DICHVU set TENDV = N'" + tendichvu + "', DONGIA = " + dongia + " where IDDV =" + _idDV;
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                }
                catch (Exception err)
                {
                    MessageBox.Show("Tên dịch vụ không được trùng nhau");
                }
            }
            _them = false;
            loadData();
            showHideControl(true);
            _enebled(false);
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            _them = false;
            showHideControl(true);
            _enebled(false);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvDanhSach_Click(object sender, EventArgs e)
        {
            if (gvDanhSach.RowCount > 0)
            {
                _idDV = gvDanhSach.GetFocusedRowCellValue("IDDV").ToString();
                txtTenDichVu.Text = gvDanhSach.GetFocusedRowCellValue("TENDV").ToString();
                txtDonGia.Text = gvDanhSach.GetFocusedRowCellValue("DONGIA").ToString();
            }
        }
    }
}