using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.Core.Exceptions
{
    public class SerialConnectionException: Exception
    {
        public override string Message => "Serial connection with device has failed. Try launching this app again.";
    }
}
