#define RD_PIN 10
#define WR_PIN 12
#define CS_PIN 9
#define RST_PIN 6
#define CLK_PIN 13

int dumpComplete = 0;
int input;
int kB_dumped = 0;
int headerBytesDumped = 0;


void setup()
{
    Serial.begin(115200);
    pinMode(RD_PIN, OUTPUT);
    pinMode(WR_PIN, OUTPUT);
    pinMode(CS_PIN, OUTPUT);
    pinMode(RST_PIN, OUTPUT);
    pinMode(CLK_PIN, OUTPUT);

    DDRL = 0x00;
    DDRA = 0xFF; 
    DDRC = 0xFF;

    digitalWrite(RST_PIN, LOW);
    delay(50);
    digitalWrite(RST_PIN, HIGH);
    delay(50);


    digitalWrite(RD_PIN, HIGH);
    digitalWrite(WR_PIN, HIGH);
    digitalWrite(CS_PIN, HIGH);
    digitalWrite(RST_PIN, HIGH);
    digitalWrite(CLK_PIN, HIGH);

}

void setAddress(uint16_t address)
{
    PORTA = address & 0xFF;        // First part of data at the address
    PORTC = (address >> 8) & 0xFF; // Second part of data at the address
}

uint8_t readByte(uint16_t address)
{
    setAddress(address);
    digitalWrite(CS_PIN, LOW);
    digitalWrite(RD_PIN, LOW);
    delayMicroseconds(15);
    byte data = PINL;
    digitalWrite(RD_PIN, HIGH);
    digitalWrite(CS_PIN, HIGH);
    return data;
}

void writeByte(uint8_t data, uint16_t address)
{
  digitalWrite(RD_PIN, HIGH);
  digitalWrite(CS_PIN, LOW); 

  setAddress(address);  

  DDRL = 0xFF;       
  PORTL = data;       
  delayMicroseconds(1); 

  digitalWrite(WR_PIN, LOW);  
  delayMicroseconds(2);       
  digitalWrite(WR_PIN, HIGH);

  digitalWrite(CS_PIN, HIGH);
  
  DDRL = 0x00;      
}


void dumpROM(uint32_t size, uint32_t startAddress = 0)
{
    for (uint32_t addr = startAddress; addr < startAddress + size; addr++)
    {
        uint8_t data = readByte(addr);
        Serial.write(data); 
        if (addr % 2048 == 0)
        {
          kB_dumped += 2;
            Serial.flush();                               
        }
    }
}

void dumpHeader() {

    Serial.println("START");
    for (uint16_t addr = 0x0104; addr <= 0x0133; addr++) {
        uint8_t value = readByte(addr);
        Serial.write(value);
    }
    Serial.flush();
    Serial.println("END");
}


void dumpMBCType()
{
    Serial.println("START");
    uint8_t value = getMBCType();
    Serial.write(value);
    Serial.println("END");
}

void dumpROMSize()
{
    Serial.println("START");
    uint8_t value = getROMSize();
    Serial.write(value);
    Serial.println("END");
}

void dumpRAMSize()
{
    Serial.println("START");
    uint8_t value = getRAMSize();
    Serial.write(value);
    Serial.println("END");
}

void dumpTitle()
{
    Serial.println("START");

    char title[17];
    getTitle(title);

    for (int i = 0; i < 16; i++) {
        if (title[i] == '\0') break;
        Serial.write(title[i]);
    }

    Serial.println("END");
}

void getTitle(char* buffer)
{
    for (int i = 0; i < 16; i++) {
        uint8_t byte = readByte(0x0134 + i);
        if (byte == 0x00) {
            buffer[i] = '\0';
            return;
        }
        buffer[i] = static_cast<char>(byte);
    }
    buffer[16] = '\0';
}


uint8_t getMBCType()
{
  uint8_t value = readByte(0x0147);
  return value;
}

uint8_t getROMSize()
{
  uint8_t value = readByte(0x0148);
  return value;
}

uint8_t getRAMSize()
{
  uint8_t value = readByte(0x0149);
  return value;
}

uint32_t calculateROMAddressCount()
{
  uint8_t value = readByte(0x0148);

  uint32_t romSize = 0;

  switch (value)
  {
    case 0: romSize = (uint32_t)32 * (uint32_t)1024; break;
    case 1: romSize = (uint32_t)64 * (uint32_t)1024; break;
    case 2: romSize = (uint32_t)128 * (uint32_t)1024; break;
    case 3: romSize = (uint32_t)256 * (uint32_t)1024; break;
    case 4: romSize = (uint32_t)512 * (uint32_t)1024; break;
    case 5: romSize = (uint32_t)1024 * (uint32_t)1024; break;
    case 6: romSize = (uint32_t)2 * (uint32_t)1024 * (uint32_t)1024; break;
    case 7: romSize = (uint32_t)4 * (uint32_t)1024 * (uint32_t)1024; break;
    case 8: romSize = (uint32_t)8 * (uint32_t)1024 * (uint32_t)1024; break;
    case 0x52: romSize = (uint32_t)1152 * (uint32_t)1024; break;
    case 0x53: romSize = (uint32_t)1280 * (uint32_t)1024; break;
    case 0x54: romSize = (uint32_t)1536 * (uint32_t)1024; break;
    default:
      romSize = 0;
  }

  return romSize;
}

void dumpROMWithBanks(uint32_t romSize, uint8_t mbcType)
{
    uint32_t bankCount = romSize / 16384;
    bool useBankSwitch = false;
    uint16_t (*switchBank)(uint16_t) = nullptr;
    uint16_t startAddr = 0x0000;

    switch(mbcType)
    {
        case 0x00: case 0x08: case 0x09: // MBC0
            bankCount = 1;
            break;
        case 0x01: case 0x02: case 0x03: // MBC1
            useBankSwitch = true;
            switchBank = switchMBC1Bank;
            startAddr = 0x4000;
            break;
        case 0x05: case 0x06: // MBC2
            useBankSwitch = true;
            switchBank = switchMBC2Bank;
            startAddr = 0x4000;        
            if(bankCount > 16) bankCount = 16;
            break;
        case 0x0F: case 0x10: case 0x11: case 0x12: case 0x13: // MBC3
            useBankSwitch = true;
            switchBank = switchMBC3Bank;
            startAddr = 0x4000;
            break;
        case 0x19: case 0x1A: case 0x1B: case 0x1C: case 0x1D: case 0x1E: // MBC5
            useBankSwitch = true;
            switchBank = switchMBC5Bank;
            startAddr = 0x4000;
            break;
        default:
            Serial.println("Unsupported MBC");
            return;
    }

    Serial.println("START");
    Serial.flush();
    kB_dumped = 0;

    dumpROM(0x4000, 0x0000);

    if(useBankSwitch)
    {
        if (mbcType == 0x05 || mbcType == 0x06) { // MBC2
          for (uint16_t bankNumber = 1; bankNumber <= 0x0F && bankNumber < bankCount; bankNumber++)
          {
            switchBank(bankNumber);
            dumpROM(0x4000, 0x4000);
          }
        } else { // other MBCs
          for (uint16_t bankNumber = 1; bankNumber < bankCount; bankNumber++)
          {
            switchBank(bankNumber);
            dumpROM(0x4000, startAddr);
          }
        }
    } else { // no MBC present
        dumpROM(0x4000, 0x4000);
    }

    Serial.println("END");
    Serial.flush();
}


uint16_t switchMBC1Bank(uint16_t bank) { if(bank==0) bank=1; writeByte(bank, 0x2000); delay(5); return bank; }
uint16_t switchMBC2Bank(uint16_t bank) { if(bank==0) bank=1; bank &= 0x1F; writeByte(bank, 0x2100); delay(20); return bank; }
uint16_t switchMBC3Bank(uint16_t bank) { if(bank==0) bank=1; writeByte(bank, 0x2000); delay(5); return bank; }
uint16_t switchMBC5Bank(uint16_t bank)
{
    writeByte(bank & 0xFF, 0x2000);
    writeByte((bank >> 8) & 0x01, 0x3000);
    delay(5);
    return bank;
}

void dumpRAM() {
    uint8_t mbcType = getMBCType();
    uint8_t ramSize = getRAMSize();
    uint32_t ramBankCount = 0;
    uint32_t ramSizeBytes = 0;
    uint16_t ramBankSize = 8192;

    Serial.println("START");
    Serial.flush();

    // MBC2 RAM compatiblity
    if (mbcType == 0x05 || mbcType == 0x06) {
      dumpMBC2RAM();
      return;
    }

    switch(ramSize) {
        case 0:
            ramBankCount = 0;
            ramSizeBytes = 0;
            break;
        case 1: // 2KB RAM
            ramBankCount = 1;
            ramSizeBytes = 2048;
            ramBankSize = 2048;
            break;
        case 2: // 8KB RAM
            ramBankCount = 1;
            ramSizeBytes = 8192;
            break;
        case 3: // 32KB RAM 
            ramBankCount = 4;
            ramSizeBytes = 32768;
            break;
        case 4: // 128KB RAM
            ramBankCount = 16;
            ramSizeBytes = 131072;
            break;
        case 5: // 64KB RAM 
            ramBankCount = 8;
            ramSizeBytes = 65536;
            break;
        default:
            ramBankCount = 0;
            ramSizeBytes = 0;
            break;
    }
    int cartridgeRAMPresent;
    switch (mbcType)
    {
      case 0x02:
      case 0x03:
      case 0x05:
      case 0x06:
      case 0x08:
      case 0x09:
      case 0x0C:
      case 0x0D:
      case 0x0F:
      case 0x10:
      case 0x11:
      case 0x12:
      case 0x13:
      case 0x17:
      case 0x19:
      case 0x1B:
      case 0x1E:
      case 0x22:
      case 0xFD:
      case 0xFF:
        cartridgeRAMPresent = 1;
        break;
      default:
        cartridgeRAMPresent = 0;

    }
    if (cartridgeRAMPresent == 1) { 
        if (mbcType == 0x05 || mbcType == 0x06) {
             for (uint16_t addr = 0xA000; addr <= 0xA1FF; addr++) {
                 uint8_t data = readByte(addr);
                 Serial.write(data);
             }
        } else {
            writeByte(0x0A, 0x0000);
            delay(50);
            
            for (uint32_t bankNumber = 0; bankNumber < ramBankCount; bankNumber++) {
                writeByte(bankNumber, 0x4000);
                
                for (uint16_t addr = 0xA000; addr <= 0xBFFF; addr++) {
                    uint8_t data = readByte(addr);
                    Serial.write(data);
                }
            }
        }
    }

    Serial.println("END");
    Serial.flush();
}

void dumpMBC2RAM()
{
  writeByte(0x0A, 0x0000);
  delayMicroseconds(50);

  for (uint16_t addr = 0xA000; addr <= 0xA1FF; addr++) {
    uint8_t data = readByte(addr);
    data &= 0x0F;
    Serial.write(data);
  }
  Serial.println("END");
  Serial.flush();
}

void writeMBC2RAM()
{
  writeByte(0x0A, 0x0000);
  delayMicroseconds(50);

  while (!Serial.find("START")) { ; }

    uint32_t address = 0;
    uint8_t currentBank = 0xFF;

    while (address < 512) {
        if (Serial.available()) {
            uint8_t data = Serial.read() & 0x0F;
            writeByte(data, 0xA000 + (address & 0x01FF));
            address++;
        }
    }
    while (!Serial.find("END")) { ; }
}

int ValidCheckSum()
{
  int checksum = 0;

  for (int j = 0x134; j < 0x14E; j++)
  {
      checksum += readByte(j);
  }

  if (((checksum + 25) & 0xFF) == 0)
  {
    return 1;
  }
  return 0;
}

void saveRAMToCart() {
    uint8_t mbcType = getMBCType();
    uint8_t ramSize = getRAMSize();
    uint32_t ramBankCount = 0;
    uint16_t ramBankSize = 8192; 
    uint32_t ramSizeBytes = 0;

    if (mbcType == 0x05 || mbcType == 0x06)
    {
      writeMBC2RAM();
    }

    switch(ramSize) {
        case 0: ramBankCount = 0; ramSizeBytes = 0; break;
        case 1: ramBankCount = 1; ramSizeBytes = 2048; ramBankSize = 2048; break;
        case 2: ramBankCount = 1; ramSizeBytes = 8192; break;
        case 3: ramBankCount = 4; ramSizeBytes = 32768; break;
        case 4: ramBankCount = 16; ramSizeBytes = 131072; break;
        case 5: ramBankCount = 8; ramSizeBytes = 65536; break;
        default: ramBankCount = 0; ramSizeBytes = 0; break;
    }

    if (ramBankCount == 0) return;

    int cartridgeBattery;
    switch (mbcType) {
      case 0x03: case 0x06: case 0x09: case 0x0D:
      case 0x0F: case 0x10: case 0x13: case 0x17:
      case 0x1B: case 0x1E: case 0x22: case 0xFD:
      case 0xFF:
        cartridgeBattery = 1; break;
      default:
        cartridgeBattery = 0;
    }

    if (cartridgeBattery) {
        writeByte(0x0A, 0x0000);
        delayMicroseconds(50);
    }

    while (!Serial.find("START")) { ; }

    uint32_t address = 0;
    uint8_t currentBank = 0xFF;

    while (address < ramSizeBytes) {
        if (Serial.available()) {
            uint8_t data = Serial.read();

            if ((cartridgeBattery == 1) && ramBankCount > 1) {
                uint8_t bank = address / ramBankSize;
                if (bank != currentBank) {
                    writeByte(bank, 0x4000);
                    currentBank = bank;
                }
            }

            if (mbcType == 0x02) data &= 0x0F;

            writeByte(data, 0xA000 + (address % ramBankSize));
            address++;
        }
    }

    while (!Serial.find("END")) { ; }
}

void loop()
{

    if (Serial.available()) {
      String command = Serial.readStringUntil('\n');
      Serial.println(command);
      if (command == "GET_HEADER") {
          dumpHeader();
      }
      else if (command == "GET_MBC")
      {
        dumpMBCType();
      }
      else if (command == "GET_ROM_SIZE")
      {
        dumpROMSize();
      }
      else if (command == "GET_RAM_SIZE")
      {
        dumpRAMSize();
      }
      else if (command == "GET_TITLE")
      {
        dumpTitle();
      }
      else if (command == "DUMP_ROM") {
        dumpROMWithBanks(calculateROMAddressCount(), getMBCType());
      } else if (command == "DUMP_RAM")
      {
        dumpRAM();
      } else if (command == "WRITE_RAM") {
        saveRAMToCart();
      } else if (command == "CHECKSUM_VERIFY")
      {
        Serial.println("START");
        Serial.flush();
        Serial.println(ValidCheckSum());
        Serial.println("END");
        Serial.flush();
      } else if (command == "PING")
      {
        Serial.println("START");
        Serial.flush();
        Serial.print("PONG");
        Serial.println("END");
        Serial.flush();
      }
    } 
}