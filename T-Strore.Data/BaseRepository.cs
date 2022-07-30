using System.Data;
using System.Data.SqlClient;

namespace T_Strore.Data
{
    public class BaseRepository
    {
        public IDbConnection ConString => new SqlConnection(@"Server=DESKTOP-U9ABOQU\SQLEXPRESS;Database=T-Store.DB;Trusted_Connection=True;");
    }
}
