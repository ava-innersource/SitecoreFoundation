using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Foundation.Resources
{
    public interface ICSSProcessor
    {
        string Process(string input);
    }
}
