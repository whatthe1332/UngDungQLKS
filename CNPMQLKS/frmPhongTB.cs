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
    public partial class frmPhongTB : DevExpress.XtraEditors.XtraForm
    {
        public frmPhongTB()
        {
            InitializeComponent();
        }
        bool _them;
        string _idP;
        string _idTB;
        private void frmPhongTB_Load(object sender, EventArgs e)
        {
            loadData();
            showHideControl(true);
            _enebled(false);
            loadComboboxP();
            loadComboboxTB();
        }
        void loadComboboxP()
        {
            string query = "SELECT TENPHONG FROM dbo.PHONG";
            DataProvider provider = new DataProvider();
            cbMaP.DisplayMember = "TENPHONG";
            cbMaP.DataSource = provider.ExecuteQuery(query);
        }
        void loadComboboxTB()
        {
            string query = "SELECT TENTB FROM dbo.THIETBI";
            DataProvider provider = new DataProvider();
            cbMaTB.DisplayMember = "TENTB";
            cbMaTB.DataSource = provider.ExecuteQuery(query);
        }
        void loadData()
        {
            string query = "SELECT TENPHONG, TENTB, SOLUONG FROM dbo.PHONG_THIETBI LEFT JOIN PHONG ON PHONG_THIETBI.IDPHONG = PHONG.IDPHONG LEFT JOIN THIETBI ON PHONG_THIETBI.IDTB = THIETBI.IDTB ";
            DataProvider provider = new DataProvider();
            gcDanhSach.DataSource = provider.ExecuteQuery(query);
            gvDanhSach.OptionsBehavior.Editable = false;
        }
        void _enebled(bool t)
        {
            cbMaP.Enabled = t;
            cbMaTB.Enabled = t;
            txtSoLuong.Enabled = t;
        }
        void _reset()
        {
            cbMaP.Text = "";
            cbMaTB.Text = "";
            txtSoLuong.Text = "";
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
                string tenphong = gvDanhSach.GetFocusedRowCellValue("TENPHONG").ToString();
                string tentb = gvDanhSach.GetFocusedRowCellValue("TENTB").ToString();

                string queryA = $"SELECT * FROM dbo.PHONG where TENPHONG = N'{tenphong}'";
                DataProvider providerA = new DataProvider();
                DataTable dt = new DataTable();
                dt = providerA.ExecuteQuery(queryA);
                foreach (DataRow row in dt.Rows)
                    _idP = row["IDPHONG"].ToString();
                string queryB = $"SELECT * FROM dbo.THIETBI where TENTB = N'{tentb}'";
                DataProvider providerB = new DataProvider();
                DataTable dt2 = new DataTable();
                dt2 = providerB.ExecuteQuery(queryB);
                foreach (DataRow row in dt2.Rows)
                    _idTB = row["IDTB"].ToString();


                cbMaP.Text = gvDanhSach.GetFocusedRowCellValue("TENPHONG").ToString();
                cbMaTB.Text = gvDanhSach.GetFocusedRowCellValue("TENTB").ToString();
                txtSoLuong.Text = gvDanhSach.GetFocusedRowCellValue("SOLUONG").ToString();
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
                    string query = "Delete from PHONG_THIETBI where IDPHONG =" + _idP + " and IDTB =" + _idTB;
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
            string tenphong = cbMaP.Text;
            string tentb = cbMaTB.Text;
            string idphong = "",idtb = "";

            string queryA = $"SELECT * FROM dbo.PHONG where TENPHONG = N'{tenphong}'";
            DataProvider providerA = new DataProvider();
            DataTable dt = new DataTable();
            dt = providerA.ExecuteQuery(queryA);
            foreach (DataRow row in dt.Rows)
                idphong = row["IDPHONG"].ToString();
            string queryB = $"SELECT * FROM dbo.THIETBI where TENTB = N'{tentb}'";
            DataProvider providerB = new DataProvider();
            DataTable dt2 = new DataTable();
            dt2 = providerB.ExecuteQuery(queryB);
            foreach (DataRow row in dt2.Rows)
                idtb = row["IDTB"].ToString();

            string soluong = txtSoLuong.Text;
            try
            {
                if (_them)
                {
                    string query = $"Insert into PHONG_THIETBI values ({idphong}, {idtb}, {soluong})";
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                }
                else
                {
                    string query = $"UPDATE PHONG_THIETBI set IDPHONG = {idphong}, IDTB = {idtb}, SOLUONG = {soluong} where IDPHONG = {_idP} and IDTB =" + _idTB;
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                }
                _them = false;
                loadData();
                showHideControl(true);
                _enebled(false);
            }
            catch (Exception)
            {
                MessageBox.Show("Bàn hoặc thiết bị không tồn tại");
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
    }
}