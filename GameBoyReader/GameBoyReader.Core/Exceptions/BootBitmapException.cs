using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.Core.Exceptions
{
    internal class BootBitmapException: Exception
    {
        public override string Message => "Retrieved verification data was incorrect. Clean cartridge contacts and try inserting it again.";
    }
}
