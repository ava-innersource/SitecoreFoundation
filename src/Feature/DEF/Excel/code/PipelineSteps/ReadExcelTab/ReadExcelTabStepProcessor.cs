using ExcelDataReader;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using System;
using System.Collections.Generic;
using System.IO;

namespace SF.Feature.DEF.Excel
{

    [RequiredEndpointPlugins(typeof(ExcelSettings))]
    public class ReadExcelTabStepProcessor : BaseReadDataStepProcessor
    {
        public ReadExcelTabStepProcessor()
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
            var settings = endpoint.GetExcelSettings();
            if (settings == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(settings.FileLocation))
            {
                logger.Error(
                    "No File Location is specified on the endpoint. " +
                    "(pipeline step: {0}, endpoint: {1})",
                    pipelineStep.Name, endpoint.Name);
                return;
            }

            var excelSettings = pipelineStep.GetPlugin<ReadExcelTabSettings>();
            if (string.IsNullOrWhiteSpace(excelSettings.Sheet))
            {
                logger.Error(
                    "No tab has been configured" +
                    "(pipeline step: {0}, endpoint: {1})",
                    pipelineStep.Name, endpoint.Name);
                return;
            }
           


            SynchronizationSettings synchronizationSettings = pipelineContext.GetSynchronizationSettings();
            Dictionary<string, string> source = null;
            if (synchronizationSettings != null)
            {
                source = synchronizationSettings.Source as Dictionary<string, string>;
            }


            try
            {
                ////add the data that was read from the file to a plugin
                var dataSettings = new IterableDataSettings(GetEnumerable(settings.FileLocation, excelSettings.Sheet, excelSettings.FirstRowHasColumnNames, logger));

                pipelineContext.Plugins.Add(dataSettings);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"ReadExcelTabStepProcessor: error occurred.pipelineStep: {pipelineStep.Name}, endpoint: {endpoint.Name}", ex);
                return;
            }
        }

        private IEnumerable<Dictionary<string, string>> GetEnumerable(string filePath, string sheetName, bool firstRowHasColumnNames, Sitecore.Services.Core.Diagnostics.ILogger logger)
        {
            FileInfo objFileInfo = new FileInfo(@"C:\Websites\btod.dev.local\Website\MobileDBExcel\Mobile CoE - BP Device Detail Report.xlsx");
            //using (var stream = new FileStream(objFileInfo.ToString(), FileMode.Open, FileAccess.Read))
            //using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))

            var stream = objFileInfo.Exists ? new FileStream(objFileInfo.ToString(), FileMode.Open, FileAccess.Read) : File.Open(filePath, FileMode.Open, FileAccess.Read);

            using (stream)
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        if (reader != null && reader.Name == sheetName)
                        {
                            var columns = new Dictionary<int, string>();

                            if (firstRowHasColumnNames)
                            {
                                reader.Read();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    columns.Add(i, reader[i].ToString());
                                }
                            }
                            var sourceValue = string.Empty;
                            while (reader.Read())
                            {
                                var record = new Dictionary<string, string>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (reader[i] != null)
                                        sourceValue = reader[i].ToString();
                                    else
                                        sourceValue = string.Empty;

                                    if (columns.ContainsKey(i))
                                    {
                                        record.Add(columns[i], sourceValue);
                                    }
                                    else
                                    {
                                        record.Add(reader.GetName(i), sourceValue);
                                    }
                                }

                                yield return record;
                            }
                        }
                    } while (reader.NextResult());

                }
            }
        }
    }
}
