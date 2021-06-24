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

        public void LuuKetQua(string maHocSinh, string maLop)
        {
            HocSinhDTO hocSinh = new HocSinhDTO();
            hocSinh.MaHocSinh = maHocSinh;

            LopDTO lop = new LopDTO();
            lop.MaLop = maLop;

            int diemDat = QuyDinhDAO.Instance.LayDiemDatQuyDinh();
            float diemTBCNChung = DiemBUS.Instance.LayDiemTBCNChung(maHocSinh, maLop);
            // Ket qua
            KetQuaDTO ketQua = new KetQuaDTO();
            ketQua.MaKetQua = diemTBCNChung < diemDat ? "KQ0004" : "KQ0001";

            KQHSCaNamDAO.Instance.XoaKetQua(maHocSinh, maLop);
            KQHSCaNamDAO.Instance.LuuKetQua(new KQHSCaNamDTO(
                hocSinh,
                lop,
                ketQua,
                DiemBUS.Instance.LayDiemTBHKChung(maHocSinh, maLop, "HK1"),
                DiemBUS.Instance.LayDiemTBHKChung(maHocSinh, maLop, "HK2"),
                diemTBCNChung
            ));
        }

        public IList<KQHSCaNamDTO> Report(string maLop)
        {
            DataTable dataTable = KQHSCaNamDAO.Instance.Report(maLop);
            IList<KQHSCaNamDTO> ilist = new List<KQHSCaNamDTO>();

            foreach (DataRow Row in dataTable.Rows)
            {
                HocSinhDTO hocSinh = new HocSinhDTO();
                hocSinh.MaHocSinh = Convert.ToString(Row["MaHocSinh"]);
                hocSinh.HoTen = Convert.ToString(Row["HoTen"]);

                LopDTO lop = new LopDTO();
                lop.MaLop = Convert.ToString(Row["MaLop"]);
                lop.TenLop = Convert.ToString(Row["TenLop"]);


                KetQuaDTO ketQua = new KetQuaDTO();
                ketQua.MaKetQua = Convert.ToString(Row["MaKetQua"]);
                ketQua.TenKetQua = Convert.ToString(Row["TenKetQua"]);

                ilist.Add(new KQHSCaNamDTO(
                    hocSinh,
                    lop,
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
