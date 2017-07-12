using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.PipelineSteps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DXF.Feature.File
{

    [RequiredEndpointPlugins(typeof(FileSystemSettings))]
    public class MoveFilesStepProcessor : BaseReadDataStepProcessor
    {
        public MoveFilesStepProcessor()
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
            if (!System.IO.File.Exists(settings.Path) && !Directory.Exists(settings.Path))
            {
                logger.Error(
                    "The path specified on the endpoint does not exist. " +
                    "(pipeline step: {0}, endpoint: {1}, path: {2})",
                    pipelineStep.Name, endpoint.Name, settings.Path);
                return;
            }

            var moveSettings = pipelineStep.GetPlugin<MoveFileSettings>();
            if (moveSettings == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(moveSettings.DestinationDirectory))
            {
                //logger.Error(
                //    "The path specified on the endpoint does not exist. " +
                //    "(pipeline step: {0}, endpoint: {1}, path: {2})",
                //    pipelineStep.Name, endpoint.Name, settings.Path);
                return;
            }

            //TODO the copy or move and renaming 

            //logger.Info(
            //    "{0} rows were read from the file. (pipeline step: {1}, endpoint: {2})",
            //    lines.Count, pipelineStep.Name, endpoint.Name);
            
        }
    }
}
