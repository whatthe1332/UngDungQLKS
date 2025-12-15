using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNPMQLKS
{
    public class CTRANGTHAI
    {
        public bool _value { set; get; }
        public string _display { set; get; }
        public CTRANGTHAI()
        {

        }
        public CTRANGTHAI(bool _val, string _dis)
        {
            this._value = _val;
            this._display = _dis;
        }
        public static List<CTRANGTHAI> getList()
        {
            List<CTRANGTHAI> lst = new List<CTRANGTHAI>();
            CTRANGTHAI[] collect = new CTRANGTHAI[2]
            {
                new CTRANGTHAI(false, "Chưa tính tiền"),
                new CTRANGTHAI(true, "Đã tính tiền")
            };
            lst.AddRange(collect);
            return lst;
        }
    }
}
