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
    public partial class frmThietBi : DevExpress.XtraEditors.XtraForm
    {
        public frmThietBi()
        {
            InitializeComponent();
        }
        bool _them;
        string _idTB;
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
                    string query = "Delete from THIETBI where IDTB =" + _idTB;
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                    loadData();
                }
                catch (Exception)
                {
                    MessageBox.Show("Chưa chọn thiết bị đễ xóa");
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenthietbi = txtTenThietBi.Text;
            string dongia = txtDonGia.Text;
            if (_them)
            {
                try
                {
                    string query = "Insert into THIETBI values (N'" + tenthietbi + "'," + dongia + ")";
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                }
                catch (Exception err)
                {
                    MessageBox.Show("Tên thiết bị không được trùng nhau");
                }
            }
            else
            {
                try
                {
                    string query = "UPDATE THIETBI set TENTB = N'" + tenthietbi + "',DONGIA = " + dongia + " where IDTB =" + _idTB;
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                }
                catch (Exception err)
                {
                    MessageBox.Show("Tên thiết bị không được trùng nhau");
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

        private void frmThietBi_Load(object sender, EventArgs e)
        {
            loadData();
            showHideControl(true);
            _enebled(false);
        }
        void loadData()
        {
            string query = "SELECT * FROM dbo.THIETBI";
            DataProvider provider = new DataProvider();
            gcDanhSach.DataSource = provider.ExecuteQuery(query);
            gvDanhSach.OptionsBehavior.Editable = false;
        }
        void _enebled(bool t)
        {
            txtTenThietBi.Enabled = t;
            txtDonGia.Enabled = t;
        }
        void _reset()
        {
            txtTenThietBi.Text = "";
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
        private void gvDanhSach_Click(object sender, EventArgs e)
        {
            if (gvDanhSach.RowCount > 0)
            {
                _idTB = gvDanhSach.GetFocusedRowCellValue("IDTB").ToString();
                txtTenThietBi.Text = gvDanhSach.GetFocusedRowCellValue("TENTB").ToString();
                txtDonGia.Text = gvDanhSach.GetFocusedRowCellValue("DONGIA").ToString();
            }
        }
    }
}