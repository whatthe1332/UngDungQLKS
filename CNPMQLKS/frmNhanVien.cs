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
    public partial class frmNhanVien : DevExpress.XtraEditors.XtraForm
    {
        public frmNhanVien()
        {
            InitializeComponent();
        }
        bool _them;
        int _idNV = 0;
        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            loadComboboxCV();
            loadData();
            showHideControl(true);
            _enebled(false);
        }
        void loadComboboxCV()
        {
            string query = "SELECT TENCHUCVU FROM dbo.CHUCVU";
            DataProvider provider = new DataProvider();
            cboChucVu.DisplayMember = "TENCHUCVU";
            cboChucVu.DataSource = provider.ExecuteQuery(query);
        }
        void loadData()
        {
            string query = "SELECT TAIKHOAN,MATKHAU,TENNV,TENCHUCVU,TRANGTHAI FROM NHANVIEN, CHUCVU WHERE NHANVIEN.IDCHUCVU = CHUCVU.IDCHUCVU";
            DataProvider provider = new DataProvider();
            gcDanhSach.DataSource = provider.ExecuteQuery(query);
            gvDanhSach.OptionsBehavior.Editable = false;
        }
        void _enebled(bool t)
        {
            txtTaiKhoan.Enabled = t;
            txtMatKhau.Enabled = t;
            txtTenNV.Enabled = t;
            cboChucVu.Enabled = t;
            cboTrangThai.Enabled = t;
        }
        void _reset()
        {
            txtTaiKhoan.Text = "";
            txtMatKhau.Text = "";
            txtTenNV.Text = "";
            cboChucVu.Text = "";
            cboTrangThai.SelectedIndex = 0;
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
                    string query = "Delete from NHANVIEN where IDNV =" + _idNV;
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                    loadData();
                }
                catch (Exception)
                {
                    MessageBox.Show("Chưa chọn thiết bị trong phòng đễ xóa");
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboChucVu.Text == "")
            {
                MessageBox.Show("Chức vụ không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }    
            string taikhoan = txtTaiKhoan.Text;
            string matkhau = txtMatKhau.Text;
            string tennv = txtTenNV.Text;
            string tencv = cboChucVu.Text;
            int idcv = 0;
            string queryB = $"SELECT * FROM dbo.CHUCVU where TENCHUCVU = N'{tencv}'";
            DataProvider providerB = new DataProvider();
            DataTable dt2 = new DataTable();
            dt2 = providerB.ExecuteQuery(queryB);
            foreach (DataRow row in dt2.Rows)
                idcv = int.Parse(row["IDCHUCVU"].ToString());
            int trangthai = 0;
            if (cboTrangThai.Text == "Hoạt động")
                trangthai = 1;
            else
                trangthai = 0;
            try
            {
                if (_them)
                {
                    try
                    {
                        string query = $"Insert into NHANVIEN values ('{taikhoan}','{matkhau}', N'{tennv}',{idcv},{trangthai})";
                        DataProvider provider = new DataProvider();
                        provider.ExecuteQuery(query);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("Tài khoản nhân viên không được trùng nhau");
                    }
        }
                else
                {
                    try
                    {
                        string query = $"UPDATE NHANVIEN set TAIKHOAN = '{taikhoan}', MATKHAU = '{matkhau}', TENNV = N'{tennv}', IDCHUCVU = {idcv}, TRANGTHAI = {trangthai} where IDNV = {_idNV}";
                        DataProvider provider = new DataProvider();
                        provider.ExecuteQuery(query);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("Tài khoản nhân viên không được trùng nhau");
                    }
                }
                _them = false;
                loadData();
                showHideControl(true);
                _enebled(false);
            }
            catch (Exception)
            {
                MessageBox.Show("Nhân viên không tồn tại");
            }
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
                string taikhoan = gvDanhSach.GetFocusedRowCellValue("TAIKHOAN").ToString();
                string matkhau = gvDanhSach.GetFocusedRowCellValue("MATKHAU").ToString();
                string tennv = gvDanhSach.GetFocusedRowCellValue("TENNV").ToString();
                string queryA = $"SELECT * FROM NHANVIEN WHERE TAIKHOAN = '{taikhoan}' AND MATKHAU = '{matkhau}'";
                DataProvider providerA = new DataProvider();
                DataTable dt = new DataTable();
                dt = providerA.ExecuteQuery(queryA);
                foreach (DataRow row in dt.Rows)
                {
                    _idNV = int.Parse(row["IDNV"].ToString());
                }
                txtTaiKhoan.Text = taikhoan;
                txtMatKhau.Text = matkhau;
                txtTenNV.Text = tennv;
                cboChucVu.Text = gvDanhSach.GetFocusedRowCellValue("TENCHUCVU").ToString();
                if ((bool)gvDanhSach.GetFocusedRowCellValue("TRANGTHAI") == true)
                    cboTrangThai.SelectedIndex = 0;
                else
                    cboTrangThai.SelectedIndex = 1;

            }
        }
    }
}