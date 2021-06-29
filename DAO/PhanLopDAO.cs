using DTO;
using System.Data;

namespace DAO
{
    public class PhanLopDAO
    {
        private static PhanLopDAO instance;

        private PhanLopDAO() { }

        public static PhanLopDAO Instance
        {
            get
            {
                if (instance == null) instance = new PhanLopDAO();
                return instance;
            }
            private set => instance = value;
        }
        public void ThemHocSinhVaoLop(string maHocSinh, string maLop)
        {
            string maNamHoc = "NH" + maLop.Substring(6);
            string maKhoiLop = "KHOI" + maLop.Substring(3, 2);
            string query = "EXEC LuuHocSinhVaoBangPhanLop @maNamHoc , @maKhoiLop , @maLop , @maHocSinh";
            object[] parameters = new object[] { maNamHoc, maKhoiLop, maLop, maHocSinh };
            DataProvider.Instance.ExecuteNonQuery(query, parameters);
        }
        public void LuuHocSinhVaoBangPhanLop(PhanLopDTO phanLop)
        {
            string query = "EXEC LuuHocSinhVaoBangPhanLop @maNamHoc , @maKhoiLop , @maLop , @maHocSinh";
            object[] parameters = new object[] { phanLop.MaNamHoc, phanLop.MaKhoiLop, phanLop.MaLop, phanLop.MaHocSinh };
            DataProvider.Instance.ExecuteNonQuery(query, parameters);
        }

        public void XoaHocSinhKhoiBangPhanLop(PhanLopDTO phanLop)
        {
            string query = "EXEC XoaHocSinhKhoiBangPhanLop @maNamHoc , @maKhoiLop , @maLop , @maHocSinh";
            object[] parameters = new object[] { phanLop.MaNamHoc, phanLop.MaKhoiLop, phanLop.MaLop, phanLop.MaHocSinh };
            DataProvider.Instance.ExecuteNonQuery(query, parameters);
        }
    }
}
