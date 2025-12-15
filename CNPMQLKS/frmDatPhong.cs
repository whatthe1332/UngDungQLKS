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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;

namespace CNPMQLKS
{
    public partial class frmDatPhong : DevExpress.XtraEditors.XtraForm
    {
        public frmDatPhong()
        {
            InitializeComponent();
            string query = "SELECT A.IDPHONG, A.TENPHONG, C.DONGIA, A.IDTANG, B.TENTANG from PHONG A, TANG B, LOAIPHONG C WHERE A.IDTANG = B.IDTANG AND A.TINHTRANG = 0 AND A.IDLOAIPHONG = C.IDLOAIPHONG";
            DataProvider provider = new DataProvider();
            gcPhong.DataSource = provider.ExecuteQuery(query);
            gcDatPhong.DataSource = provider.ExecuteQuery(query).Clone();
        }
        frmMain objMain = (frmMain)Application.OpenForms["frmMain"];
        int _rowDatPhong = 0;
        bool _them;
        int _idPhong = 0;
        int _idDP = 0;
        string _tenPhong;
        List<OBJ_DPDV> lstDPDV;
        GridHitInfo downHitInfor = null;
        private void btnThem_Click(object sender, EventArgs e)
        {
            _them = true;
            showHideControl(false);
            _enebled(true);
            _reset();
            addReset();
            tabDanhSach.SelectedTabPage = PageChiTiet;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            _them = false;
            showHideControl(false);
            _enebled(true);
            tabDanhSach.SelectedTabPage = PageChiTiet;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string query = "Delete from DATPHONG where IDDP =" + _idDP;
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                    string query4 = "SELECT * FROM dbo.DATPHONG_CT WHERE IDDP = " + _idDP;
                    DataTable dt = new DataTable();
                    dt = provider.ExecuteQuery(query4);
                    foreach (DataRow row in dt.Rows)
                    {
                        string query5 = $"UPDATE PHONG SET TINHTRANG = 0 where IDPHONG = {int.Parse(row["IDPHONG"].ToString())}";
                        provider.ExecuteQuery(query5);
                    }
                    objMain.showRoom();
                    string query2 = "Delete from DATPHONG_CT where IDDP =" + _idDP;
                    provider.ExecuteQuery(query2);
                    string query3 = "Delete from DATPHONG_DICHVU where IDDP =" + _idDP;
                    provider.ExecuteQuery(query3);
                    loadDanhSach();
                }
                catch (Exception)
                {
                    MessageBox.Show("Chưa chọn khách hàng đễ xóa");
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            saveData();
            objMain.showRoom();
            _them = false;
            showHideControl(true);
            _enebled(false);
            loadDanhSach();
            tabDanhSach.SelectedTabPage = PageDanhSach;
        }
        void saveData()
        {
            if (_them)
            {
                try
                {
                    int idnv = objMain._idnv;
                    int theodoan = 1;
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
                    for (int i = 0; i < gvDatPhong.RowCount; i++)
                    {
                        string query2 = $"INSERT INTO DATPHONG_CT VALUES({IDDP},{int.Parse(gvDatPhong.GetRowCellValue(i, "IDPHONG").ToString())},{dtNgayTra.Value.Day - dtNgayDat.Value.Day},{int.Parse(gvDatPhong.GetRowCellValue(i, "DONGIA").ToString())},{(dtNgayTra.Value.Day - dtNgayDat.Value.Day) * int.Parse(gvDatPhong.GetRowCellValue(i, "DONGIA").ToString())})";
                        provider.ExecuteQuery(query2);
                        string query7 = "SELECT IDENT_CURRENT('DATPHONG_CT') as [IDDPCT]";
                        DataTable dt2 = new DataTable();
                        dt2 = provider.ExecuteQuery(query7);
                        string IDDPCT = "";
                        foreach (DataRow row in dt2.Rows)
                        {
                            IDDPCT = row["IDDPCT"].ToString();
                        }
                        string query3 = $"UPDATE PHONG SET TINHTRANG = 1 where IDPHONG = {int.Parse(gvDatPhong.GetRowCellValue(i, "IDPHONG").ToString())}";
                        provider.ExecuteQuery(query3);
                        if (gvSPDV.RowCount > 0)
                        {
                            for (int j = 0; j < gvSPDV.RowCount; j++)
                            {
                                if (int.Parse(gvDatPhong.GetRowCellValue(i, "IDPHONG").ToString()) == int.Parse(gvSPDV.GetRowCellValue(j, "IDPHONG").ToString()))
                                {
                                    string query4 = $"INSERT INTO DATPHONG_DICHVU VALUES ({IDDP},{IDDPCT},{int.Parse(gvSPDV.GetRowCellValue(j, "IDPHONG").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "IDDV").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "SOLUONG").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "DONGIA").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "SOLUONG").ToString()) * int.Parse(gvSPDV.GetRowCellValue(j, "DONGIA").ToString())})";
                                    provider.ExecuteQuery(query4);
                                }
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
                    for (int i = 0; i < gvDatPhong.RowCount; i++)
                    {
                        string query8 = $"INSERT INTO DATPHONG_CT VALUES({_idDP},{int.Parse(gvDatPhong.GetRowCellValue(i, "IDPHONG").ToString())},{dtNgayTra.Value.Day - dtNgayDat.Value.Day},{int.Parse(gvDatPhong.GetRowCellValue(i, "DONGIA").ToString())},{(dtNgayTra.Value.Day - dtNgayDat.Value.Day) * int.Parse(gvDatPhong.GetRowCellValue(i, "DONGIA").ToString())})";
                        provider.ExecuteQuery(query8);
                        string query7 = "SELECT IDENT_CURRENT('DATPHONG_CT') as [IDDPCT]";
                        DataTable dt2 = new DataTable();
                        dt2 = provider.ExecuteQuery(query7);
                        string IDDPCT = "";
                        foreach (DataRow row in dt2.Rows)
                        {
                            IDDPCT = row["IDDPCT"].ToString();
                        }
                        string query9 = $"UPDATE PHONG SET TINHTRANG = 1 where IDPHONG = {int.Parse(gvDatPhong.GetRowCellValue(i, "IDPHONG").ToString())}";
                        provider.ExecuteQuery(query9);
                        if (gvSPDV.RowCount > 0)
                        {
                            for (int j = 0; j < gvSPDV.RowCount; j++)
                            {
                                if (int.Parse(gvDatPhong.GetRowCellValue(i, "IDPHONG").ToString()) == int.Parse(gvSPDV.GetRowCellValue(j, "IDPHONG").ToString()))
                                {
                                    string query4 = $"INSERT INTO DATPHONG_DICHVU VALUES ({_idDP},{IDDPCT},{int.Parse(gvSPDV.GetRowCellValue(j, "IDPHONG").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "IDDV").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "SOLUONG").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "DONGIA").ToString())},{int.Parse(gvSPDV.GetRowCellValue(j, "SOLUONG").ToString()) * int.Parse(gvSPDV.GetRowCellValue(j, "DONGIA").ToString())})";
                                    provider.ExecuteQuery(query4);
                                }
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
        void addReset()
        {
            string query = "SELECT A.IDPHONG, A.TENPHONG, C.DONGIA, A.IDTANG, B.TENTANG from PHONG A, TANG B, LOAIPHONG C WHERE A.IDTANG = B.IDTANG AND A.TINHTRANG = 0 AND A.IDLOAIPHONG = C.IDLOAIPHONG";
            DataProvider provider = new DataProvider();
            gcPhong.DataSource = provider.ExecuteQuery(query);
            gcDatPhong.DataSource = provider.ExecuteQuery(query).Clone();
            gvPhong.ExpandAllGroups();
            string query2 = "SELECT * FROM DATPHONG_DICHVU WHERE IDDP = 0";
            gcSPDV.DataSource = provider.ExecuteQuery(query2);
            txtThanhTien.Text = "0";
        }
        private void btnBoQua_Click(object sender, EventArgs e)
        {
            _them = false;
            showHideControl(true);
            _enebled(false);
            tabDanhSach.SelectedTabPage = PageDanhSach;
        }
        private void btnIn_Click(object sender, EventArgs e)
        {
            string query3 = $"UPDATE DATPHONG SET TRANGTHAI = N'Đã tính tiền' WHERE IDDP = {_idDP}";
            DataProvider provider = new DataProvider();
            provider.ExecuteQuery(query3);
            string query = "SELECT A.IDPHONG, A.TENPHONG, C.DONGIA, A.IDTANG, B.TENTANG from PHONG A, TANG B, LOAIPHONG C, DATPHONG_CT D WHERE A.IDTANG = B.IDTANG AND A.IDLOAIPHONG = C.IDLOAIPHONG AND A.IDPHONG = D.IDPHONG AND D.IDDP = " + _idDP;
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                string query2 = $"UPDATE PHONG SET TINHTRANG = 0 where IDPHONG = {int.Parse(row["IDPHONG"].ToString())}";
                provider.ExecuteQuery(query2);
            }
            loadDanhSach();
            objMain.showRoom();
            //DataProvider provider = new DataProvider();
            //provider.XuatReport(_idDP.ToString(), "PHIEU_DATPHONG", "Phiếu đặt phòng chi tiết");
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void frmDatPhong_Load(object sender, EventArgs e)
        {
            lstDPDV = new List<OBJ_DPDV>();
            dtTuNgay.Value = DataProvider.getFirstDayInMonth(DateTime.Now.Year, DateTime.Now.Month);
            dtDenNgay.Value = DateTime.Now;
            loadKH();
            loadDanhSach();
            loadDV();
            cboTrangThai.DataSource = CTRANGTHAI.getList();
            cboTrangThai.ValueMember = "_value";
            cboTrangThai.DisplayMember = "_display";
            showHideControl(true);
            _enebled(false);
            gvPhong.ExpandAllGroups();
            tabDanhSach.SelectedTabPage = PageDanhSach;
        }
        void loadDV()
        {
            string query = "SELECT * FROM dbo.DICHVU";
            DataProvider provider = new DataProvider();
            gcSanPham.DataSource = provider.ExecuteQuery(query);
            gvSanPham.OptionsBehavior.Editable = false;
        }
        void loadDanhSach()
        {
            string query = $"SELECT * FROM dbo.DATPHONG WHERE CONVERT(datetime,'{dtTuNgay.Value.ToString("dd/MM/yyyy")}',103) <= NGAYDATPHONG AND CONVERT(datetime,'{dtDenNgay.Value.ToString("dd/MM/yyyy")}',103) >= NGAYDATPHONG AND TRANGTHAI = N'Chưa tính tiền' AND THEODOAN = 1";
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
                string query2 = "SELECT * FROM dbo.KHACHHANG WHERE IDKH = "+ dp.IDKH;
                DataTable dt2 = new DataTable();
                dt2 = provider.ExecuteQuery(query2);
                foreach (DataRow row2 in dt2.Rows)
                {
                    dp.HOTEN = row2["HOTEN"].ToString();
                }
                dp.NGAYDATPHONG = (DateTime) row["NGAYDATPHONG"];
                dp.NGAYTRAPHONG = (DateTime) row["NGAYTRAPHONG"];
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
                dp.NGAYTAO = (DateTime) row["NGAYTAO"];
                if (row["NGAYCHINHSUA"].ToString() != "")
                    dp.NGAYCHINHSUA = (DateTime)row["NGAYCHINHSUA"];
                lstDP.Add(dp);
            }
            gcDanhSach.DataSource = lstDP;
            gvDanhSach.OptionsBehavior.Editable = false;
        }    
        public void loadKH()
        {
            string query = "SELECT * FROM dbo.KHACHHANG";
            DataProvider provider = new DataProvider();
            cboKhachHang.DataSource = provider.ExecuteQuery(query);
            cboKhachHang.DisplayMember = "HOTEN";
            cboKhachHang.ValueMember = "IDKH";
        }

        void _enebled(bool t)
        {
            cboKhachHang.Enabled = t;
            btnAddNew.Enabled = t;
            dtNgayDat.Enabled = t;
            dtNgayTra.Enabled = t;
            cboTrangThai.Enabled = t;
            chkDoan.Enabled = t;
            spSoNguoi.Enabled = t;
            txtGhiChu.Enabled = t;
            gcPhong.Enabled = t;
            gcSanPham.Enabled = t;
            gcDatPhong.Enabled = t;
            gcSPDV.Enabled = t;
            txtThanhTien.Enabled = t;
        }
        void _reset()
        {
            dtNgayDat.Value = DateTime.Now;
            dtNgayTra.Value = DateTime.Now.AddDays(1);
            spSoNguoi.Value = 1;
            chkDoan.Checked = true;
            cboTrangThai.SelectedValue = false;
            txtGhiChu.Text = "";
            lblThanhToan.Text = "0";
        }
        void showHideControl(bool t)
        {
            btnThem.Visible = t;
            btnSua.Visible = t;
            btnXoa.Visible = t;
            btnThoat.Visible = t;
            btnLuu.Visible = !t;
            btnBoQua.Visible = !t;
            btnIn.Visible = t;
        }

        private void gvDatPhong_MouseDown(object sender, MouseEventArgs e)
        {
            if (gvDatPhong.GetFocusedRowCellValue("TENPHONG") != null)
            {
                _idPhong = int.Parse(gvDatPhong.GetFocusedRowCellValue("IDPHONG").ToString());
                _tenPhong = gvDatPhong.GetFocusedRowCellValue("TENPHONG").ToString();
            }
            GridView view = sender as GridView;
            downHitInfor = null;
            GridHitInfo hitInfor = view.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None) return;
            if (e.Button == MouseButtons.Left && hitInfor.RowHandle >= 0)
                downHitInfor = hitInfor;
        }

        private void gvDatPhong_MouseMove(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Button == MouseButtons.Left && downHitInfor != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(downHitInfor.HitPoint.X - dragSize.Width / 2, downHitInfor.HitPoint.Y - dragSize.Height / 2), dragSize);
                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    DataRow row = view.GetDataRow(downHitInfor.RowHandle);
                    view.GridControl.DoDragDrop(row, DragDropEffects.Move);
                    downHitInfor = null;
                    DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                }
            }
        }

        private void gvPhong_MouseMove(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Button == MouseButtons.Left && downHitInfor != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(downHitInfor.HitPoint.X - dragSize.Width / 2, downHitInfor.HitPoint.Y - dragSize.Height / 2), dragSize);
                if (!dragRect.Contains(new Point(e.X,e.Y)))
                {
                    DataRow row = view.GetDataRow(downHitInfor.RowHandle);
                    view.GridControl.DoDragDrop(row, DragDropEffects.Move);
                    downHitInfor = null;
                    DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                }
            }
        }

        private void gvPhong_MouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            downHitInfor = null;
            GridHitInfo hitInfor = view.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None) return;
            if (e.Button == MouseButtons.Left && hitInfor.RowHandle >= 0)
                downHitInfor = hitInfor;
        }

        private void gcPhong_DragDrop(object sender, DragEventArgs e)
        {
            GridControl grid = sender as GridControl;
            DataTable table = grid.DataSource as DataTable;
            DataRow row = e.Data.GetData(typeof(DataRow)) as DataRow;
            if (row != null && table != null && row.Table != table)
            {
                table.ImportRow(row);
                row.Delete();
            }
            gvPhong.ExpandAllGroups();
        }

        private void gcPhong_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DataRow)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }
        bool cal(Int32 _Width, GridView _View)
        {
            _View.IndicatorWidth = _View.IndicatorWidth < _Width ? _Width : _View.IndicatorWidth;
            return true;
        }

        private void gvPhong_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!gvPhong.IsGroupRow(e.RowHandle))
            {
                if (e.Info.IsRowIndicator)
                {
                    if (e.RowHandle < 0)
                    {
                        e.Info.ImageIndex = 0;
                        e.Info.DisplayText = string.Empty;
                    }
                    else
                    {
                        e.Info.ImageIndex = -1;
                        e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    }
                    SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                    Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                    BeginInvoke(new MethodInvoker(delegate { cal(_Width, gvPhong); }));
                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));
                SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                BeginInvoke(new MethodInvoker(delegate { cal(_Width, gvPhong); }));
            }
        }

        private void gvPhong_CustomDrawGroupRow(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridGroupRowInfo info = e.Info as GridGroupRowInfo;
            string caption = info.Column.Caption;
            if (info.Column.Caption == string.Empty)
                caption = info.Column.ToString();
            info.GroupText = string.Format("{0}: {1} ({2} phòng trống)", caption, info.GroupValueText, view.GetChildRowCount(e.RowHandle));
        }

        private void gcSanPham_DoubleClick(object sender, EventArgs e)
        {
            
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
                    gvSPDV.SetRowCellValue(gvSPDV.FocusedRowHandle,"THANHTIEN", sl * gia);
                }
                else
                {
                    gvSPDV.SetRowCellValue(gvSPDV.FocusedRowHandle, "THANHTIEN", 0);
                }
            }
            gvSPDV.UpdateTotalSummary();
            txtThanhTien.Text = (double.Parse(gvSPDV.Columns["THANHTIEN"].SummaryItem.SummaryValue.ToString()) + double.Parse(gvDatPhong.Columns["DONGIA"].SummaryItem.SummaryValue.ToString())).ToString("N0");
        }

        private void gvDatPhong_RowCountChanged(object sender, EventArgs e)
        {
            if (gvDatPhong.RowCount > _rowDatPhong)
                _rowDatPhong = gvDatPhong.RowCount;
            if (gvDatPhong.RowCount < _rowDatPhong && _them == false)
            {
                string query = $"UPDATE PHONG SET TINHTRANG = 0 where IDPHONG = {_idPhong}";
                DataProvider provider = new DataProvider();
                provider.ExecuteQuery(query);
                string query2 = $"DELETE FROM DATPHONG_DICHVU WHERE IDDP = {_idDP} AND IDPHONG = {_idPhong}";
                provider.ExecuteQuery(query2);
                string query3 = $"DELETE FROM DATPHONG_CT WHERE IDDP = {_idDP} AND IDPHONG = {_idPhong}";
                provider.ExecuteQuery(query3);
                objMain.showRoom();
            }
            if (gvDatPhong.RowCount < _rowDatPhong)
            {
                List<OBJ_DPDV> listtmp = new List<OBJ_DPDV>();
                foreach (var item in lstDPDV)
                {
                    if (item.IDPHONG == _idPhong)
                    {
                        var tmp = item;
                        listtmp.Add(item);
                    }
                }
                foreach (var item in listtmp)
                {
                    lstDPDV.Remove(item);
                }
                loadDPDV();
            }
            _rowDatPhong = gvDatPhong.RowCount;
            double t = 0;
            if (gvSPDV.Columns["THANHTIEN"].SummaryItem.SummaryValue == null)
                t = 0;
            else
                t = double.Parse(gvSPDV.Columns["THANHTIEN"].SummaryItem.SummaryValue.ToString());
            txtThanhTien.Text = (double.Parse(gvDatPhong.Columns["DONGIA"].SummaryItem.SummaryValue.ToString()) + t).ToString("N0");
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmKH frm = new frmKH();
            frm.ShowDialog();
        }
        public void setKhachHang(int idkh)
        {
            string query = "SELECT * FROM KHACHHANG WHERE IDKH = " + idkh;
            DataProvider provider = new DataProvider();
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                cboKhachHang.SelectedValue = row["IDKH"].ToString();
                cboKhachHang.Text = row["HOTEN"].ToString();
            }
        }

        private void gvDanhSach_Click(object sender, EventArgs e)
        {
            if (gvDanhSach.RowCount > 0)
            {
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnIn.Enabled = true;
                _idDP = int.Parse(gvDanhSach.GetFocusedRowCellValue("IDDP").ToString());
                string query = "SELECT * FROM DATPHONG WHERE IDDP = " + _idDP;
                DataProvider provider = new DataProvider();
                DataTable dt = new DataTable();
                dt = provider.ExecuteQuery(query);
                foreach (DataRow row in dt.Rows)
                {
                    cboKhachHang.SelectedValue = row["IDKH"].ToString();
                    dtNgayDat.Value = (DateTime) row["NGAYDATPHONG"];
                    dtNgayTra.Value = (DateTime)row["NGAYTRAPHONG"];
                    spSoNguoi.Value = int.Parse(row["SONGUOIO"].ToString());
                    cboTrangThai.Text = row["TRANGTHAI"].ToString();
                    txtGhiChu.Text = row["GHICHU"].ToString();
                    int tien = int.Parse(row["SOTIEN"].ToString());
                    txtThanhTien.Text = tien.ToString("N0");
                    loadDP();
                }
                loadDPSP();
            }
        }
        void loadDP()
        {
            _rowDatPhong = 0;
            string query = "SELECT A.IDPHONG, A.TENPHONG, C.DONGIA, A.IDTANG, B.TENTANG from PHONG A, TANG B, LOAIPHONG C, DATPHONG_CT D WHERE A.IDTANG = B.IDTANG AND A.IDLOAIPHONG = C.IDLOAIPHONG AND A.IDPHONG = D.IDPHONG AND D.IDDP = " + _idDP;
            DataProvider provider = new DataProvider();
            gcDatPhong.DataSource = provider.ExecuteQuery(query);
            _rowDatPhong = gvDatPhong.RowCount;
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

        private void gvDanhSach_DoubleClick(object sender, EventArgs e)
        {
            if (gvDanhSach.RowCount > 0)
            {
                _them = false;
                showHideControl(false);
                _enebled(true);
                _idDP = int.Parse(gvDanhSach.GetFocusedRowCellValue("IDDP").ToString());
                string query = "SELECT * FROM DATPHONG WHERE IDDP = " + _idDP;
                DataProvider provider = new DataProvider();
                DataTable dt = new DataTable();
                dt = provider.ExecuteQuery(query);
                foreach (DataRow row in dt.Rows)
                {
                    cboKhachHang.SelectedValue = row["IDKH"].ToString();
                    dtNgayDat.Value = (DateTime)row["NGAYDATPHONG"];
                    dtNgayTra.Value = (DateTime)row["NGAYTRAPHONG"];
                    spSoNguoi.Value = int.Parse(row["SONGUOIO"].ToString());
                    cboTrangThai.Text = row["TRANGTHAI"].ToString();
                    txtGhiChu.Text = row["GHICHU"].ToString();
                    int tien = int.Parse(row["SOTIEN"].ToString());
                    txtThanhTien.Text = tien.ToString("N0");
                    loadDP();
                    loadDPSP();
                }
                tabDanhSach.SelectedTabPage = PageChiTiet;
            }
        }

        private void gvDanhSach_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!gvDanhSach.IsGroupRow(e.RowHandle))
            {
                if (e.Info.IsRowIndicator)
                {
                    if (e.RowHandle < 0)
                    {
                        e.Info.ImageIndex = 0;
                        e.Info.DisplayText = string.Empty;
                    }
                    else
                    {
                        e.Info.ImageIndex = -1;
                        e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    }
                    SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                    Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                    BeginInvoke(new MethodInvoker(delegate { cal(_Width, gvDanhSach); }));
                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));
                SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                BeginInvoke(new MethodInvoker(delegate { cal(_Width, gvDanhSach); }));
            }
        }

        private void gvSPDV_DoubleClick(object sender, EventArgs e)
        {
            if (gvSPDV.GetFocusedRowCellValue("IDDV") != null)
            {
                foreach (var item in lstDPDV)
                {
                    if (item.IDDV == int.Parse(gvSPDV.GetFocusedRowCellValue("IDDV").ToString()))
                    {
                        lstDPDV.Remove(item);
                        loadDPDV();
                        break;
                    }
                }
            }
        }

        private void gvDatPhong_Click(object sender, EventArgs e)
        {
            _idPhong = int.Parse(gvDatPhong.GetFocusedRowCellValue("IDPHONG").ToString());
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
                dv.TENPHONG = _tenPhong;
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
                        txtThanhTien.Text = (double.Parse(gvSPDV.Columns["THANHTIEN"].SummaryItem.SummaryValue.ToString()) + double.Parse(gvDatPhong.Columns["DONGIA"].SummaryItem.SummaryValue.ToString())).ToString("N0");
                        return;
                    }
                }
                lstDPDV.Add(dv);
            }
            loadDPDV();
            txtThanhTien.Text = (double.Parse(gvSPDV.Columns["THANHTIEN"].SummaryItem.SummaryValue.ToString()) + double.Parse(gvDatPhong.Columns["DONGIA"].SummaryItem.SummaryValue.ToString())).ToString("N0");
        }
    }
}