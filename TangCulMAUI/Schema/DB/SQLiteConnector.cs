using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TangCulMAUI.Schema.DB
{
    class SQLiteConnector
    {
        SQLiteConnection Database;
        async Task Init()
        {
            if (Database is not null)
                return;

            //Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            //var result = await Database.CreateTableAsync<TodoItem>();
        }

    }
}
