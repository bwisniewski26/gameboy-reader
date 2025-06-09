﻿using GameBoyReader.Core.Enums;
using GameBoyReader.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.Core.Services
{
    public interface ICartridgeDumperService
    {
        public Task<CartridgeContent> DumpCartridge(string comPort);
    }
}
