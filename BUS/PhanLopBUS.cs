using DAO;
using DevComponents.DotNetBar.Controls;
using DTO;
using System.Windows.Forms;

namespace BUS
{
    public class PhanLopBUS
    {
        private static PhanLopBUS instance;

        private PhanLopBUS() { }

        public static PhanLopBUS Instance
        {
            get
            {
                if (instance == null) instance = new PhanLopBUS();
                return instance;
            }
            private set => instance = value;
        }
        public void ThemHocSinhVaoLop(string maHocSinh, string maLop)
        {
            PhanLopDAO.Instance.ThemHocSinhVaoLop(maHocSinh, maLop);
            LopDAO.Instance.ThemSiSo(maLop);
        }
        public void LuuHocSinhVaoBangPhanLop(string namHoc, string khoiLop, string lop, ListViewEx listViewEx)
        {
            foreach (ListViewItem item in listViewEx.Items)
            {
                PhanLopDTO phanLop = new PhanLopDTO(namHoc, khoiLop, lop, item.SubItems[0].Text.ToString());
                PhanLopDAO.Instance.LuuHocSinhVaoBangPhanLop(phanLop);
            }
            LopDAO.Instance.ThemSiSo(lop);
        }

        public void XoaHocSinhKhoiBangPhanLop(string namHoc, string khoiLop, string lop, ListViewEx listViewEx)
        {
            foreach (ListViewItem item in listViewEx.Items)
            {
                PhanLopDTO phanLop = new PhanLopDTO(namHoc, khoiLop, lop, item.SubItems[0].Text.ToString());
                PhanLopDAO.Instance.XoaHocSinhKhoiBangPhanLop(phanLop);
            }
            LopDAO.Instance.BotSiSo(lop);
        }
    }
}
