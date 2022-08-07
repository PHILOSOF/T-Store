using System.Data;
using System.Data.SqlClient;

namespace T_Strore.Data;

public class BaseRepositories
{
    public IDbConnection _connection;

    public BaseRepositories(IDbConnection dbConnection)
    {
        _connection = dbConnection;
    }
    
    public IDbConnection Connection=> new SqlConnection(_connection.ConnectionString);
}
