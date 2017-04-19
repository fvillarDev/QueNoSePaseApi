using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;

namespace QueNoSePase.API.Models
{
    public class DataHelpercs
    {
        public class DataHelper
        {
            private static string ConnectionString = "Server=59ae8913-569b-4e6e-bc6a-a609000e4452.sqlserver.sequelizer.com;Database=db59ae8913569b4e6ebc6aa609000e4452;User ID=owfcbeesatwrggwf;Password=MsaUsdzdRoGQS8GRVXxPLcX7FUdM7p3oKPnDXmvEAkwWKAaamk4AYmHJBbnF8VfA;";

            private static SqlCommand DataCommand(string cmd, SqlConnection connection)
            {
                return new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 60,
                    Connection = connection,
                    CommandText = cmd
                };
            }

            public static void ExecuteNonQuery(string cmd, params object[] parameters)
            {
                try
                {
                    using (var scope = new TransactionScope())
                    {
                        using (var connection = new SqlConnection(ConnectionString))
                        {
                            var sqlCommand = DataCommand(cmd, connection);
                            connection.Open();
                            SqlCommandBuilder.DeriveParameters(sqlCommand);
                            if (parameters != null)
                                if (parameters.Length > 0)
                                {
                                    var index = 0;
                                    foreach (SqlParameter parameter in sqlCommand.Parameters)
                                    {
                                        if (parameter.Direction == ParameterDirection.Input ||
                                            parameter.Direction == ParameterDirection.
                                                InputOutput)
                                        {
                                            parameter.Value = parameters[index];
                                            index++;
                                        }
                                    }
                                }
                            sqlCommand.ExecuteNonQuery();
                            connection.Close();
                        }
                        scope.Complete();
                    }
                }
                catch (SqlException ex)
                {
                    //LogHelper.Log(ex);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}