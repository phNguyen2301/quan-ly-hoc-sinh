using DTO;
using System.Data;

namespace DAO
{
    public class KQLHMonHocDAO
    {
        private static KQLHMonHocDAO instance;

        private KQLHMonHocDAO() { }

        public static KQLHMonHocDAO Instance
        {
            get
            {
                if (instance == null) instance = new KQLHMonHocDAO();
                return instance;
            }
            private set => instance = value;
        }

        public DataTable Report(string maMonHoc, string maHocKy)
        {
            string query = "EXEC ReportKQLHMonHoc @maNamHoc , @maMonHoc , @maHocKy";
            object[] parameters = new object[] { maMonHoc, maHocKy };
            return DataProvider.Instance.ExecuteQuery(query, parameters);
        }

        public void LuuKetQua(KQLHMonHocDTO ketQua)
        {
            string query = "EXEC ThemKQLHMonHoc @maLop , @maNamHoc , @maMonHoc , @maHocKy , @soLuongDat , @tiLe";
            object[] parameters = new object[] {
                ketQua.Lop.MaLop,
                ketQua.MonHoc.MaMonHoc,
                ketQua.HocKy.MaHocKy,
                ketQua.SoLuongDat,
                ketQua.TiLe
            };
            DataProvider.Instance.ExecuteQuery(query, parameters);
        }

        public void XoaKetQua(string maLop, string maMonHoc, string maHocKy)
        {
            string query = "EXEC XoaKQLHMonHoc @maLop , @maNamHoc , @maMonHoc , @maHocKy";
            object[] parameters = new object[] { maLop, maMonHoc, maHocKy };
            DataProvider.Instance.ExecuteQuery(query, parameters);
        }
    }
}
