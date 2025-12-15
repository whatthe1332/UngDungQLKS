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
    public partial class frmDatPhongDon : DevExpress.XtraEditors.XtraForm
    {
        public frmDatPhongDon()
        {
            InitializeComponent();
        }
        public bool _thanhtoan;
        public bool _them;
        public int _idPhong;
        int _idDP = 0;
        OBJ_PHONG _phongHienTai = new OBJ_PHONG();
        frmMain objMain = (frmMain)Application.OpenForms["frmMain"];
        List<OBJ_DPDV> lstDPDV;
        private void btnLuu_Click(object sender, EventArgs e)
        {
            saveData();
            objMain.showRoom();
            this.Close();
        }
        void saveData()
        {
            if (_them)
            {
                try
                {
                    int idnv = objMain._idnv;
                    int theodoan = 0;
                    int disabled = 0;
                    string datenow = DateTime.Now.ToString("dd/MM/yyyy");
                    string query = $"INSERT INTO DATPHONG VALUES({int.Parse(cboKhachHang.SelectedValue.ToString())},CONVERT(datetime,'{dtNgayDat.Text}',103),CONVERT(datetime,'{dtNgayTra.Text}',103),{double.Parse(txtThanhTien.Text)},{int.Parse(spSoNguoi.Value.ToString())},N'{cboTrangThai.Text}',{idnv},{theodoan},N'{txtGhiChu.Text}',{disabled},CONVERT(datetime,'{datenow}',103),null)";
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                    string query6 = "SELECT IDENT_CURRENT('DATPHONG') as [IDDP]";
                    DataTable dt = new DataTable();
                    dt = provider.ExecuteQuery(query6);
                    string IDDP = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        IDDP = row["IDDP"].ToString();
                        _idDP = int.Parse(IDDP);
                    }
                    string query2 = $"INSERT INTO DATPHONG_CT VALUES({IDDP},{_idPhong},{dtNgayTra.Value.Day - dtNgayDat.Value.Day},{int.Parse(_phongHienTai.DONGIA.ToString())},{(dtNgayTra.Value.Day - dtNgayDat.Value.Day) * int.Parse(_phongHienTai.DONGIA.ToString())})";
                    provider.ExecuteQuery(query2);
                    string query7 = "SELECT IDENT_CURRENT('DATPHONG_CT') as [IDDPCT]";
                    DataTable dt2 = new DataTable();
                    dt2 = provider.ExecuteQuery(query7);
                    string IDDPCT = "";
                    foreach (DataRow row in dt2.Rows)
                    {
                        IDDPCT = row["IDDPCT"].ToString();
                    }
                    string query3 = $"UPDATE PHONG SET TINHTRANG = 1 where IDPHONG = {_phongHienTai.IDPHONG}";
                    provider.ExecuteQuery(query3);
                    if (gvSPDV.RowCount > 0)
                    {
                        for (int j = 0; j < gvSPDV.RowCount; j++)
                        {
                            if (_phongHienTai.IDPHONG == int.Parse(gvSPDV.GetRowCellValue(j, "IDPHONG").ToString()))
                            {
                                string query4 = $"INSERT INTO DATPHONG_DICHVU VALUES ({IDDP},{IDDPCT},{int.Parse(gvSPDV.GetRowCellValue(j, "IDPHONG").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "IDDV").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "SOLUONG").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "DONGIA").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "SOLUONG").ToString()) * int.Parse(gvSPDV.GetRowCellValue(j, "DONGIA").ToString())})";
                                provider.ExecuteQuery(query4);
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("Ngày đặt phòng phải bé hơn hoặc bàng ngày trả phòng");
                }
            }
            else
            {
                try
                {
                    int idnv = 1;
                    int theodoan = 1;
                    int disabled = 0;
                    string datenow = DateTime.Now.ToString("dd/MM/yyyy");
                    string query = $"UPDATE DATPHONG SET IDKH = {int.Parse(cboKhachHang.SelectedValue.ToString())}, NGAYDATPHONG = CONVERT(datetime,'{dtNgayDat.Text}',103), NGAYTRAPHONG = CONVERT(datetime,'{dtNgayTra.Text}',103), SOTIEN = {double.Parse(txtThanhTien.Text)}, SONGUOIO = {int.Parse(spSoNguoi.Value.ToString())}, TRANGTHAI = N'{cboTrangThai.Text}', IDNV = {idnv}, THEODOAN = {theodoan}, GHICHU = N'{txtGhiChu.Text}', DISABLED = {disabled}, NGAYCHINHSUA = CONVERT(datetime,'{datenow}',103) WHERE IDDP = {_idDP}";
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                    string query2 = $"DELETE FROM DATPHONG_DICHVU WHERE IDDP = {_idDP}";
                    provider.ExecuteQuery(query2);
                    string query3 = $"DELETE FROM DATPHONG_CT WHERE IDDP = {_idDP}";
                    provider.ExecuteQuery(query3);
                    string query8 = $"INSERT INTO DATPHONG_CT VALUES({_idDP},{_phongHienTai.IDPHONG},{dtNgayTra.Value.Day - dtNgayDat.Value.Day},{_phongHienTai.DONGIA},{(dtNgayTra.Value.Day - dtNgayDat.Value.Day) * _phongHienTai.DONGIA})";
                    provider.ExecuteQuery(query8);
                    string query7 = "SELECT IDENT_CURRENT('DATPHONG_CT') as [IDDPCT]";
                    DataTable dt2 = new DataTable();
                    dt2 = provider.ExecuteQuery(query7);
                    string IDDPCT = "";
                    foreach (DataRow row in dt2.Rows)
                    {
                        IDDPCT = row["IDDPCT"].ToString();
                    }
                    string query9 = $"UPDATE PHONG SET TINHTRANG = 1 where IDPHONG = {_idPhong}";
                    provider.ExecuteQuery(query9);
                    if (gvSPDV.RowCount > 0)
                    {
                        for (int j = 0; j < gvSPDV.RowCount; j++)
                        {
                            if (_idPhong == int.Parse(gvSPDV.GetRowCellValue(j, "IDPHONG").ToString()))
                            {
                                string query4 = $"INSERT INTO DATPHONG_DICHVU VALUES ({_idDP},{IDDPCT},{int.Parse(gvSPDV.GetRowCellValue(j, "IDPHONG").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "IDDV").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "SOLUONG").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "DONGIA").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "SOLUONG").ToString()) * int.Parse(gvSPDV.GetRowCellValue(j, "DONGIA").ToString())})";
                                provider.ExecuteQuery(query4);
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("Ngày đặt phòng phải bé hơn hoặc bàng ngày trả phòng");
                }
            }
        }
        private void btnIn_Click(object sender, EventArgs e)
        {
            if (_thanhtoan)
            {
                string query = $"UPDATE DATPHONG SET TRANGTHAI = N'Đã tính tiền' WHERE IDDP = {_idDP}";
                DataProvider provider = new DataProvider();
                provider.ExecuteQuery(query);
                string query2 = $"UPDATE PHONG SET TINHTRANG = 0 where IDPHONG = {_idPhong}";
                provider.ExecuteQuery(query2);
                objMain.showRoom();
                this.Close();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDatPhongDon_Load(object sender, EventArgs e)
        {
            lstDPDV = new List<OBJ_DPDV>();
            string query = "SELECT * FROM dbo.PHONG WHERE IDPHONG = " + _idPhong;
            DataProvider provider = new DataProvider();
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                _phongHienTai.IDPHONG = int.Parse(row["IDPHONG"].ToString());
                _phongHienTai.TENPHONG = row["TENPHONG"].ToString();
                _phongHienTai.IDLOAIPHONG = int.Parse(row["IDLOAIPHONG"].ToString());
                _phongHienTai.TINHTRANG = (bool) row["TINHTRANG"];
                _phongHienTai.IDTANG = int.Parse(row["IDTANG"].ToString());
                string query2 = "SELECT * FROM LOAIPHONG WHERE IDLOAIPHONG = " + _phongHienTai.IDLOAIPHONG;
                DataTable dt2 = new DataTable();
                dt2 = provider.ExecuteQuery(query2);
                foreach (DataRow row2 in dt2.Rows)
                    _phongHienTai.DONGIA = int.Parse(row2["DONGIA"].ToString());
                lblPhong.Text = _phongHienTai.TENPHONG + " - Đơn giá: " + _phongHienTai.DONGIA.ToString("N0") + " VNĐ";
            }
            dtNgayDat.Value = DateTime.Now;
            dtNgayTra.Value = DateTime.Now.AddDays(1);
            cboTrangThai.DataSource = CTRANGTHAI.getList();
            cboTrangThai.ValueMember = "_value";
            cboTrangThai.DisplayMember = "_display";
            spSoNguoi.Value = 1;
            txtThanhTien.Text = _phongHienTai.DONGIA.ToString();
            loadDV();
            loadKH();
            string query3 = $"SELECT * FROM DATPHONG_CT WHERE IDPHONG = {_idPhong} ORDER BY IDDPCT DESC";
            DataTable dt3 = new DataTable();
            dt3 = provider.ExecuteQuery(query3);
            foreach (DataRow row3 in dt3.Rows)
            {
                if (!_them)
                {
                    _idDP = int.Parse(row3["IDDP"].ToString());
                    string query4 = $"SELECT * FROM DATPHONG WHERE IDDP = {_idDP}";
                    DataTable dt4 = new DataTable();
                    dt4 = provider.ExecuteQuery(query4);
                    foreach (DataRow row4 in dt4.Rows)
                    {
                        cboKhachHang.SelectedValue = row4["IDKH"].ToString();
                        dtNgayDat.Value = (DateTime)row4["NGAYDATPHONG"];
                        dtNgayTra.Value = (DateTime)row4["NGAYTRAPHONG"];
                        spSoNguoi.Value = int.Parse(row4["SONGUOIO"].ToString());
                        cboTrangThai.Text = row4["TRANGTHAI"].ToString();
                        txtGhiChu.Text = row4["GHICHU"].ToString();
                        int tien = int.Parse(row4["SOTIEN"].ToString());
                        txtThanhTien.Text = tien.ToString("N0");
                    }
                    loadSPDV();
                    break;
                }    
            }
            if (_thanhtoan)
            {
                visible(false);
                setEnable(false);
            }
        }
        void loadSPDV()
        {
            lstDPDV = new List<OBJ_DPDV>();
            string query = "SELECT * FROM dbo.DATPHONG_DICHVU WHERE IDDP = " + _idDP;
            DataProvider provider = new DataProvider();
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                OBJ_DPDV dv = new OBJ_DPDV();
                dv.IDDPDV = int.Parse(row["IDDPDV"].ToString());
                dv.IDDP = int.Parse(row["IDDP"].ToString());
                dv.IDDV = int.Parse(row["IDDV"].ToString());
                string query3 = "SELECT * FROM DICHVU WHERE IDDV = " + dv.IDDV;
                DataTable dt3 = new DataTable();
                dt3 = provider.ExecuteQuery(query3);
                foreach (DataRow row3 in dt3.Rows)
                {
                    dv.TENDV = row3["TENDV"].ToString();
                }
                dv.IDPHONG = int.Parse(row["IDPHONG"].ToString());
                string query2 = "SELECT * FROM PHONG WHERE IDPHONG = " + dv.IDPHONG;
                DataTable dt2 = new DataTable();
                dt2 = provider.ExecuteQuery(query2);
                foreach (DataRow row2 in dt2.Rows)
                {
                    dv.TENPHONG = row2["TENPHONG"].ToString();
                }
                dv.DONGIA = float.Parse(row["DONGIA"].ToString());
                dv.SOLUONG = int.Parse(row["SOLUONG"].ToString());
                dv.THANHTIEN = float.Parse(row["THANHTIEN"].ToString());
                lstDPDV.Add(dv);
            }
            gcSPDV.DataSource = lstDPDV;
            gvSPDV.OptionsBehavior.Editable = false;
        }
        void loadDV()
        {
            string query = "SELECT * FROM dbo.DICHVU";
            DataProvider provider = new DataProvider();
            gcSanPham.DataSource = provider.ExecuteQuery(query);
            gvSanPham.OptionsBehavior.Editable = false;
        }
        public void loadKH()
        {
            string query = "SELECT * FROM dbo.KHACHHANG";
            DataProvider provider = new DataProvider();
            cboKhachHang.DataSource = provider.ExecuteQuery(query);
            cboKhachHang.DisplayMember = "HOTEN";
            cboKhachHang.ValueMember = "IDKH";
        }
        public void getKH(int idKH)
        {
            string query = "SELECT * FROM dbo.KHACHHANG WHERE IDKH = " + idKH;
            DataProvider provider = new DataProvider();
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                cboKhachHang.SelectedValue = row["IDKH"].ToString();
                cboKhachHang.Text = row["HOTEN"].ToString();
            }    
        }    
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmKH frm = new frmKH();
            frm.kh_dp = "datphongdon";
            frm.ShowDialog();
        }

        private void gvSanPham_DoubleClick(object sender, EventArgs e)
        {
            if (_idPhong == 0)
            {
                MessageBox.Show("Vui lòng chọn phòng?", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (gvSanPham.GetFocusedRowCellValue("IDDV") != null)
            {
                OBJ_DPDV dv = new OBJ_DPDV();
                dv.IDDV = int.Parse(gvSanPham.GetFocusedRowCellValue("IDDV").ToString());
                dv.TENDV = gvSanPham.GetFocusedRowCellValue("TENDV").ToString();
                dv.IDPHONG = _idPhong;
                dv.TENPHONG = _phongHienTai.TENPHONG;
                dv.DONGIA = float.Parse(gvSanPham.GetFocusedRowCellValue("DONGIA").ToString());
                dv.SOLUONG = 1;
                dv.THANHTIEN = dv.DONGIA * dv.SOLUONG;
                foreach (var item in lstDPDV)
                {
                    if (item.IDDV == dv.IDDV && item.IDPHONG == dv.IDPHONG)
                    {
                        item.SOLUONG = item.SOLUONG + 1;
                        item.THANHTIEN = item.SOLUONG * item.DONGIA;
                        loadDPDV();
                        txtThanhTien.Text = (double.Parse(gvSPDV.Columns["THANHTIEN"].SummaryItem.SummaryValue.ToString()) + _phongHienTai.DONGIA).ToString("N0");
                        return;
                    }
                }
                lstDPDV.Add(dv);
            }
            loadDPDV();
            txtThanhTien.Text = (double.Parse(gvSPDV.Columns["THANHTIEN"].SummaryItem.SummaryValue.ToString()) + _phongHienTai.DONGIA).ToString("N0");
        }
        void loadDPDV()
        {
            List<OBJ_DPDV> lstDP = new List<OBJ_DPDV>();
            foreach (var item in lstDPDV)
            {
                lstDP.Add(item);
            }
            gcSPDV.DataSource = lstDP;
        }

        private void gvSPDV_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "SOLUONG")
            {
                int sl = int.Parse(e.Value.ToString());
                if (sl != 0)
                {
                    double gia = double.Parse(gvSPDV.GetRowCellValue(gvSPDV.FocusedRowHandle, "DONGIA").ToString());
                    gvSPDV.SetRowCellValue(gvSPDV.FocusedRowHandle, "THANHTIEN", sl * gia);
                }
                else
                {
                    gvSPDV.SetRowCellValue(gvSPDV.FocusedRowHandle, "THANHTIEN", 0);
                }
            }
            gvSPDV.UpdateTotalSummary();
            txtThanhTien.Text = (double.Parse(gvSPDV.Columns["THANHTIEN"].SummaryItem.SummaryValue.ToString()) + _phongHienTai.DONGIA).ToString("N0");
        }

        private void gvSPDV_HiddenEditor(object sender, EventArgs e)
        {
            gvSPDV.UpdateCurrentRow();
        }

        private void setEnable(bool t)
        {
            cboKhachHang.Enabled = t;
            dtNgayTra.Enabled = t;
            dtNgayDat.Enabled = t;
            spSoNguoi.Enabled = t;
            cboTrangThai.Enabled = t;
            txtGhiChu.Enabled = t;
            cboKhachHang.Enabled = t;
            gcSanPham.Enabled = t;
            gcSPDV.Enabled = t;
            btnAddNew.Enabled = t;
        }
        private void visible(bool t)
        {
            btnLuu.Visible = t;
        }
    }
}