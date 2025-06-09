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

    DDRL = 0x00; // PORTL as output (D0-D7) - data at the address
    DDRA = 0xFF; // PORTA as input (A0-A7) - first part of address
    DDRC = 0xFF; // PORTC as input (A8-A15) - second part of address

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

void dumpROM(uint32_t size, uint32_t startAddress = 0)
{
    Serial.println("START"); // Start marker
    Serial.flush();

    for (uint32_t addr = startAddress; addr < size; addr++)
    {
        uint8_t data = readByte(addr);
        Serial.write(data); // Send raw bytes
        if (addr % 4096 == 0)
        {
          kB_dumped += 4;
            lcd.setCursor(0,0);
            lcd.print("Progress:");
            lcd.setCursor(0,1);
            lcd.print(kB_dumped);
            lcd.setCursor(4,1);
            lcd.print("KB");
            digitalWrite(LED_PIN, !digitalRead(LED_PIN)); // Toggle LED every 4 KB to indicate progress
            Serial.flush();                               // Mitigate buffer overflow
        }
    }

    Serial.println("END"); // End marker
    Serial.flush();
}

void dumpHeader() {
    lcd.setCursor(0,0);
    lcd.print("Sending HEADER");

    Serial.println("START");
    for (uint16_t addr = 0x0104; addr <= 0x0133; addr++) {
        lcd.setCursor(0,1);
        lcd.print(addr - 0x0134);
        uint8_t value = readByte(addr);
        Serial.write(value);
    }
    Serial.println("END");
}


void dumpMBCType()
{
    lcd.setCursor(0,0);
    lcd.print("Sending MBC");
    Serial.println("START");
    uint8_t value = getMBCType();
    Serial.write(value);
    Serial.println("END");
}

void dumpROMSize()
{
    lcd.setCursor(0,0);
    lcd.print("Sending ROM size");
    Serial.println("START");
    uint8_t value = getROMSize();
    Serial.write(value);
    Serial.println("END");
}

void dumpRAMSize()
{
    lcd.setCursor(0,0);
    lcd.print("Sending RAM size");
    Serial.println("START");
    uint8_t value = getRAMSize();
    Serial.write(value);
    Serial.println("END");
}

void dumpTitle()
{
    lcd.setCursor(0, 0);
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

void loop()
{

    if (Serial.available()) {
      String command = Serial.readStringUntil('\n');
      Serial.println(command);
      if (command == "GET_HEADER") {
          dumpHeader();
      }
      if (command == "GET_MBC")
      {
        dumpMBCType();
      }
      if (command == "GET_ROM_SIZE")
      {
        dumpROMSize();
      }
      if (command == "GET_RAM_SIZE")
      {
        dumpRAMSize();
      }
      if (command == "GET_TITLE")
      {
        dumpTitle();
      }
      if (command == "DUMP_ROM") {
        digitalWrite(LED_PIN, HIGH);
        delay(500);
        digitalWrite(LED_PIN, LOW);
        dumpROM(0x8000); // 32 KB dla Tetrisa
      }
    } 
}