using System.Data;
using System.Data.SqlClient;

namespace T_Strore.Data;

public class BaseRepository
{
    public IDbConnection _connection;

    public BaseRepository(IDbConnection dbConnection)
    {
        _connection = dbConnection;
    }
    
    public IDbConnection Connection=> new SqlConnection(_connection.ConnectionString);
}
