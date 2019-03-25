using System.Data.SqlClient;

namespace Ziri.DAL
{
    public class EFContextBase : DBContextBase
    {
        protected EFContextBase(string connectionString) : base(new SqlConnection(connectionString)) { }
    }
}
