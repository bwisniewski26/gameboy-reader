import serial

PORT = "COM6" # Connection port with Arduino
BAUDRATE = 115200 # Baudrate for serial connection
OUTPUT_FILE = "test.gb" # Output file name
ROM_SIZE = 0x8000  # Temporary hardcoded value for MBC0 cartridges

def dump_rom():
    with serial.Serial(PORT, BAUDRATE, timeout=1) as ser:
        print("Awaiting START DUMP signal from dumper...")

        while True:
            line = ser.readline().decode(errors="ignore").strip()
            if line == "START DUMP":
                print("Dumping cartridge!")
                break
        
        with open(OUTPUT_FILE, "wb") as f:
            bytes_read = 0
            buffer = bytearray()
            
            while bytes_read < ROM_SIZE:
                if bytes_read % 1024 == 0:
                    print(f"Dumped {bytes_read / 1024} KB...")
                data = ser.read(1)  
                if not data:
                    print("Timeout - no data!")
                    break
                
                buffer.extend(data)
                bytes_read += 1
                
                # Checking for "END DUMP" signal
                if bytes_read >= 8 and buffer[-8:] == b"END DUMP":
                    f.write(buffer[:-8])  # Writing all data except "END DUMP"
                    print("Dump finished!")
                    break
                
                # Writing data to file every 4096 bytes
                if bytes_read % 4096 == 0:
                    f.write(buffer)
                    buffer = bytearray()
            
            # Writing remaining data to file
            if buffer:
                f.write(buffer)
        
        print(f"ROM saved as {OUTPUT_FILE}, size: {bytes_read - 8 if bytes_read >= 8 else bytes_read} bytes")

if __name__ == "__main__":
    dump_rom()