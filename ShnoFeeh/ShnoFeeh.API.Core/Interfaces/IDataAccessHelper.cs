using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface IDataAccessHelper
    {
        Task<int> RunAsync();
        Task<int> RunAsync(DataTable dataTable);
        Task<int> RunAsync(DataSet dataSet);
        void CommandWithoutParams(string sprocName);
        void CommandWithParams(string sprocName, SqlParameter[] parameters);
        void Dispose();
    }
}
