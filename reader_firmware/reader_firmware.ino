#define LED_PIN 7
#define RD_PIN 10
#define WR_PIN 12
#define CS_PIN 9
#define RST_PIN 6
#define CLK_PIN 13

#include "LiquidCrystal_I2C.h"
#include  "Wire.h"

LiquidCrystal_I2C lcd(0x27,  16, 2);

int dumpComplete = 0;
int input;
int kB_dumped = 0;
int headerBytesDumped = 0;


void setup()
{
    Serial.begin(115200);
    lcd.init();
    lcd.backlight();
    pinMode(LED_PIN, OUTPUT);
    pinMode(RD_PIN, OUTPUT);
    pinMode(WR_PIN, OUTPUT);
    pinMode(CS_PIN, OUTPUT);
    pinMode(RST_PIN, OUTPUT);
    pinMode(CLK_PIN, OUTPUT);

    DDRL = 0x00;
    DDRA = 0xFF; 
    DDRC = 0xFF;

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
    delayMicroseconds(100);
    byte data = PINL;
    digitalWrite(RD_PIN, HIGH);
    digitalWrite(CS_PIN, HIGH);
    return data;
}

void writeByte(uint8_t data, uint16_t address)
{
  digitalWrite(RD_PIN, HIGH);
  digitalWrite(WR_PIN, LOW);
  setAddress(address);

  DDRL = 0xFF;       
  PORTL = data;      
  delayMicroseconds(100);

  digitalWrite(RD_PIN, LOW);
  digitalWrite(WR_PIN, HIGH);

  DDRL = 0x00;       
}

void dumpROM(uint32_t size, uint32_t startAddress = 0, uint8_t isMBC0 = 0)
{
    if (isMBC0 == 1) {
      Serial.println("START"); 
      Serial.flush();
      kB_dumped = 0;
    }

    for (uint32_t addr = startAddress; addr < startAddress + size; addr++)
    {
        uint8_t data = readByte(addr);
        Serial.write(data); 
        if (addr % 1024 == 0)
        {
          kB_dumped++;
            lcd.clear();
            lcd.setCursor(0,0);
            lcd.print("Progress:");
            lcd.setCursor(0,1);
            lcd.print(kB_dumped);
            lcd.setCursor(4,1);
            lcd.print("KB");
            digitalWrite(LED_PIN, !digitalRead(LED_PIN)); 
            Serial.flush();                               
        }
    }

    if (isMBC0 == 1)
    {
      Serial.println("END"); // End marker
      Serial.flush();
    }
}

void dumpHeader() {
    lcd.setCursor(0,0);
    lcd.clear();
    lcd.print("Sending HEADER");

    Serial.println("START");
    for (uint16_t addr = 0x0104; addr <= 0x0133; addr++) {
        lcd.setCursor(0,1);
        lcd.print(addr - 0x0134);
        uint8_t value = readByte(addr);
        Serial.write(value);
    }
    Serial.flush();
    Serial.println("END");
}


void dumpMBCType()
{
    lcd.setCursor(0,0);
    lcd.clear();
    lcd.print("Sending MBC");
    Serial.println("START");
    uint8_t value = getMBCType();
    Serial.write(value);
    Serial.println("END");
}

void dumpROMSize()
{
    lcd.setCursor(0,0);
    lcd.clear();
    lcd.print("Sending ROM size");
    Serial.println("START");
    uint8_t value = getROMSize();
    Serial.write(value);
    Serial.println("END");
}

void dumpRAMSize()
{
    lcd.setCursor(0,0);
    lcd.clear();
    lcd.print("Sending RAM size");
    Serial.println("START");
    uint8_t value = getRAMSize();
    Serial.write(value);
    Serial.println("END");
}

void dumpTitle()
{
    lcd.setCursor(0, 0);
    lcd.clear();
    lcd.print("Sending TITLE");
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
  uint32_t romSize = (uint32_t)32 * (uint32_t)1024 * (uint32_t)(1u << value);
  return romSize;
}

void dumpMBC0()
{
  uint32_t romSize = calculateROMAddressCount();
  dumpROM(romSize, 0, 1);
}

void dumpMBC1()
{
  uint32_t romSize = calculateROMAddressCount();
  uint32_t bankCount = (romSize / 16384);
  Serial.println("START"); 
  Serial.flush();
  kB_dumped = 0;
  dumpROM(0x4000, 0, 0);
  for (int bankNumber = 1; bankNumber < bankCount; bankNumber++)
  {
      switchMBC1Bank(bankNumber);
      dumpROM(0x4000, 0x4000, 0);
  }
  Serial.println("END"); // End marker
  Serial.flush();
}

void switchMBC1Bank(uint8_t bankNumber)
{
    uint8_t bank = bankNumber & 0x1F;
    if (bank == 0) bank = 1;

    writeByte(bank, 0x2000);
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
      else if (command == "DUMP_MBC0")
      {
        digitalWrite(LED_PIN, HIGH);
        delay(500);
        digitalWrite(LED_PIN, LOW);
        lcd.setCursor(0, 0);
        lcd.clear();
        lcd.print("MBC0 Cart dump");
        dumpMBC0();
      }
      else if (command == "DUMP_MBC1")
      {
        digitalWrite(LED_PIN, HIGH);
        delay(500);
        digitalWrite(LED_PIN, LOW);
        lcd.setCursor(0, 0);
        lcd.clear();
        lcd.print("MBC1 Cart dump");
        dumpMBC1();
      }
      else {
        lcd.setCursor(0, 0);
        lcd.clear();
        lcd.print("Unknown command");
      }
    } 
}