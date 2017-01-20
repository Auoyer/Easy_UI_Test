using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC_T.Controllers
{
    public class Things
    {
        public string aa { get; set; }
        public string bb { get; set; }
        public string cc { get; set; }
        public string dd { get; set; }
        public string ee { get; set; }
        public string ff { get; set; }
        public string gg { get; set; }
    }

    public static class Ex_Things
    {
       public static List<Things> ts = new List<Things>();
       public static List<Things> init()
        {
            if (ts == null || ts.Count < 1)
            {
                ts.Add(new Things() { aa = "1", bb = "1", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "1", bb = "1", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "2", bb = "2", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "2", bb = "2", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "2", bb = "3", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "3", bb = "3", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "3", bb = "4", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "3", bb = "4", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "4", bb = "1", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "5", bb = "1", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "6", bb = "2", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "2", bb = "3", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "1", bb = "3", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "1", bb = "3", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
                ts.Add(new Things() { aa = "3", bb = "3", cc = "333", dd = "444", ee = "555", ff = "666", gg = "777" });
            }
            return ts;
        }

    }
}
