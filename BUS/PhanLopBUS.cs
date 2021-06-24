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

        public void LuuHocSinhVaoBangPhanLop(string maNamHoc, string khoiLop, string lop, ListViewEx listViewEx)
        {
            foreach (ListViewItem item in listViewEx.Items)
            {
                PhanLopDTO phanLop = new PhanLopDTO(maNamHoc, khoiLop, lop, item.SubItems[0].Text.ToString());
                PhanLopDAO.Instance.LuuHocSinhVaoBangPhanLop(phanLop);
            }
        }

        public void XoaHocSinhKhoiBangPhanLop(string maNamHoc, string khoiLop, string lop, ListViewEx listViewEx)
        {
            foreach (ListViewItem item in listViewEx.Items)
            {
                PhanLopDTO phanLop = new PhanLopDTO(maNamHoc, khoiLop, lop, item.SubItems[0].Text.ToString());
                PhanLopDAO.Instance.XoaHocSinhKhoiBangPhanLop(phanLop);
            }
        }
    }
}
