using GameBoyReader.Core.Enums;
using GameBoyReader.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.Core.Services
{
    public class CartridgeByteContentService : ICartridgeByteContentService
    {
        public CartridgeType RetrieveCartridgeType(byte[] byteContent)
        {
            return CartridgeTypeConverter.ConvertFromByte(byteContent[0x147]);
        }

        public string RetrieveGameName(byte[] byteContent)
        {
            return Encoding.ASCII.GetString(byteContent, 0x134, 16);
        }
    }
}
