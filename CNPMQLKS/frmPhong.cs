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
    public partial class frmPhong : DevExpress.XtraEditors.XtraForm
    {
        frmMain frmmain;
        public frmPhong(frmMain frm)
        {
            InitializeComponent();
            frmmain = frm;
        }
        bool _them;
        string _idPhong;
        void loadData()
        {
            string query = "SELECT * FROM dbo.PHONG";
            DataProvider provider = new DataProvider();
            gcDanhSach.DataSource = provider.ExecuteQuery(query);
            gvDanhSach.OptionsBehavior.Editable = false;
        }
        void _enebled(bool t)
        {
            txtTenPhong.Enabled = t;
            txtIDLPhong.Enabled = t;
            txtIDTang.Enabled = t;
            cbTinhTrang.Enabled = t;
        }
        void _reset()
        {
            txtTenPhong.Text = "";
            txtIDLPhong.Text = "";
            txtIDTang.Text = "";
            cbTinhTrang.Text = "";
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
                _idPhong = gvDanhSach.GetFocusedRowCellValue("IDPHONG").ToString();
                txtTenPhong.Text = gvDanhSach.GetFocusedRowCellValue("TENPHONG").ToString();
                txtIDLPhong.Text = gvDanhSach.GetFocusedRowCellValue("IDLOAIPHONG").ToString();
                if (gvDanhSach.GetFocusedRowCellValue("TINHTRANG").ToString() == "False")
                    cbTinhTrang.Text = "Trống";
                else
                    cbTinhTrang.Text = "Có người";
                txtIDTang.Text = gvDanhSach.GetFocusedRowCellValue("IDTANG").ToString();
            }
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
                    string query = "Delete from PHONG where IDPHONG =" + _idPhong;
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                    loadData();
                    frmmain.showRoom();
                }
                catch (Exception)
                {
                    MessageBox.Show("Chưa chọn phòng đễ xóa");
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenphong = txtTenPhong.Text;
            string idloaiphong = txtIDLPhong.Text;
            string tinhtrang = cbTinhTrang.Text;
            if (tinhtrang == "Trống")
                tinhtrang = "0";
            else if (tinhtrang == "Có người")
                tinhtrang = "1";
            string idtang = txtIDTang.Text;
            if (_them)
            {
                try
                {
                    string query = "Insert into PHONG values (N'" + tenphong + "'," + idloaiphong + "," + tinhtrang + "," + idtang + ")";
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                }
                catch (Exception err)
                {
                    MessageBox.Show("Tên phòng không được trùng nhau");
                }
            }
            else
            {
                try
                {
                    string query = "UPDATE PHONG set TENPHONG = N'" + tenphong + "', IDLOAIPHONG = " + idloaiphong + ", TINHTRANG = " + tinhtrang + ",IDTANG = " + idtang + " where IDPHONG =" + _idPhong;
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                }
                catch (Exception err)
                {
                    MessageBox.Show("Tên phòng không được trùng nhau");
                }
            }
            _them = false;
            loadData();
            showHideControl(true);
            _enebled(false);
            frmmain.showRoom();
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

        private void frmPhong_Load(object sender, EventArgs e)
        {
            loadData();
            showHideControl(true);
            _enebled(false);
        }
    }
}