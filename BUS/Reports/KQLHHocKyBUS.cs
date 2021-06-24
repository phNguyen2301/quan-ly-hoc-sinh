using System.Collections.Generic;
using System;
using DTO;
using System.Data;
using DAO;

namespace BUS
{
    public class KQLHHocKyBUS
    {
        private static KQLHHocKyBUS instance;

        private KQLHHocKyBUS() { }

        public static KQLHHocKyBUS Instance
        {
            get
            {
                if (instance == null) instance = new KQLHHocKyBUS();
                return instance;
            }
            private set => instance = value;
        }

        public void LuuKetQua(string maLop, string maHocKy)
        {
            LopDTO lop = new LopDTO();
            lop.MaLop = maLop;
            lop.SiSo = LopDAO.Instance.LaySiSo(maLop);

            HocKyDTO hocKy = new HocKyDTO();
            hocKy.MaHocKy = maHocKy;

            int soLuongDat = DiemBUS.Instance.LaySoLuongDat(maLop,maHocKy);
            KQLHHocKyDAO.Instance.XoaKetQua(maLop, maHocKy);
            KQLHHocKyDAO.Instance.LuuKetQua(new KQLHHocKyDTO(
                lop,
                hocKy,
                soLuongDat,
                Convert.ToSingle(Math.Round(soLuongDat * 100F / lop.SiSo, 2))
            ));
        }

        public IList<KQLHHocKyDTO> Report(string maHocKy)
        {
            DataTable dataTable = KQLHHocKyDAO.Instance.Report(maHocKy);
            IList<KQLHHocKyDTO> ilist = new List<KQLHHocKyDTO>();

            foreach (DataRow Row in dataTable.Rows)
            {
                LopDTO lop = new LopDTO();
                lop.MaLop = Convert.ToString(Row["MaLop"]);
                lop.TenLop = Convert.ToString(Row["TenLop"]);
                lop.SiSo = Convert.ToInt32(Row["SiSo"]);


                HocKyDTO hocKy = new HocKyDTO();
                hocKy.MaHocKy = Convert.ToString(Row["MaHocKy"]);
                hocKy.TenHocKy = Convert.ToString(Row["TenHocKy"]);

                ilist.Add(new KQLHHocKyDTO(
                    lop,
                    hocKy,
                    Convert.ToInt32(Row["SoLuongDat"]),
                    Convert.ToSingle(Row["TiLe"])
                ));
            }
            return ilist;
        }
    }
}
