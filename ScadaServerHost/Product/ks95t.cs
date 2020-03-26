using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KtsDBHelper;

namespace ScadaServerHost.Product
{
    public class ks95t
    {
        static string CmdText = "INSERT INTO `test_db`.`ks95t` " +
            "(`devId`, `recordTime`, `temp`, `humi`) VALUES (@devId, @recordTime, @temp, @humi);";

        public static SqlInfo GetSqlText(uint devId, byte[] data)
        {
            SqlInfo res = new SqlInfo();
            Dictionary<string, object> param;
            bool c = ParseSourceData(devId, data, out param);
            if(c == true)
            {
                res.Command = CmdText;
                res.Param = param;
                res.IsValid = true;
                return res;
            }
            else
            {
                res.IsValid = false;
                return res;
            }

        }


        public static bool ParseSourceData(uint devId, byte[] data, out Dictionary<string, object> param)
        {
            string src = Encoding.Default.GetString(data);
            param = new Dictionary<string, object>();
            string ts = CommonLib.MidStrEx(src, "temp=", ";");
            string hs = CommonLib.MidStrEx(src, "humi=", ";");
            double temp, humi;
            try
            {
                temp = Convert.ToDouble(ts);
                humi = Convert.ToDouble(hs);
            }
            catch
            {
                return false;
            }
            param.Add("@devId", devId);
            param.Add("@recordTime", DateTime.Now);
            param.Add("@temp", temp);
            param.Add("@humi", humi);
            return true;
        }

    }
}
