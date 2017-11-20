using Sitecore.DataExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DEF.Feature.File
{

    public static class FileSystemEndpointExtensions
    {
        public static FileSystemSettings GetFileSystemSettings(this Endpoint endpoint)
        {
            return endpoint.GetPlugin<FileSystemSettings>();
        }
        public static bool HasFileSystemSettings(this Endpoint endpoint)
        {
            return (GetFileSystemSettings(endpoint) != null);
        }
    }
}
