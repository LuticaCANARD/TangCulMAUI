using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangCulMAUI.Schema.InternalData;

namespace TangCulMAUI.Schema.DB
{
    class DataLoader
    {
        private DataLoader() { }
        private static readonly Lazy<DataLoader> _instance = new(() => { return new DataLoader(); });
        public static DataLoader Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public static void SavePersonDataToSQLite(Person person)
        {
            // 직렬화후 저장하는 함수이다.

            
        }

    }
}
