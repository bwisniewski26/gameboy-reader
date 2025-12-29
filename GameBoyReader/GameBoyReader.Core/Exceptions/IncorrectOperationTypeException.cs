using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.Core.Exceptions
{
    public class IncorrectOperationTypeException: Exception
    {
        public override string Message => "This operation is not supported by the device.";
    }
}
