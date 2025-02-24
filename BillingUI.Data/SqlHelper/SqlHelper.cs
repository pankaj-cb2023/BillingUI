using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Data.SqlHelper
{
    public static class SqlHelper
    {
        public static IEnumerable<T> GetRecords<T>(string sql, Func<IDataReader, T> transform, string connString, params SqlParameter[] parameters)
        {
            using var connection = new SqlConnection(connString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);

            var list = new List<T>();
            using (var reader = cmd.ExecuteReader(CommandBehavior.SingleResult))
            {
                while (reader.Read())
                    list.Add(transform.Invoke(reader));
            }
            return list;
        }

    }
}
