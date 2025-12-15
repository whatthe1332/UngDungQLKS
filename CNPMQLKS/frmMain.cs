using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CNPMQLKS.DAO;
using DevExpress.Utils.Drawing;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Ribbon.ViewInfo;
using DevExpress.XtraNavBar;

namespace CNPMQLKS
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {
        public frmMain(int idnv)
        {
            InitializeComponent();
            _idnv = idnv;
        }
        public int _idnv;
        GalleryItem item = null;
        private void frmMain_Load(object sender, EventArgs e)
        {
            leftMenu();
            showRoom();
        }
        void leftMenu()
        {
            string query = "SELECT * FROM dbo.FUNC where ISGROUP = 'true' and MENU = 1 order by SORT ASC";
            DataProvider provider = new DataProvider();
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                NavBarGroup navGroup = new NavBarGroup(row["DESCRIPTION"].ToString());
                navGroup.Tag = row["FUNC_CODE"].ToString();
                navGroup.Name = row["FUNC_CODE"].ToString();
                navMain.Groups.Add(navGroup);
                getChild(row["FUNC_CODE"].ToString(),navGroup);
                navMain.Groups[navGroup.Name].Expanded = true;
            }
        }
        void getChild(string parent, NavBarGroup navGroup)
        {
            string query = "SELECT * FROM dbo.FUNC where ISGROUP = 'false' and MENU = 1 and PARENT = '" + parent + "' order by SORT DESC";
            DataProvider provider = new DataProvider();
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                NavBarItem navItem = new NavBarItem(row["DESCRIPTION"].ToString());
                navItem.Tag = row["FUNC_CODE"].ToString();
                navItem.Name = row["FUNC_CODE"].ToString();
                navGroup.ItemLinks.Add(navItem);
            }
        }
        public void showRoom()
        {
            gControl.Gallery.Groups.Clear();
            string query = "SELECT * FROM dbo.TANG";
            DataProvider provider = new DataProvider();
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            gControl.Gallery.ItemImageLayout = ImageLayoutMode.ZoomInside;
            gControl.Gallery.ImageSize = new Size(64,64);
            gControl.Gallery.ShowItemText = true;
            gControl.Gallery.ShowGroupCaption = true;
            foreach (DataRow row in dt.Rows)
            {
                var galleryItem = new GalleryItemGroup();
                galleryItem.Caption = row["TENTANG"].ToString();
                galleryItem.CaptionAlignment = GalleryItemGroupCaptionAlignment.Stretch;
                string query2 = "SELECT * FROM dbo.PHONG where IDTANG =" + row["IDTANG"].ToString(); ;
                DataTable dt2 = new DataTable();
                dt2 = provider.ExecuteQuery(query2);
                foreach (DataRow row2 in dt2.Rows)
                {
                    var gc_item = new GalleryItem();
                    gc_item.Caption = row2["TENPHONG"].ToString();
                    gc_item.Value = row2["IDPHONG"].ToString();
                    if (row2["TINHTRANG"].ToString() == "False")
                        gc_item.ImageOptions.Image = imageList1.Images[0];
                    else
                        gc_item.ImageOptions.Image = imageList1.Images[1];
                    galleryItem.Items.Add(gc_item);
                }
                gControl.Gallery.Groups.Add(galleryItem);
            }
        }
        private void navMain_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            string func_code = e.Link.Item.Tag.ToString();
            switch (func_code)
            {
                case "KHACHHANG":
                    {
                        frmKH frm = new frmKH();
                        frm.ShowDialog();
                        break;
                    }
                case "LOAIPHONG":
                    {
                        frmLoaiPhong frm = new frmLoaiPhong();
                        frm.ShowDialog();
                        break;
                    }
                case "TANG":
                    {
                        frmTang frm = new frmTang(this);
                        frm.ShowDialog();
                        break;
                    }
                case "PHONG":
                    {
                        frmPhong frm = new frmPhong(this);
                        frm.ShowDialog();
                        break;
                    }
                case "THIETBI":
                    {
                        frmThietBi frm = new frmThietBi();
                        frm.ShowDialog();
                        break;
                    }
                case "SANPHAM":
                    {
                        frmDichVu frm = new frmDichVu();
                        frm.ShowDialog();
                        break;
                    }
                case "PHONG_THIETBI":
                    {
                        frmPhongTB frm = new frmPhongTB();
                        frm.ShowDialog();
                        break;
                    }
                case "DATPHONGTHEODOAN":
                    {
                        frmDatPhong frm = new frmDatPhong();
                        frm.ShowDialog();
                        break;
                    }
                case "QUANLYNV":
                    {
                        int idcv = 0;
                        string query = "SELECT * FROM dbo.NHANVIEN WHERE IDNV = " + _idnv;
                        DataProvider provider = new DataProvider();
                        DataTable dt = new DataTable();
                        dt = provider.ExecuteQuery(query);
                        foreach (DataRow row in dt.Rows)
                        {
                            idcv = int.Parse(row["IDCHUCVU"].ToString());
                        }    
                        if (idcv == 1)
                        {
                            frmNhanVien frm = new frmNhanVien();
                            frm.ShowDialog();
                        }    
                        else
                        {
                            MessageBox.Show("Tài khoản bạn không có quyền vào chức năng này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    }
                case "DOIMK":
                    {
                        frmForgetPass frm = new frmForgetPass();
                        frm.ShowDialog();
                        break;
                    }
            }    
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            frmBaoCao frm = new frmBaoCao();
            frm.ShowDialog();
        }

        private void popupMenu1_Popup(object sender, EventArgs e)
        {
            Point point = gControl.PointToClient(Control.MousePosition);
            RibbonHitInfo hitinfo = gControl.CalcHitInfo(point);
            if (hitinfo.InGalleryItem || hitinfo.HitTest == RibbonHitTest.GalleryImage)
                item = hitinfo.GalleryItem;
        }

        private void btnSPDV_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!checkEmpty(int.Parse(item.Value.ToString())))
            {
                MessageBox.Show("Phòng chưa được đặt nên không thể cập nhật dịch vụ. Vui lòng chọn phòng khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            frmDatPhongDon frm = new frmDatPhongDon();
            frm._idPhong = int.Parse(item.Value.ToString());
            frm._them = false;
            frm._thanhtoan = false;
            frm.ShowDialog();
        }

        private void btnChuyenPhong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!checkEmpty(int.Parse(item.Value.ToString())))
            {
                MessageBox.Show("Phòng chưa được đặt nên không thể chuyển. Vui lòng chọn phòng khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            frmChuyenPhong frm = new frmChuyenPhong();
            frm._idPhong = int.Parse(item.Value.ToString());
            frm.ShowDialog();
        }
        private void btnDatPhong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (checkEmpty(int.Parse(item.Value.ToString())))
            {
                MessageBox.Show("Phòng đã được đặt. Vui lòng chọn phòng khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            frmDatPhongDon frm = new frmDatPhongDon();
            frm._idPhong = int.Parse(item.Value.ToString());
            frm._them = true;
            frm._thanhtoan = false;
            frm.ShowDialog();
        }
        public bool checkEmpty(int idphong)
        {
            string query = "SELECT * FROM dbo.PHONG WHERE IDPHONG = " + idphong;
            DataProvider provider = new DataProvider();
            DataTable dt = new DataTable();
            dt = provider.ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                if ((bool)row["TINHTRANG"] == true)
                    return true;
                else
                    return false;
            }
            return false;
        }    
        private void btnThanhToan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!checkEmpty(int.Parse(item.Value.ToString())))
            {
                MessageBox.Show("Phòng chưa được đặt nên không thể thanh toán. Vui lòng chọn phòng khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            frmDatPhongDon frm = new frmDatPhongDon();
            frm._idPhong = int.Parse(item.Value.ToString());
            frm._them = false;
            frm._thanhtoan = true;
            frm.ShowDialog();
        }
    }
}
