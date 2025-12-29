# Arduino Mega-based cartridge dumper for first generation GameBoy cartridges

GameBoy cartridge dumper based on Arduino Mega controller, with CLI and GUI apps.

## Features
- supports MBC0, MBC1, MBC2, MBC3 and MBC5 cartridges (should work with every cartridge featuring one of these MBCs)
- allows for save backups to computer and writing local saved games onto cartridge
- friendly GUI for managing your dumped games

## Connections

- VCC - 5V on Arduino
- CLK - digital port 13
- ~RD - digital port 10
- ~WR - digital port 12
- ~CS - digital port 9
- ~RST - digital port 6
- A0-A7 - PORTA
- A8-A15 - PORTC 
- D0-D7 - PORTL

## Requirements
- Arduino Mega2560
- Windows 10 (for GUI app)
- Windows 10/11, macOS or Linux distribution with .NET installed

