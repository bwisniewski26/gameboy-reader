using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyReader.Core.Models
{
    public class CartridgeContent
    {
        public CartridgeInformation CartridgeInformation { get; set; }
        public Byte[] CartridgeByteContent { get; set; }

        public CartridgeContent(CartridgeInformation cartridgeInformation, Byte[] byteContent) 
        {
            CartridgeInformation = cartridgeInformation;
            CartridgeByteContent = byteContent;
        }
    }
}
