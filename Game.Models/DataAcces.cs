using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Models
{
    public class DataAcces : IDataAcces
    {
        public async Task<List<T>> LoadData<T, U>(string sql, U parameters, string conectionString)
        {
            using (IDbConnection connection = new SqlConnection(conectionString))
            {
                connection.Open();
                var rows = await connection.QueryAsync<T>(sql, parameters);
                return rows.ToList();
            }
        }
        public Task SaveData<T>(string sql, T parameters, string conectionString)
        {
            using (IDbConnection connection = new SqlConnection(conectionString))
            {
                return connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
