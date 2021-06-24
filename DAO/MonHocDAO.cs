using System.Data;

namespace DAO
{
    public class MonHocDAO
    {
        private static MonHocDAO instance;

        private MonHocDAO() { }

        public static MonHocDAO Instance
        {
            get
            {
                if (instance == null) instance = new MonHocDAO();
                return instance;
            }
            private set => instance = value;
        }

        public DataTable LayDanhSachMonHoc()
        {
            string query = "SELECT * FROM MONHOC";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public DataTable LayDanhSachMonHoc(string lop)
        {
            string query = "EXEC LayDanhSachMonHocTheoLop @maLop";
            object[] parameters = new object[] { lop };
            return DataProvider.Instance.ExecuteQuery(query, parameters);
        }

        public void CapNhatMonHoc(DataTable dataTable)
        {
            DataProvider.Instance.UpdateTable(dataTable, "MONHOC");
        }
    }
}
