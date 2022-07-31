using System.Data;
using System.Data.SqlClient;

namespace T_Strore.Data;

public class BaseRepository
{
    public IDbConnection ConString => new SqlConnection(@"Server=.\SQLEXPRESS;Database=T-Store.DB;Trusted_Connection=True;");
}
