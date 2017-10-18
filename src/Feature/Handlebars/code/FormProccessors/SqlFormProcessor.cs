using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Sitecore.Data.Fields;

namespace SF.Feature.Handlebars
{
    public class SqlFormProcessor : IFormProcessor
    {
        public void Process(Item processorItem, Item formConfiguration, HttpRequestBase request)
        {
            var connectionString = processorItem.Fields["Connection String"].Value;

            SqlConnection conn = new SqlConnection(connectionString);
            using (conn)
            {
                var opened = false;
                try
                {
                    conn.Open();
                    opened = true;

                    var commandText = processorItem.Fields["Query"].Value;
                    SqlCommand command = new SqlCommand(commandText, conn);

                    var fields = (NameValueListField)processorItem.Fields["Fields"];
                    foreach (var formKey in fields.NameValues.Keys)
                    {
                        command.Parameters.AddWithValue(fields.NameValues[formKey.ToString()], request.Form[formKey.ToString()]);
                    }

                    command.ExecuteNonQuery();
                }
                finally
                {
                    if (opened)
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}