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

        public void LuuKetQua(string maLop, string maNamHoc)
        {
            KQHSCaNamDAO.Instance.XoaKetQua(maLop, maNamHoc);

            LopDTO lop = new LopDTO();
            lop.MaLop = maLop;

            NamHocDTO namHoc = new NamHocDTO();
            namHoc.MaNamHoc = maNamHoc;

            HanhKiemDTO hanhKiem = new HanhKiemDTO();
            hanhKiem.MaHanhKiem = "HK0001";

            int diemDat = QuyDinhDAO.Instance.LayDiemDatQuyDinh();

            DataTable DanhsachHocKy = HocKyDAO.Instance.LayDanhSachHocKy();
            DataTable DanhSachHocSinh = HocSinhDAO.Instance.LayDanhSachHocSinhTheoLop(maNamHoc, maLop);
            DataTable DanhSachMon = MonHocDAO.Instance.LayDanhSachMonHoc(maNamHoc, maLop);

            // LayDiemTBCN
            foreach (DataRow rhocSinh in DanhSachHocSinh.Rows)
            {
                string maHocSinh = rhocSinh["MaHocSinh"].ToString();
                HocSinhDTO hocSinh = new HocSinhDTO();
                hocSinh.MaHocSinh = maHocSinh;

                //Lay diem HK1:
                string maHocKy = "HK1";
                float diemHK1 = 0;
                float tongdiemChung = 0;
                int tongheSoChung = 0;

                foreach (DataRow rmon in DanhSachMon.Rows)
                {
                    string maMonHoc = rmon["MaMonHoc"].ToString();

                    float diemTBMon = 0;
                    float tongdiemMon = 0;
                    int tongheSoMon = 0;

                    DataTable DanhSachDiem = DiemDAO.Instance.LayDanhSachDiemHocSinh(maHocSinh, maMonHoc, maHocKy, maNamHoc, maLop);
                    foreach (DataRow row in DanhSachDiem.Rows)
                    {
                        float diem = Convert.ToSingle(row["Diem"].ToString());
                        int heSo = Convert.ToInt32(row["HeSo"].ToString());
                        tongdiemMon += diem * heSo;
                        tongheSoMon += heSo;
                    }
                    
                    if (tongheSoMon != 0) diemTBMon = (float)Math.Round(tongdiemMon / tongheSoMon, 2);
                    int heSoMon = Convert.ToInt32(rmon["HeSo"].ToString());
                    tongdiemChung += diemTBMon * heSoMon;
                    tongheSoChung += heSoMon;
                }
                if (tongheSoChung != 0) diemHK1 = (float)Math.Round(tongdiemChung / tongheSoChung, 2);
                int heSoHK1 = Convert.ToInt32(DanhsachHocKy.Rows[0]["HeSo"].ToString());
                //Lay diem HK2:
                maHocKy = "HK2";
                float diemHK2 = 0;
                tongdiemChung = 0;
                tongheSoChung = 0;

                foreach (DataRow rmon in DanhSachMon.Rows)
                {
                    string maMonHoc = rmon["MaMonHoc"].ToString();

                    float diemTBMon = 0;
                    float tongdiemMon = 0;
                    int tongheSoMon = 0;

                    DataTable DanhSachDiem = DiemDAO.Instance.LayDanhSachDiemHocSinh(maHocSinh, maMonHoc, maHocKy, maNamHoc, maLop);
                    foreach (DataRow row in DanhSachDiem.Rows)
                    {
                        float diem = Convert.ToSingle(row["Diem"].ToString());
                        int heSo = Convert.ToInt32(row["HeSo"].ToString());
                        tongdiemMon += diem * heSo;
                        tongheSoMon += heSo;
                    }

                    if (tongheSoMon != 0) diemTBMon = (float)Math.Round(tongdiemMon / tongheSoMon, 2);
                    int heSoMon = Convert.ToInt32(rmon["HeSo"].ToString());
                    tongdiemChung += diemTBMon * heSoMon;
                    tongheSoChung += heSoMon;
                }
                if (tongheSoChung != 0) diemHK2 = (float)Math.Round(tongdiemChung / tongheSoChung, 2);
                int heSoHK2 = Convert.ToInt32(DanhsachHocKy.Rows[1]["HeSo"].ToString());
                // Lay diem CN:
            
                float diemCN = (float)Math.Round((diemHK1 * heSoHK1 + diemHK2 * heSoHK2) / (heSoHK1 + heSoHK2), 2);
                HocLucDTO hocLuc = new HocLucDTO();
                hocLuc.MaHocLuc = diemCN < diemDat ? "HL0001" : "HL0004";
                KetQuaDTO ketQua = new KetQuaDTO();
                ketQua.MaKetQua = diemCN < diemDat ? "KQ0004" : "KQ0001";
                KQHSCaNamDAO.Instance.LuuKetQua(new KQHSCaNamDTO(
                hocSinh,
                lop,
                namHoc,
                hocLuc,
                hanhKiem,
                ketQua,
                diemHK1,
                diemHK2,
                diemCN
            ));
            }
        }
   
        public IList<KQHSCaNamDTO> Report(string maLop, string maNamHoc)
        {

            KQHSCaNamBUS.Instance.LuuKetQua(maLop, maNamHoc);
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
