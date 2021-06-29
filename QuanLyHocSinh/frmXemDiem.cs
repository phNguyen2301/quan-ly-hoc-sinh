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
            NamHocBUS.Instance.HienThiComboBox(cmbNamHoc);
            HocKyBUS.Instance.HienThiComboBox(cmbHocKy);

            if (cmbNamHoc.SelectedValue != null)
                LopBUS.Instance.HienThiComboBox(cmbNamHoc.SelectedValue.ToString(), cmbLop);

            if (cmbNamHoc.SelectedValue != null && cmbLop.SelectedValue != null)
            {
                MonHocBUS.Instance.HienThiComboBox(
                    cmbNamHoc.SelectedValue.ToString(), 
                    cmbLop.SelectedValue.ToString(), 
                    cmbMonHoc
                );
            }
        }


        private void bindingNavigatorExitItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNamHoc.SelectedValue != null)
                LopBUS.Instance.HienThiComboBox(cmbNamHoc.SelectedValue.ToString(), cmbLop);
            cmbLop.DataBindings.Clear();
        }

        private void cmbLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNamHoc.SelectedValue != null && cmbLop.SelectedValue != null)
            {
                MonHocBUS.Instance.HienThiComboBox(
                    cmbNamHoc.SelectedValue.ToString(),
                    cmbLop.SelectedValue.ToString(),
                    cmbMonHoc
                );
                HocSinhBUS.Instance.HienThiComboBox(
                    cmbNamHoc.SelectedValue.ToString(),
                    cmbLop.SelectedValue.ToString(),
                    cmbHocSinh
                );
            }

            cmbMonHoc.DataBindings.Clear();
            cmbHocSinh.DataBindings.Clear();
        }

        private void btnHienThiDanhSach_Click(object sender, EventArgs e)
        {
            if (cmbNamHoc.SelectedValue != null &&
                cmbLop.SelectedValue != null &&
                cmbHocKy.SelectedValue != null &&
                cmbMonHoc.SelectedValue != null)
                HocSinhBUS.Instance.HienThiHocSinhTheoLop(
                    bindingNavigatorDiem,
                    dgvDiem,
                    cmbNamHoc.SelectedValue.ToString(),
                    cmbLop.SelectedValue.ToString()
                );
            DiemBUS.Instance.HienThi(dgvDiem, cmbMonHoc, cmbHocKy, cmbNamHoc, cmbLop);
        }

        internal void btnHienThiClicked(object sender, EventArgs e)
        {
            btnHienThiDanhSach_Click(sender, e);
        }
    }
}
