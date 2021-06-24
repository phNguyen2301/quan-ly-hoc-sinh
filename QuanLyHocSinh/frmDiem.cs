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
        private int[,] STT = null;

        public frmDiem()
        {
            InitializeComponent();
        }

        private void frmDiem_Load(object sender, EventArgs e)
        {
            HocKyBUS.Instance.HienThiComboBox(cmbHocKy);

            LopBUS.Instance.HienThiComboBox(cmbLop);

            if (cmbLop.SelectedValue != null)
                MonHocBUS.Instance.HienThiComboBox(
                    cmbLop.SelectedValue.ToString(), 
                    cmbMonHoc
                );
        }

        private void btnLuuDiem_Click(object sender, EventArgs e)
        {
            string[] colNames = { "colDiemMieng", "colDiem15Phut", "colDiem45Phut", "colDiemThi" };
            if (!KiemTraTruocKhiLuu.KiemTraDiem(dgvDiem, colNames) || STT == null) return;

            string maLop = cmbLop.SelectedValue.ToString();
            string maNamHoc = cmbNamHoc.SelectedValue.ToString();
            string maMonHoc = cmbMonHoc.SelectedValue.ToString();
            string maHocKy = cmbHocKy.SelectedValue.ToString();
            int rowCount = 0;

            foreach (DataGridViewRow row in dgvDiem.Rows)
            {
                string maHocSinh = row.Cells["colMaHocSinh"].Value.ToString();
                rowCount++;

                for (int i = 0; i < colNames.Length; i++)
                {
                    if (row.Cells[colNames[i]].Value.ToString() == "") row.Cells[colNames[i]].Value = 0;
                    string chuoiDiem = row.Cells[colNames[i]].Value.ToString();

                    if (string.IsNullOrWhiteSpace(chuoiDiem)) continue;
                    int count = 0;

                    for (int j = 0; j < chuoiDiem.Length; j++)
                    {
                        if (chuoiDiem[j] != ';' && j != chuoiDiem.Length - 1) count++;
                        else
                        {
                            if (j == chuoiDiem.Length - 1)
                            {
                                j++;
                                count++;
                            }

                            string diemDaXuLy = chuoiDiem.Substring(j - count, count);
                            if (!string.IsNullOrWhiteSpace(diemDaXuLy) && QuyDinhBUS.Instance.KiemTraDiem(diemDaXuLy))
                            {
                                DiemDTO diem = new DiemDTO(maHocSinh, maMonHoc, maHocKy, maLop, $"LD000{i + 1}",  float.Parse(diemDaXuLy));
                                DiemBUS.Instance.ThemDiem(diem);
                            }
                            count = 0;
                        }
                    }
                }

                #region Xóa các kết quả cũ
                for (int i = 1; i < 60; i++)
                {
                    for (int j = 1; j < 20; j++)
                    {
                        int id = STT[i, j];
                        if (id > 0) DiemBUS.Instance.XoaDiem(id);
                        else break;
                    }
                }
                #endregion

                #region Lưu vào bảng kết quả
                if (rowCount <= dgvDiem.Rows.Count)
                {
                    KQHSMonHocBUS.Instance.LuuKetQua(maHocSinh, maLop, maMonHoc, maHocKy);
                    KQHSCaNamBUS.Instance.LuuKetQua(maHocSinh, maLop);
                    KQLHMonHocBUS.Instance.LuuKetQua(maLop, maMonHoc, maHocKy);
                    KQLHHocKyBUS.Instance.LuuKetQua(maLop, maHocKy);
                }
                #endregion
            }

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

        private void btnThemLop_Click(object sender, EventArgs e)
        {
            Utilities.ShowForm("frmLop");
            LopBUS.Instance.HienThiComboBox(cmbLop);
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
                cmbLop.SelectedValue.ToString(), 
                cmbMonHoc
            );
        }

        // Lấy môn học theo từng lớp
        private void cmbLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNamHoc.SelectedValue != null && cmbLop.SelectedValue != null)
                MonHocBUS.Instance.HienThiComboBox(
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
                    cmbLop.SelectedValue.ToString()
                );
            DiemBUS.Instance.HienThi(dgvDiem, cmbMonHoc, cmbHocKy, cmbLop, ref STT);
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
