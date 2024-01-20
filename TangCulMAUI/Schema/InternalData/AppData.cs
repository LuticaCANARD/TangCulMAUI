using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangCulMAUI.Schema.InternalData
{
    class AppData
    {
        private AppData() { }
        private static readonly Lazy<AppData> _instance = new(() => { return new AppData(); });
        public static AppData Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        public List<Person> PersonData = [];

        public string SavePath = "";
        public string SettingPath = "";
        public string SqlPath =Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sql");

    }
}
