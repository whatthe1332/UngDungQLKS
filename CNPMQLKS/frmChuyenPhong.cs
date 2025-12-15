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
    public partial class frmChuyenPhong : DevExpress.XtraEditors.XtraForm
    {
        public frmChuyenPhong()
        {
            InitializeComponent();
        }
        frmMain objMain = (frmMain)Application.OpenForms["frmMain"];
        public int _idPhong;
        OBJ_PHONG _phongchuyenden;
        int idDPCT = 0;
        int songayo = 1;
        private void frmChuyenPhong_Load(object sender, EventArgs e)
        {
            _phongchuyenden = new OBJ_PHONG();
            string query = "SELECT * FROM PHONG, LOAIPHONG WHERE PHONG.IDLOAIPHONG = LOAIPHONG.IDLOAIPHONG AND IDPHONG = " + _idPhong;
            DataProvider provider = new DataProvider();
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                int dongia = int.Parse(row["DONGIA"].ToString());
                lblPhong.Text = row["TENPHONG"].ToString() + " - Đơn giá: " + dongia.ToString("N0");
            }
            loadPhongTrong();
        }
        void loadPhongTrong()
        {
            string query = "SELECT * FROM PHONG, LOAIPHONG WHERE TINHTRANG = 0 AND PHONG.IDLOAIPHONG = LOAIPHONG.IDLOAIPHONG";
            DataProvider provider = new DataProvider();
            searchPhong.Properties.DataSource = provider.ExecuteQuery(query);
            searchPhong.Properties.ValueMember = "IDPHONG";
            searchPhong.Properties.DisplayMember = "TENPHONG";
        }

        private void btnChuyenPhong_Click(object sender, EventArgs e)
        {
            if (searchPhong.EditValue == null || searchPhong.EditValue.ToString() == "")
            {
                MessageBox.Show("Vui lòng chọn phòng muốn chuyển đến.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string query = $"SELECT * FROM DATPHONG_CT WHERE IDPHONG = {_idPhong} ORDER BY IDDPCT DESC";
            DataProvider provider = new DataProvider();
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                idDPCT = int.Parse(row["IDDPCT"].ToString());
                songayo = int.Parse(row["SONGAYO"].ToString());
                break;
            }
            string query2 = "SELECT * FROM PHONG, LOAIPHONG WHERE PHONG.IDLOAIPHONG = LOAIPHONG.IDLOAIPHONG AND IDPHONG = " + searchPhong.EditValue.ToString();
            DataTable dt2 = new DataTable();
            dt2 = provider.ExecuteQuery(query2);
            foreach (DataRow row2 in dt2.Rows)
            {
                _phongchuyenden.IDPHONG = int.Parse(searchPhong.EditValue.ToString());
                _phongchuyenden.TENPHONG = row2["TENPHONG"].ToString();
                _phongchuyenden.IDLOAIPHONG = int.Parse(row2["IDLOAIPHONG"].ToString());
                _phongchuyenden.TINHTRANG = (bool)row2["TINHTRANG"];
                _phongchuyenden.IDTANG = int.Parse(row2["IDTANG"].ToString());
                _phongchuyenden.DONGIA = int.Parse(row2["DONGIA"].ToString());
            }
            string query3 = $"UPDATE PHONG SET TINHTRANG = 0 where IDPHONG = {_idPhong}";
            provider.ExecuteQuery(query3);
            string query4 = $"UPDATE PHONG SET TINHTRANG = 1 where IDPHONG = {_phongchuyenden.IDPHONG}";
            provider.ExecuteQuery(query4);
            string query5 = $"UPDATE DATPHONG_CT SET IDPHONG = {_phongchuyenden.IDPHONG}, DONGIA = {_phongchuyenden.DONGIA}, THANHTIEN = {_phongchuyenden.DONGIA * songayo}  WHERE IDDPCT = {idDPCT}";
            provider.ExecuteQuery(query5);
            string query6 = $"UPDATE DATPHONG_DICHVU SET IDPHONG = {int.Parse(searchPhong.EditValue.ToString())} WHERE IDDPCT = {idDPCT}";
            provider.ExecuteQuery(query6);
            objMain.showRoom();
            this.Close();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}