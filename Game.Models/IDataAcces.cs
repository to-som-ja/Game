using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Models
{
    public interface IDataAcces
    {
        Task<List<T>> LoadData<T, U>(string sql, U parameters, string conectionString);
        Task SaveData<T>(string sql, T parameters, string conectionString);
    }
}