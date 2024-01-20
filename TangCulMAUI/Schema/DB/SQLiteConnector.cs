using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using TangCulMAUI.Schema.InternalData;

namespace TangCulMAUI.Schema.DB
{
    public class DBPersonData
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int Age { get; set; }

        public string? Name { get; set; }
        public PersonStatus Status { get; set; }

        public string? Traits { get; set; }
        public string? Agent { get; set; }
        public static explicit operator Person(DBPersonData data)
        {

            return new Person(data.Name, data.Age, data.Traits?.Split('\t'), data.Status, data.Agent);
        }
    }
    public class SQLiteConnector
    {
        SQLiteAsyncConnection Database = null;
        async Task Init()
        {
            if (Database is not null)
                return;

            if(!Directory.Exists(AppData.Instance.SqlPath))
                Directory.CreateDirectory(AppData.Instance.SqlPath);
            string databaseFileName = Path.Combine(AppData.Instance.SqlPath, "sql.db");
            Database = new SQLiteAsyncConnection(databaseFileName,storeDateTimeAsTicks:true);
            CreateTableResult ret =  await Database.CreateTableAsync<DBPersonData>(CreateFlags.AllImplicit).ConfigureAwait(false);

            return;

        }
        private SQLiteConnector() { }
        private static readonly Lazy<SQLiteConnector> _instance = new(() => {
            return new SQLiteConnector();

        });
        public static SQLiteConnector Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void SavePersonToDB(Person person)
        {
            DBPersonData data = (DBPersonData)person;
            Task task = Database == null ? Init() : Database.InsertAsync(data);
            if (Database == null)
            {
                task.Wait();
                if( Database != null )
                    task = Database.InsertAsync(data);
            }
            task.Wait();
        }
        public List<Person> LoadPersonList()
        {
            var list = new List<Person>();
            if(Database == null)
            {
                 Init().Wait();

            }
            Task<List<DBPersonData>> query = Database.Table<DBPersonData>().ToListAsync();
            foreach(DBPersonData data in query.Result){
                list.Add((Person)data);
            }
            return list;
        }
    }
}
