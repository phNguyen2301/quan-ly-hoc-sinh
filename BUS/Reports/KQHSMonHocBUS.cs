using System.Collections.Generic;
using System;
using DTO;
using System.Data;
using DAO;

namespace BUS
{
    public class KQHSMonHocBUS
    {
        private static KQHSMonHocBUS instance;

        private KQHSMonHocBUS() { }

        public static KQHSMonHocBUS Instance
        {
            get
            {
                if (instance == null) instance = new KQHSMonHocBUS();
                return instance;
            }
            private set => instance = value;
        }
         
        public void LuuKetQua(string maHocSinh, string maLop, string maNamHoc, string maMonHoc, string maHocKy, bool first)
        {
            HocSinhDTO hocSinh = new HocSinhDTO();
            hocSinh.MaHocSinh = maHocSinh;

            LopDTO lop = new LopDTO();
            lop.MaLop = maLop;

            MonHocDTO monHoc = new MonHocDTO();
            monHoc.MaMonHoc = maMonHoc;

            HocKyDTO hocKy = new HocKyDTO();
            hocKy.MaHocKy = maHocKy;

            NamHocDTO namHoc = new NamHocDTO();
            namHoc.MaNamHoc = maNamHoc;

            float[] DanhSachDiem = DiemBUS.Instance.LayDiemHK(maHocSinh, maLop, maNamHoc, maMonHoc, maHocKy);
            if (first) KQHSMonHocDAO.Instance.XoaKetQua(maLop, maNamHoc, maMonHoc, maHocKy);
            KQHSMonHocDAO.Instance.LuuKetQua(new KQHSMonHocDTO(
                hocSinh,
                lop,
                namHoc,
                monHoc,
                hocKy,
                DanhSachDiem[0],
                DanhSachDiem[1],
                DanhSachDiem[2],
                DanhSachDiem[3],
                DanhSachDiem[4]
            ));
        }

        public IList<KQHSMonHocDTO> Report(string maLop, string maNamHoc, string maMonHoc, string maHocKy)
        {
            DataTable dataTable1 = HocSinhDAO.Instance.LayDanhSachHocSinhTheoLop(maNamHoc, maLop);
            int count = 0;
            foreach (DataRow row in dataTable1.Rows)
            {
                count++;
                string maHocSinh = row["MaHocSinh"].ToString();
                KQHSMonHocBUS.Instance.LuuKetQua(maHocSinh, maLop, maNamHoc, maMonHoc, maHocKy, count == 1);
            }
            DataTable dataTable = KQHSMonHocDAO.Instance.Report(maLop, maNamHoc, maMonHoc, maHocKy);
            IList<KQHSMonHocDTO> ilist = new List<KQHSMonHocDTO>();

            foreach (DataRow Row in dataTable.Rows)
            {
                HocSinhDTO hocSinh = new HocSinhDTO();
                hocSinh.MaHocSinh = Convert.ToString(Row["MaHocSinh"]);
                hocSinh.HoTen = Convert.ToString(Row["HoTen"]);

                LopDTO lop = new LopDTO();
                lop.MaLop = Convert.ToString(Row["MaLop"]);
                lop.TenLop = Convert.ToString(Row["TenLop"]);

                NamHocDTO namHoc = new NamHocDTO();
                namHoc.MaNamHoc = Convert.ToString(Row["MaNamHoc"]);
                namHoc.TenNamHoc = Convert.ToString(Row["TenNamHoc"]);

                MonHocDTO monHoc = new MonHocDTO();
                monHoc.MaMonHoc = Convert.ToString(Row["MaMonHoc"]);
                monHoc.TenMonHoc = Convert.ToString(Row["TenMonHoc"]);

                HocKyDTO hocKy = new HocKyDTO();
                hocKy.MaHocKy = Convert.ToString(Row["MaHocKy"]);
                hocKy.TenHocKy = Convert.ToString(Row["TenHocKy"]);

                ilist.Add(new KQHSMonHocDTO(
                    hocSinh,
                    lop,
                    namHoc,
                    monHoc,
                    hocKy,
                    Convert.ToSingle(Row["DiemMiengTB"]),
                    Convert.ToSingle(Row["Diem15PhutTB"]),
                    Convert.ToSingle(Row["Diem45PhutTB"]),
                    Convert.ToSingle(Row["DiemThi"]),
                    Convert.ToSingle(Row["DiemTBHK"])
                ));
            }
            return ilist;
        }
    }
}
