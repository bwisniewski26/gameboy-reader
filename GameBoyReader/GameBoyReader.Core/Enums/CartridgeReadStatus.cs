using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.Core.Enums
{
    public enum CartridgeReadStatus
    {
        Idle,
        InProgress,
        Complete,
        Error
    }
}
