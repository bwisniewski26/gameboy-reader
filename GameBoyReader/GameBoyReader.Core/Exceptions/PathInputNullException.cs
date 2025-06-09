using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.Core.Exceptions
{
    public class PathInputNullException: Exception
    {
        public override string Message => "Path to save dumped file is null. Launch this app again.";
    }
}
