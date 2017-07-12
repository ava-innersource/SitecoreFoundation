using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DXF.Feature.File
{

    public class MoveFilesStepItemModel : ItemModel
    {
        public const string EndpointFrom = "EndpointFrom";

        public const string DestinationDirectory = "DestinationDirectory";
        public const string Copy = "Copy";
        public const string AppendTimeStamp = "AppendTimeStamp";
    }
}
