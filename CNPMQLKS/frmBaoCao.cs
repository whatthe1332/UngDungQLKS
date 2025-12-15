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
    public partial class frmBaoCao : DevExpress.XtraEditors.XtraForm
    {
        public frmBaoCao()
        {
            InitializeComponent();
        }
        int _idDP = 0;
        List<OBJ_DPDV> lstDPDV;
        void loadDanhSach()
        {
            string query = $"SELECT * FROM dbo.DATPHONG WHERE CONVERT(datetime,'{dtTuNgay.Value.ToString("dd/MM/yyyy")}',103) <= NGAYDATPHONG AND CONVERT(datetime,'{dtDenNgay.Value.ToString("dd/MM/yyyy")}',103) >= NGAYDATPHONG";
            DataProvider provider = new DataProvider();
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            List<OBJ_DATPHONG> lstDP = new List<OBJ_DATPHONG>();
            OBJ_DATPHONG dp;
            foreach (DataRow row in dt.Rows)
            {
                dp = new OBJ_DATPHONG();
                dp.IDDP = int.Parse(row["IDDP"].ToString());
                dp.IDKH = int.Parse(row["IDKH"].ToString());
                string query2 = "SELECT * FROM dbo.KHACHHANG WHERE IDKH = " + dp.IDKH;
                DataTable dt2 = new DataTable();
                dt2 = provider.ExecuteQuery(query2);
                foreach (DataRow row2 in dt2.Rows)
                {
                    dp.HOTEN = row2["HOTEN"].ToString();
                }
                dp.NGAYDATPHONG = (DateTime)row["NGAYDATPHONG"];
                dp.NGAYTRAPHONG = (DateTime)row["NGAYTRAPHONG"];
                dp.SOTIEN = double.Parse(row["IDDP"].ToString());
                dp.SONGUOIO = int.Parse(row["SONGUOIO"].ToString());
                dp.IDNV = int.Parse(row["IDNV"].ToString());
                string query3 = "SELECT * FROM dbo.NHANVIEN WHERE IDNV = " + dp.IDNV;
                DataTable dt3 = new DataTable();
                dt3 = provider.ExecuteQuery(query3);
                foreach (DataRow row3 in dt3.Rows)
                {
                    dp.TENNV = row3["TENNV"].ToString();
                    if (dp.TENNV == "" || dp.TENNV == null)
                        dp.TENNV = "Nhân viên đã nghỉ";
                }
                dp.TRANGTHAI = row["TRANGTHAI"].ToString();
                dp.THEODOAN = bool.Parse(row["THEODOAN"].ToString());
                dp.DISABLED = bool.Parse(row["DISABLED"].ToString());
                dp.GHICHU = row["GHICHU"].ToString();
                dp.NGAYTAO = (DateTime)row["NGAYTAO"];
                if (row["NGAYCHINHSUA"].ToString() != "")
                    dp.NGAYCHINHSUA = (DateTime)row["NGAYCHINHSUA"];
                lstDP.Add(dp);
            }
            gcDanhSach.DataSource = lstDP;
            gvDanhSach.OptionsBehavior.Editable = false;
        }

        private void frmBaoCao_Load(object sender, EventArgs e)
        {
            dtTuNgay.Value = DataProvider.getFirstDayInMonth(DateTime.Now.Year, DateTime.Now.Month);
            dtDenNgay.Value = DateTime.Now;
            lstDPDV = new List<OBJ_DPDV>();
            loadDanhSach();
        }

        private void gvDanhSach_Click(object sender, EventArgs e)
        {
            if (gvDanhSach.RowCount > 0)
            {
                btnIn.Enabled = true;
                _idDP = int.Parse(gvDanhSach.GetFocusedRowCellValue("IDDP").ToString());
                string query = "SELECT * FROM DATPHONG WHERE IDDP = " + _idDP;
                DataProvider provider = new DataProvider();
                DataTable dt = new DataTable();
                dt = provider.ExecuteQuery(query);
                foreach (DataRow row in dt.Rows)
                {
                    int tien = int.Parse(row["SOTIEN"].ToString());
                    txtThanhTien.Text = tien.ToString("N0");
                    loadDP();
                }
                loadDPSP();
            }
        }
        void loadDP()
        {
            string query = "SELECT A.IDPHONG, A.TENPHONG, C.DONGIA, A.IDTANG, B.TENTANG from PHONG A, TANG B, LOAIPHONG C, DATPHONG_CT D WHERE A.IDTANG = B.IDTANG AND A.IDLOAIPHONG = C.IDLOAIPHONG AND A.IDPHONG = D.IDPHONG AND D.IDDP = " + _idDP;
            DataProvider provider = new DataProvider();
            gcDatPhong.DataSource = provider.ExecuteQuery(query);
        }
        void loadDPSP()
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

        private void gvDanhSach_DoubleClick(object sender, EventArgs e)
        {
            if (gvDanhSach.RowCount > 0)
            {
                _idDP = int.Parse(gvDanhSach.GetFocusedRowCellValue("IDDP").ToString());
                string query = "SELECT * FROM DATPHONG WHERE IDDP = " + _idDP;
                DataProvider provider = new DataProvider();
                DataTable dt = new DataTable();
                dt = provider.ExecuteQuery(query);
                foreach (DataRow row in dt.Rows)
                {
                    int tien = int.Parse(row["SOTIEN"].ToString());
                    txtThanhTien.Text = tien.ToString("N0");
                    loadDP();
                    loadDPSP();
                }
                tabDanhSach.SelectedTabPage = PageChiTiet;
            }
        }

        private void dtTuNgay_ValueChanged(object sender, EventArgs e)
        {
            if (dtTuNgay.Value > dtDenNgay.Value)
            {
                MessageBox.Show("Ngày không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                loadDanhSach();
        }

        private void dtDenNgay_ValueChanged(object sender, EventArgs e)
        {
            if (dtTuNgay.Value > dtDenNgay.Value)
            {
                MessageBox.Show("Ngày không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                loadDanhSach();
        }

        private void dtTuNgay_Leave(object sender, EventArgs e)
        {
            if (dtTuNgay.Value > dtDenNgay.Value)
            {
                MessageBox.Show("Ngày không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                loadDanhSach();
        }

        private void dtDenNgay_Leave(object sender, EventArgs e)
        {
            if (dtTuNgay.Value > dtDenNgay.Value)
            {
                MessageBox.Show("Ngày không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                loadDanhSach();
        }

        private void btnIn_Click(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}