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

        public bool CalculateHeaderChecksum(List<byte> byteContent)
        {
            if (byteContent == null || byteContent.Count <= 0x014D)
                return false;

            int checksum = 0;

            for (int address = 0x0134; address <= 0x014C; address++)
            {
                checksum = checksum - byteContent[address] - 1;
            }

            byte calculatedChecksum = (byte)checksum;
            byte headerChecksum = byteContent[0x014D];

            return calculatedChecksum == headerChecksum;
        }
    }
}
