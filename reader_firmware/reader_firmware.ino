#define LED_PIN 7
#define RD_PIN 10
#define WR_PIN 12
#define CS_PIN 9
#define RST_PIN 6
#define CLK_PIN 13

int dumpComplete = 0;

void setup()
{
    Serial.begin(115200);

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

    digitalWrite(LED_PIN, HIGH);
    delay(500);
    digitalWrite(LED_PIN, LOW);
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

void dumpROM(uint32_t size)
{
    Serial.println("START DUMP"); // Start marker
    Serial.flush();

    for (uint32_t addr = 0; addr < size; addr++)
    {
        uint8_t data = readByte(addr);
        Serial.write(data); // Send raw bytes
        if (addr % 4096 == 0)
        {
            digitalWrite(LED_PIN, !digitalRead(LED_PIN)); // Toggle LED every 4 KB to indicate progress
            Serial.flush();                               // Mitigate buffer overflow
        }
    }

    Serial.println("END DUMP"); // End marker
    Serial.flush();
}

void loop()
{
    if (dumpComplete == 0)
    {
        dumpROM(0x8000); // 32 KB dla Tetrisa
        dumpComplete = 1;
    }
    else
    {
        digitalWrite(LED_PIN, HIGH);
        delay(250);
        digitalWrite(LED_PIN, LOW);
        delay(250);
    }
}