using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Infrastructure.Logging
{
    public class LoggerAdapter : IAppLogger
    {
        internal IDataAccessHelper _dataHelper = null;
        //  private readonly ILogger<T> _logger;
        public LoggerAdapter(IDataAccessHelper dataHelper)
        {
            _dataHelper = dataHelper;
        }
        public LoggerAdapter()
        {
            // _logger = loggerFactory.CreateLogger<T>();
        }
        public void LogWarning(string message, params object[] args)
        {
            //  _logger.LogWarning(message, args);
        }

        public async Task<int> LogInformationAsync(ExceptionLogDto exLog)
        {

            SqlParameter[] parameters =
        {
                    new SqlParameter( "@ExceptionType",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionInnerException",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionMessage",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionSeverity",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionFileName",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionLineNumber",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionColumnNumber",  SqlDbType.NVarChar ),
                    new SqlParameter( "@ExceptionMethodName",  SqlDbType.NVarChar )
                };

            parameters[0].Value = exLog.ExceptionType;
            parameters[1].Value = exLog.ExceptionInnerException;
            parameters[2].Value = exLog.ExceptionMessage;
            parameters[3].Value = exLog.ExceptionSeverity;
            parameters[4].Value = exLog.ExceptionFileName;
            parameters[5].Value = exLog.ExceptionLineNumber;
            parameters[6].Value = exLog.ExceptionColumnNumber;
            parameters[7].Value = exLog.ExceptionMethodName;
            _dataHelper.CommandWithParams("spi_exceptionapi", parameters);

            DataTable dt = new DataTable();

            int result = await _dataHelper.RunAsync(dt);
            return result;
        }
    }
}
