using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SF.DXF.Feature.Database
{

    [RequiredEndpointPlugins(typeof(DatabaseSettings))]
    public class QueryDatabaseStepProcessor : BaseReadDataStepProcessor
    {
        public QueryDatabaseStepProcessor()
        {
        }

        protected override void ReadData(
            Endpoint endpoint,
            PipelineStep pipelineStep,
            PipelineContext pipelineContext)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }
            if (pipelineStep == null)
            {
                throw new ArgumentNullException("pipelinestep");
            }
            if (pipelineContext == null)
            {
                throw new ArgumentNullException("pipelineContext");
            }
            var logger = pipelineContext.PipelineBatchContext.Logger;
            //
            //get the file path from the plugin on the endpoint
            var settings = endpoint.GetDatabaseSettings();
            if (settings == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                logger.Error(
                    "No connection string is specified on the endpoint. " +
                    "(pipeline step: {0}, endpoint: {1})",
                    pipelineStep.Name, endpoint.Name);
                return;
            }

            var querySettings = pipelineStep.GetPlugin<QuerySettings>();
            if (string.IsNullOrWhiteSpace(querySettings.Query))
            {
                logger.Error(
                    "No query has been configured" +
                    "(pipeline step: {0}, endpoint: {1})",
                    pipelineStep.Name, endpoint.Name);
                return;
            }


            ////add the data that was read from the file to a plugin
            var dataSettings = new IterableDataSettings(GetEnumerable(settings.ConnectionType, settings.ConnectionString, querySettings.Query, logger));
            pipelineContext.Plugins.Add(dataSettings);
        }

        private IEnumerable<Dictionary<string, string>> GetEnumerable(string connectionType, string connectionString, string query, Sitecore.Services.Core.Diagnostics.ILogger logger)
        {
            using (var conn = DBConnectionFactory.GetConnection(connectionType, connectionString))
            {
                if (conn == null)
                {
                    logger.Error(
                        "Database Type is invalid: {0} ",
                        connectionType);
                    yield break;
                }

                //Need to test conn and query in try catch so we can properly report errors
                //yield return cannot be in a try catch.
                IDbCommand cmd = null;
                IDataReader reader = null;
                try
                {
                    cmd = conn.CreateCommand();
                    conn.Open();
                    cmd.CommandText = query;
                    reader = cmd.ExecuteReader();
                }
                catch (Exception ex)
                {
                    logger.Error("Could not run query: {0}. Exception: {1} ",
                       query, ex);

                    //force connection closed.
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    yield break;

                }

                bool wasLogged = false;
                using (cmd)
                {
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, string> record = new Dictionary<string, string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var fieldName = reader.GetName(i);
                                if (!record.ContainsKey(fieldName))
                                {
                                    record.Add(fieldName, reader[i].ToString());
                                }
                                else
                                {
                                    if (!wasLogged)
                                    {
                                        logger.Warn(
                                            "dataset has duplicate field names: {0} ",
                                            fieldName);
                                        wasLogged = true;
                                    }

                                }
                            }
                            yield return record;
                        }

                        if (reader.NextResult())
                        {
                            logger.Warn("Dataset has multiple result sets. Ignoring the rest.");
                            wasLogged = true;
                        }

                        reader.Close();

                    }
                }

            }

        }
    }
}
