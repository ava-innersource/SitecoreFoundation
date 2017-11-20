using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DEF.Feature.File
{


    [RequiredEndpointPlugins(typeof(FileSystemSettings))]
    public class ReadSystemFilesStepProcessor : BaseReadDataStepProcessor
    {
        public ReadSystemFilesStepProcessor()
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
                throw new ArgumentNullException("pipelineStep");
            }
            if (pipelineContext == null)
            {
                throw new ArgumentNullException("pipelineContext");
            }
            var logger = pipelineContext.PipelineBatchContext.Logger;
            //
            //get the file path from the plugin on the endpoint
            var settings = endpoint.GetFileSystemSettings();
            if (settings == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(settings.Path))
            {
                logger.Error(
                    "No path is specified on the endpoint. " +
                    "(pipeline step: {0}, endpoint: {1})",
                    pipelineStep.Name, endpoint.Name);
                return;
            }
            
            //not a file or directory
            if (!System.IO.File.Exists(settings.Path) && !Directory.Exists(settings.Path ))
            {
                logger.Error(
                    "The path specified on the endpoint does not exist. " +
                    "(pipeline step: {0}, endpoint: {1}, path: {2})",
                    pipelineStep.Name, endpoint.Name, settings.Path);
                return;
            }


            var dataSettings = new IterableDataSettings(GetEnumerable(settings, logger));
            logger.Info(
                "path: {0} was processed.. (pipeline step: {1}, endpoint: {2})",
                settings.Path, pipelineStep.Name, endpoint.Name);
            //
            //add the plugin to the pipeline context
            pipelineContext.Plugins.Add(dataSettings);
        }

        private IEnumerable<Dictionary<string, string>> GetEnumerable(FileSystemSettings settings, Sitecore.Services.Core.Diagnostics.ILogger logger)
        {
            if (Directory.Exists(settings.Path))
            {
                foreach (string filePath in Directory.EnumerateFiles(settings.Path))
                {

                    foreach (Dictionary<string, string> line in GetFileContents(filePath, settings))
                    {
                        yield return line;
                    }

                    logger.Info("file: {0} was processed.", filePath);
                }
            }
            else
            {
                foreach (Dictionary<string, string> line in GetFileContents(settings.Path, settings))
                {
                    yield return line;
                }
            }            
        }

        private IEnumerable<Dictionary<string, string>> GetFileContents(string filePath, FileSystemSettings settings)
        {
            //read the file, one line at a time
            var separator = new string[] { settings.ColumnSeparator };
            string[] keys = null;

            using (var reader = new StreamReader(System.IO.File.OpenRead(filePath)))
            {
                var firstLine = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (firstLine && settings.ColumnHeadersInFirstLine)
                    {
                        firstLine = false;
                        keys = line.Split(separator, StringSplitOptions.None);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    
                    var values = line.Split(separator, StringSplitOptions.None);
                    var record = new Dictionary<string, string>();
                    int colIndex = 1;
                    foreach(string value in values)
                    {
                        if (keys != null)
                        {
                            record.Add(keys[colIndex - 1].Trim(), value.Trim());
                        }
                        else
                        {
                            record.Add(colIndex.ToString(), value.Trim());
                        }
                        colIndex++;
                    }
                    yield return record;
                }
            }
            
        }
    }
}
