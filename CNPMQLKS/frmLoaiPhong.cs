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
    public partial class frmLoaiPhong : DevExpress.XtraEditors.XtraForm
    {
        public frmLoaiPhong()
        {
            InitializeComponent();
        }
        bool _them;
        string _idlp;

        private void frmLoaiPhong_Load(object sender, EventArgs e)
        {
            loadData();
            showHideControl(true);
            _enebled(false);
        }
        void loadData()
        {
            string query = "SELECT * FROM dbo.LOAIPHONG";
            DataProvider provider = new DataProvider();
            gcDanhSach.DataSource = provider.ExecuteQuery(query);
            gvDanhSach.OptionsBehavior.Editable = false;
        }
        void _enebled(bool t)
        {
            txtTenLoaiPhong.Enabled = t;
            txtSoGiuong.Enabled = t;
            txtSoNguoiTD.Enabled = t;
            txtDonGia.Enabled = t;
            txtGhiChu.Enabled = t;
        }
        void _reset()
        {
            txtTenLoaiPhong.Text = "";
            txtSoGiuong.Text = "";
            txtSoNguoiTD.Text = "";
            txtDonGia.Text = "";
            txtGhiChu.Text = "";
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
                    string query = "Delete from LOAIPHONG where IDLOAIPHONG =" + _idlp;
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                    loadData();
                }
                catch (Exception)
                {
                    MessageBox.Show("Chưa chọn loại phòng đễ xóa");
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenloaiphong = txtTenLoaiPhong.Text;
            string sogiuong = txtSoGiuong.Text;
            string songuoitoida = txtSoNguoiTD.Text;
            string dongia = txtDonGia.Text;
            string ghichu = txtGhiChu.Text;
            if (_them)
            {
                string query = "Insert into LOAIPHONG values (N'" + tenloaiphong + "'," + sogiuong + "," + songuoitoida + "," + dongia + ",N'" + ghichu + "')";
                DataProvider provider = new DataProvider();
                provider.ExecuteQuery(query);
            }
            else
            {
                string query = "UPDATE LOAIPHONG set TENLOAIPHONG = N'" + tenloaiphong + "', SOGIUONG = " + sogiuong + ",SONGUOITOIDA = " + songuoitoida + ",DONGIA = " + dongia + ", GHICHU = N'" + ghichu + "' where IDLOAIPHONG =" + _idlp;
                DataProvider provider = new DataProvider();
                provider.ExecuteQuery(query);
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
                _idlp = gvDanhSach.GetFocusedRowCellValue("IDLOAIPHONG").ToString();
                txtTenLoaiPhong.Text = gvDanhSach.GetFocusedRowCellValue("TENLOAIPHONG").ToString();
                txtSoGiuong.Text = gvDanhSach.GetFocusedRowCellValue("SOGIUONG").ToString();
                txtSoNguoiTD.Text = gvDanhSach.GetFocusedRowCellValue("SONGUOITOIDA").ToString();
                txtDonGia.Text = gvDanhSach.GetFocusedRowCellValue("DONGIA").ToString();
                txtGhiChu.Text = gvDanhSach.GetFocusedRowCellValue("GHICHU").ToString();
            }
        }
    }
}