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
    public partial class frmTang : DevExpress.XtraEditors.XtraForm
    {
        frmMain frmmain;
        public frmTang(frmMain frm)
        {
            InitializeComponent();
            frmmain = frm;
        }
        bool _them;
        string _idTang;
        private void gvDanhSach_Click(object sender, EventArgs e)
        {
            if (gvDanhSach.RowCount > 0)
            {
                _idTang = gvDanhSach.GetFocusedRowCellValue("IDTANG").ToString();
                txtTenTang.Text = gvDanhSach.GetFocusedRowCellValue("TENTANG").ToString();
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
                    string query = "Delete from TANG where IDTANG =" + _idTang;
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                    loadData();
                    frmmain.showRoom();
                }
                catch (Exception)
                {
                    MessageBox.Show("Chưa chọn tầng đễ xóa");
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tentang = txtTenTang.Text;
            if (_them)
            {
                try
                {
                    string query = "Insert into TANG values (N'" + tentang + "')";
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                }
                catch (Exception err)
                {
                    MessageBox.Show("Tên tầng không được trùng nhau");
                }
            }
            else
            {
                try
                {
                    string query = "UPDATE TANG set TENTANG = N'" + tentang + "' where IDTANG =" + _idTang;
                    DataProvider provider = new DataProvider();
                    provider.ExecuteQuery(query);
                }
                catch (Exception err)
                {
                    MessageBox.Show("Tên tầng không được trùng nhau");
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

        private void frmTang_Load(object sender, EventArgs e)
        {
            loadData();
            showHideControl(true);
            _enebled(false);
        }
        void loadData()
        {
            string query = "SELECT * FROM dbo.TANG";
            DataProvider provider = new DataProvider();
            gcDanhSach.DataSource = provider.ExecuteQuery(query);
            gvDanhSach.OptionsBehavior.Editable = false;
        }
        void _enebled(bool t)
        {
            txtTenTang.Enabled = t;
        }
        void _reset()
        {
            txtTenTang.Text = "";
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
    }
}