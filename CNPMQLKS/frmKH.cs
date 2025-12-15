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
    public partial class frmKH : DevExpress.XtraEditors.XtraForm
    {
        public frmKH()
        {
            InitializeComponent();
        }
        frmDatPhong objDP = (frmDatPhong)Application.OpenForms["frmDatPhong"];
        frmDatPhongDon objDPDon = (frmDatPhongDon)Application.OpenForms["frmDatPhongDon"];
        bool _them;
        string _idkh;
        public string kh_dp;
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
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?","Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string query = "Delete from KHACHHANG where IDKH =" + _idkh;
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                    loadData();
                }
                catch (Exception)
                {
                    MessageBox.Show("Chưa chọn khách hàng đễ xóa");
                }
            }    
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string hoten = txtHoten.Text;
            string gioitinh = txtGioiTinh.Text;
            string diachi = txtDiaChi.Text;
            string cmnd = txtCMND.Text;
            string sdt = txtSDT.Text;
            if (_them)
            {
                string query = "Insert into KHACHHANG values (N'" + hoten + "',N'" + gioitinh + "','" + cmnd + "','" + sdt + "',N'" + diachi + "')";
                DataProvider provider = new DataProvider();
                provider.ExecuteQuery(query);
            }
            else
            {
                string query = "UPDATE KHACHHANG set HOTEN = N'" + hoten + "', GIOITINH = N'"+ gioitinh +"',SOCMND = '" + cmnd + "',SDT = '" + sdt + "', DIACHI = '" + diachi + "' where IDKH =" + _idkh;
                DataProvider provider = new DataProvider();
                provider.ExecuteQuery(query);
            }
            _them = false;
            loadData();
            showHideControl(true);
            _enebled(false);
            if (objDP != null)
                objDP.loadKH();
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

        private void frmKH_Load(object sender, EventArgs e)
        {
            loadData();
            showHideControl(true);
            _enebled(false);
        }
        void loadData()
        {
            string query = "SELECT * FROM dbo.KHACHHANG";
            DataProvider provider = new DataProvider();
            gcDanhSach.DataSource = provider.ExecuteQuery(query);
            gvDanhSach.OptionsBehavior.Editable = false;
        }
        void _enebled(bool t)
        {
            txtHoten.Enabled = t;
            txtGioiTinh.Enabled = t;
            txtDiaChi.Enabled = t;
            txtCMND.Enabled = t;
            txtSDT.Enabled = t;
        }
        void _reset()
        {
            txtHoten.Text = "";
            txtGioiTinh.Text = "";
            txtDiaChi.Text = "";
            txtCMND.Text = "";
            txtSDT.Text = "";
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
                _idkh = gvDanhSach.GetFocusedRowCellValue("IDKH").ToString();
                txtHoten.Text = gvDanhSach.GetFocusedRowCellValue("HOTEN").ToString();
                txtGioiTinh.Text = gvDanhSach.GetFocusedRowCellValue("GIOITINH").ToString();
                txtDiaChi.Text = gvDanhSach.GetFocusedRowCellValue("DIACHI").ToString();
                txtCMND.Text = gvDanhSach.GetFocusedRowCellValue("SOCMND").ToString();
                txtSDT.Text = gvDanhSach.GetFocusedRowCellValue("SDT").ToString();
            }
        }

        private void gvDanhSach_DoubleClick(object sender, EventArgs e)
        {
            if (gvDanhSach.GetFocusedRowCellValue("IDKH") != null)
            {
                if (kh_dp == "datphongdon")
                {
                    objDPDon.loadKH();
                    objDPDon.getKH(int.Parse(gvDanhSach.GetFocusedRowCellValue("IDKH").ToString()));
                }   
                else
                {
                    objDP.loadKH();
                    objDP.setKhachHang(int.Parse(gvDanhSach.GetFocusedRowCellValue("IDKH").ToString()));
                }    
                this.Close();
            }
        }
    }
}