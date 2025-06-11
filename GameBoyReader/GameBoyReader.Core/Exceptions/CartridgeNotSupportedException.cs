using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.Core.Exceptions
{
    public class CartridgeNotSupportedException: Exception
    {
        public override string Message => "This cartridge is unsupported by this device.";
    }
}
