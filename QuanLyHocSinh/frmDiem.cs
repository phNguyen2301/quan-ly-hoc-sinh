using BUS;
using DevComponents.DotNetBar;
using DTO;
using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyHocSinh
{
    public partial class frmDiem : Office2007Form
    {

        public frmDiem()
        {
            InitializeComponent();
        }

        private void frmDiem_Load(object sender, EventArgs e)
        {
            NamHocBUS.Instance.HienThiComboBox(cmbNamHoc);
            HocKyBUS.Instance.HienThiComboBox(cmbHocKy);

            if (cmbNamHoc.SelectedValue != null)
                LopBUS.Instance.HienThiComboBox(cmbNamHoc.SelectedValue.ToString(), cmbLop);

            if (cmbNamHoc.SelectedValue != null && cmbLop.SelectedValue != null)
                MonHocBUS.Instance.HienThiComboBox(
                    cmbNamHoc.SelectedValue.ToString(), 
                    cmbLop.SelectedValue.ToString(), 
                    cmbMonHoc
                );
        }

        private void btnLuuDiem_Click(object sender, EventArgs e)
        {
            string[] colNames = { "colDiemMieng", "colDiem15Phut", "colDiem45Phut", "colDiemThi" };
            if (!KiemTraTruocKhiLuu.KiemTraDiem(dgvDiem, colNames)) return;

            string maLop = cmbLop.SelectedValue.ToString();
            string maNamHoc = cmbNamHoc.SelectedValue.ToString();
            string maMonHoc = cmbMonHoc.SelectedValue.ToString();
            string maHocKy = cmbHocKy.SelectedValue.ToString();
            int rowCount = 0;
            foreach (DataGridViewRow row in dgvDiem.Rows)
            {
                rowCount++;
                string maHocSinh = row.Cells["colMaHocSinh"].Value.ToString();

                for (int i = 0; i < colNames.Length; i++)
                {
                    string Diem = row.Cells[colNames[i]].Value.ToString();
                    {
                        if (!string.IsNullOrWhiteSpace(Diem))
                        {
                            DiemDTO diem = new DiemDTO(maHocSinh, maMonHoc, maHocKy, maNamHoc, maLop, $"LD000{i + 1}", float.Parse(Diem));
                            DiemBUS.Instance.UpdateDiem(diem);
                        }
                    }
                }
            }
            KQLHMonHocBUS.Instance.LuuKetQua(maLop, maNamHoc, maMonHoc, maHocKy);
            KQLHHocKyBUS.Instance.LuuKetQua(maLop, maNamHoc, maHocKy);
            MessageBox.Show("Cập nhật thành công!", "COMPLETED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnHienThiDanhSach_Click(sender, e);

            frmXemDiem frm = (frmXemDiem)Application.OpenForms["frmXemDiem"];
            if (frm != null) frm.btnHienThiClicked(sender, e);

        }

        private void btnXemDiem_Click(object sender, EventArgs e)
        {
            Utilities.ShowForm("frmXemDiem");
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnThemNamHoc_Click(object sender, EventArgs e)
        {
            Utilities.ShowForm("frmNamHoc");
            NamHocBUS.Instance.HienThiComboBox(cmbNamHoc);
        }

        private void btnThemLop_Click(object sender, EventArgs e)
        {
            Utilities.ShowForm("frmLop");
            LopBUS.Instance.HienThiComboBox(cmbNamHoc.SelectedValue.ToString(), cmbLop);
        }

        private void btnThemHocKy_Click(object sender, EventArgs e)
        {
            Utilities.ShowForm("frmHocKy");
            HocKyBUS.Instance.HienThiComboBox(cmbHocKy);
        }

        private void btnThemMonHoc_Click(object sender, EventArgs e)
        {
            Utilities.ShowForm("frmMonHoc");
            MonHocBUS.Instance.HienThiComboBox(
                cmbNamHoc.SelectedValue.ToString(), 
                cmbLop.SelectedValue.ToString(), 
                cmbMonHoc
            );
        }

        // Lấy thông tin lớp theo từng năm học
        private void cmbNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNamHoc.SelectedValue != null)
                LopBUS.Instance.HienThiComboBox(cmbNamHoc.SelectedValue.ToString(), cmbLop);
            cmbLop.DataBindings.Clear();
        }

        // Lấy môn học theo từng lớp
        private void cmbLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNamHoc.SelectedValue != null && cmbLop.SelectedValue != null)
                MonHocBUS.Instance.HienThiComboBox(
                    cmbNamHoc.SelectedValue.ToString(), 
                    cmbLop.SelectedValue.ToString(), 
                    cmbMonHoc
                );
            cmbMonHoc.DataBindings.Clear();
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

        internal void btnLuuDiemClicked(object sender, EventArgs e)
        {
            btnLuuDiem_Click(sender, e);
        }
    }
}
