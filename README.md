# Arduino Mega-based DMG GameBoy cartridge reader

## Connections

- VCC - 5V on Arduino
- CLK - digital port 13
- ~RD - digital port 10
- ~WR - digital port 12
- ~CS - digital port 9
- A0-A7 - PORTA
- A8-A15 - PORTC
- D0-D7 - PORTL
- ~RST - digital port 6

- LED as progress indicator - digital port 7

## Requirements

`reader_companion.py` requires Python 3 installed along with PySerial package.
