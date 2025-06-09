using GameBoyReader.Core.Enums;

namespace GameBoyReader.Core.Utils
{
    public static class CartridgeTypeConverter
    {
        public static CartridgeType ConvertFromByte(byte input)
        {
            return input switch
            {
                0x00 => CartridgeType.MBC0,
                0x01 => CartridgeType.MBC1,
                0x02 => CartridgeType.MBC1_RAM,
                0x03 => CartridgeType.MBC1_RAM_BATTERY,
                0x05 => CartridgeType.MBC2,
                0x06 => CartridgeType.MBC2_BATTERY,
                0x08 => CartridgeType.ROM_RAM,
                0x09 => CartridgeType.ROM_RAM_BATTERY,
                0x0B => CartridgeType.MMM01,
                0x0C => CartridgeType.MMM01_RAM,
                0x0D => CartridgeType.MMM01_RAM_BATTERY,
                0x0F => CartridgeType.MBC3_TIMER_BATTERY,
                0x10 => CartridgeType.MBC3_TIMER_RAM_BATTERY,
                0x11 => CartridgeType.MBC3,
                0x12 => CartridgeType.MBC3_RAM,
                0x13 => CartridgeType.MBC3_RAM_BATTERY,
                0x19 => CartridgeType.MBC5,
                0x1A => CartridgeType.MBC5_RAM,
                0x1B => CartridgeType.MBC5_RAM_BATTERY,
                0x1C => CartridgeType.MBC5_RUMBLE,
                0x1D => CartridgeType.MBC5_RUMBLE_RAM,
                0x1E => CartridgeType.MBC5_RUMBLE_RAM_BATTERY,
                0x20 => CartridgeType.MBC6,
                0x22 => CartridgeType.MBC7_SENSOR_RUMBLE_RAM_BATTERY,
                0xFC => CartridgeType.POCKET_CAMERA,
                0xFD => CartridgeType.BANDAI_TAMA5,
                0xFE => CartridgeType.HuC3,
                0xFF => CartridgeType.HuC1_RAM_BATTERY,
                _ => CartridgeType.Unsupported
            };
        }

        public static byte ConvertFromCartridgeType(CartridgeType input)
        {
            return input switch
            {
                CartridgeType.MBC0 => 0x00,
                CartridgeType.MBC1 => 0x01,
                CartridgeType.MBC1_RAM => 0x02,
                CartridgeType.MBC1_RAM_BATTERY => 0x03,
                CartridgeType.MBC2 => 0x05,
                CartridgeType.MBC2_BATTERY => 0x06,
                CartridgeType.ROM_RAM => 0x08,
                CartridgeType.ROM_RAM_BATTERY => 0x09,
                CartridgeType.MMM01 => 0x0B,
                CartridgeType.MMM01_RAM => 0x0C,
                CartridgeType.MMM01_RAM_BATTERY => 0x0D,
                CartridgeType.MBC3_TIMER_BATTERY => 0x0F,
                CartridgeType.MBC3_TIMER_RAM_BATTERY => 0x10,
                CartridgeType.MBC3 => 0x11,
                CartridgeType.MBC3_RAM => 0x12,
                CartridgeType.MBC3_RAM_BATTERY => 0x13,
                CartridgeType.MBC5 => 0x19,
                CartridgeType.MBC5_RAM => 0x1A,
                CartridgeType.MBC5_RAM_BATTERY => 0x1B,
                CartridgeType.MBC5_RUMBLE => 0x1C,
                CartridgeType.MBC5_RUMBLE_RAM => 0x1D,
                CartridgeType.MBC5_RUMBLE_RAM_BATTERY => 0x1E,
                CartridgeType.MBC6 => 0x20,
                CartridgeType.MBC7_SENSOR_RUMBLE_RAM_BATTERY => 0x22,
                CartridgeType.POCKET_CAMERA => 0xFC,
                CartridgeType.BANDAI_TAMA5 => 0xFD,
                CartridgeType.HuC3 => 0xFE,
                CartridgeType.HuC1_RAM_BATTERY => 0xFF,
                _ => 0xFF
            };
        }

        public static string ConvertFromCartridgeTypeToString(CartridgeType input)
        {
            return input switch
            {
                CartridgeType.MBC0 => "ROM ONLY",
                CartridgeType.MBC1 => "MBC1",
                CartridgeType.MBC1_RAM => "MBC1 + RAM",
                CartridgeType.MBC1_RAM_BATTERY => "MBC1 + RAM + BATTERY",
                CartridgeType.MBC2 => "MBC2",
                CartridgeType.MBC2_BATTERY => "MBC2 + BATTERY",
                CartridgeType.ROM_RAM => "ROM + RAM",
                CartridgeType.ROM_RAM_BATTERY => "ROM + RAM + BATTERY",
                CartridgeType.MMM01 => "MMM01",
                CartridgeType.MMM01_RAM => "MMM01 + RAM",
                CartridgeType.MMM01_RAM_BATTERY => "MMM01 + RAM + BATTERY",
                CartridgeType.MBC3_TIMER_BATTERY => "MBC3 + TIMER + BATTERY",
                CartridgeType.MBC3_TIMER_RAM_BATTERY => "MBC3 + TIMER + RAM + BATTERY",
                CartridgeType.MBC3 => "MBC3",
                CartridgeType.MBC3_RAM => "MBC3 + RAM",
                CartridgeType.MBC3_RAM_BATTERY => "MBC3 + RAM + BATTERY",
                CartridgeType.MBC5 => "MBC5",
                CartridgeType.MBC5_RAM => "MBC5 + RAM",
                CartridgeType.MBC5_RAM_BATTERY => "MBC5 + RAM + BATTERY",
                CartridgeType.MBC5_RUMBLE => "MBC5 + RUMBLE",
                CartridgeType.MBC5_RUMBLE_RAM => "MBC5 + RUMBLE + RAM",
                CartridgeType.MBC5_RUMBLE_RAM_BATTERY => "MBC5 + RUMBLE + RAM + BATTERY",
                CartridgeType.MBC6 => "MBC6",
                CartridgeType.MBC7_SENSOR_RUMBLE_RAM_BATTERY => "MBC7 + SENSOR + RUMBLE + RAM + BATTERY",
                CartridgeType.POCKET_CAMERA => "POCKET CAMERA",
                CartridgeType.BANDAI_TAMA5 => "BANDAI TAMA5",
                CartridgeType.HuC3 => "HuC3",
                CartridgeType.HuC1_RAM_BATTERY => "HuC1 + RAM + BATTERY",
                _ => "Unknown type"
            };
        }
    }

}
