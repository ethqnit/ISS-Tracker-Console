<img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=.net" alt=".NET 8">
<img src="https://img.shields.io/badge/C%23-12-239120?style=for-the-badge&logo=csharp" alt="C# 12">
<img src="https://img.shields.io/badge/Platform-Windows%20·%20Linux%20·%20macOS-0078D4?style=for-the-badge&logo=windows" alt="Cross-platform">
<img src="https://img.shields.io/github/stars/ethqnit/ISS-Tracker-Console?style=for-the-badge&color=yellow" alt="Stars">

# International Space Station Live Tracker (Braille Earth Edition)

**A blazing-fast .NET 8 console app that displays the ISS location in real time on a gorgeous Unicode Braille + ASCII world map.**

No external libraries · Pure C# 12 · Updates every 2 seconds · Looks insane in any terminal.

<div align="center">
  <img src=".github/demo.gif" alt="Live demo" />
  <br><br>
  <i>Real-time ISS tracking with continent labels, lat/lon grid, and sparkling marker</i>
</div>

<br>

## Features

- Live data from http://api.open-notify.org/iss-now.json
- Hand-crafted Braille-pattern world map (Unicode █ ░ ⣿ ⣀ etc.)
- Accurate longitude/latitude → console coordinate projection
- Grid lines every 30° with labels
- Labeled continents + oceans
- Zero-flicker live refresh
- Runs anywhere .NET 8 works (Windows, Linux, macOS)

<br>

## How to Run

### Easiest (Windows)
Just double-click `bin\Release\net8.0\ISS-Tracker-Console.exe`  
(or press F5 in Visual Studio)

### From terminal
```bash
dotnet run
