using BUS;
using DevComponents.DotNetBar;
using System;
using System.Collections;
using System.Windows.Forms;

namespace QuanLyHocSinh
{
    public partial class frmXemDiem : Office2007Form
    {
        public frmXemDiem()
        {
            InitializeComponent();
        }

        private void frmXemDiem_Load(object sender, EventArgs e)
        {
            HocKyBUS.Instance.HienThiComboBox(cmbHocKy);

            LopBUS.Instance.HienThiComboBox(cmbLop);

            if (cmbLop.SelectedValue != null)
            {
                MonHocBUS.Instance.HienThiComboBox(
                    cmbLop.SelectedValue.ToString(), 
                    cmbMonHoc
                );
                HocSinhBUS.Instance.HienThiComboBox(
                    cmbLop.SelectedValue.ToString(), 
                    cmbHocSinh
                );
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    "Bạn có muốn xóa dòng này không ?", 
                    "DELETE", 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question
                ) == DialogResult.OK)
            {
                IEnumerator iEnumerator = lvXemDiem.SelectedItems.GetEnumerator();
                while (iEnumerator.MoveNext())
                {
                    ListViewItem item = (ListViewItem)iEnumerator.Current;
                    int stt = Convert.ToInt32(item.SubItems[0].Text);
                    DiemBUS.Instance.XoaDiem(stt);
                    lvXemDiem.Items.Remove(item);
                }
            }

            string maHocSinh = cmbHocSinh.SelectedValue.ToString();
            string maMonHoc = cmbMonHoc.SelectedValue.ToString();
            string maHocKy = cmbHocKy.SelectedValue.ToString();
            string maLop = cmbLop.SelectedValue.ToString();

            KQHSMonHocBUS.Instance.LuuKetQua(maHocSinh, maLop, maMonHoc, maHocKy);
            KQHSCaNamBUS.Instance.LuuKetQua(maHocSinh, maLop);

            frmDiem frm = (frmDiem)Application.OpenForms["frmDiem"];
            if (frm != null) frm.btnHienThiClicked(sender, e);
        }

        private void bindingNavigatorExitItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLop.SelectedValue != null)
            {
                MonHocBUS.Instance.HienThiComboBox(
                    cmbLop.SelectedValue.ToString(),
                    cmbMonHoc
                );
                HocSinhBUS.Instance.HienThiComboBox(
                    cmbLop.SelectedValue.ToString(),
                    cmbHocSinh
                );
            }

            cmbMonHoc.DataBindings.Clear();
            cmbHocSinh.DataBindings.Clear();
        }

        private void btnHienThiDanhSach_Click(object sender, EventArgs e)
        {
            DiemBUS.Instance.HienThiDanhSachXemDiem(
                lvXemDiem,
                cmbHocSinh.SelectedValue.ToString(),
                cmbMonHoc.SelectedValue.ToString(),
                cmbHocKy.SelectedValue.ToString(),
                cmbLop.SelectedValue.ToString()
            );
        }

        internal void btnHienThiClicked(object sender, EventArgs e)
        {
            btnHienThiDanhSach_Click(sender, e);
        }
    }
}
