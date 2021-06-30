using System.Collections.Generic;
using System;
using DTO;
using System.Data;
using DAO;

namespace BUS
{
    public class KQHSCaNamBUS
    {
        private static KQHSCaNamBUS instance;

        private KQHSCaNamBUS() { }

        public static KQHSCaNamBUS Instance
        {
            get
            {
                if (instance == null) instance = new KQHSCaNamBUS();
                return instance;
            }
            private set => instance = value;
        }

        public void LuuKetQua(string maHocSinh, string maLop, string maNamHoc, bool first)
        {
            HocSinhDTO hocSinh = new HocSinhDTO();
            hocSinh.MaHocSinh = maHocSinh;

            LopDTO lop = new LopDTO();
            lop.MaLop = maLop;

            NamHocDTO namHoc = new NamHocDTO();
            namHoc.MaNamHoc = maNamHoc;

            HocLucDTO hocLuc = new HocLucDTO();
            hocLuc.MaHocLuc = HocLucBUS.Instance.XepLoaiHocLucCaNam(maHocSinh, maLop, maNamHoc);

            HanhKiemDTO hanhKiem = new HanhKiemDTO();
            hanhKiem.MaHanhKiem = "HK0001";

            int diemDat = QuyDinhDAO.Instance.LayDiemDatQuyDinh();
            float diemTBCNChung = DiemBUS.Instance.LayDiemTBCNChung(maHocSinh, maLop, maNamHoc);

            KetQuaDTO ketQua = new KetQuaDTO();
            ketQua.MaKetQua = (float)Math.Round((diemhk1 + diemhk2 * 2) / 3, 2) < diemDat ? "KQ0004" : "KQ0001";

            float diemhk1 = DiemBUS.Instance.LayDiemTBHKChung(maHocSinh, maLop, maNamHoc, "HK1");
            float diemhk2 = DiemBUS.Instance.LayDiemTBHKChung(maHocSinh, maLop, maNamHoc, "HK2");
            if (first) KQHSCaNamDAO.Instance.XoaKetQua(maLop,maNamHoc);
            KQHSCaNamDAO.Instance.LuuKetQua(new KQHSCaNamDTO(
                hocSinh,
                lop,
                namHoc,
                hocLuc,
                hanhKiem,
                ketQua,
                diemhk1,
                diemhk2,
                (float)Math.Round((diemhk1+ diemhk2*2)/3, 2)
            ));
        }

        public IList<KQHSCaNamDTO> Report(string maLop, string maNamHoc)
        {
            //Sua
            int count = 0;
            DataTable dataTable1 = HocSinhDAO.Instance.LayDanhSachHocSinhTheoLop(maNamHoc, maLop);

            foreach (DataRow row in dataTable1.Rows)
            {
                count++;
                string maHocSinh = row["MaHocSinh"].ToString();
                KQHSCaNamBUS.Instance.LuuKetQua(maHocSinh, maLop, maNamHoc, count == 1);
            }
            DataTable dataTable = KQHSCaNamDAO.Instance.Report(maLop, maNamHoc);
            IList<KQHSCaNamDTO> ilist = new List<KQHSCaNamDTO>();

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

                HocLucDTO hocLuc = new HocLucDTO();
                hocLuc.MaHocLuc = Convert.ToString(Row["MaHocLuc"]);
                hocLuc.TenHocLuc = Convert.ToString(Row["TenHocLuc"]);

                HanhKiemDTO hanhKiem = new HanhKiemDTO();
                hanhKiem.MaHanhKiem = Convert.ToString(Row["MaHanhKiem"]);
                hanhKiem.TenHanhKiem = Convert.ToString(Row["TenHanhKiem"]);

                KetQuaDTO ketQua = new KetQuaDTO();
                ketQua.MaKetQua = Convert.ToString(Row["MaKetQua"]);
                ketQua.TenKetQua = Convert.ToString(Row["TenKetQua"]);

                ilist.Add(new KQHSCaNamDTO(
                    hocSinh,
                    lop,
                    namHoc,
                    hocLuc,
                    hanhKiem,
                    ketQua,
                    Convert.ToSingle(Row["DiemTBHK1"]),
                    Convert.ToSingle(Row["DiemTBHK2"]),
                    Convert.ToSingle(Row["DiemTBCN"])
                ));
            }
            return ilist;
        }
    }
}
