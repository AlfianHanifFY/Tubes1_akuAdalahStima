<h1 align="center">Tugas Besar 1 IF2211 Strategi Algoritma</h1>
<h2 align="center">Semester II tahun 2024/2025</h2>
<h3 align="center">Pemanfaatan Algoritma Greedy dalam Pembuatan Bot Permainan Robocode Tank Royale</h3>

## Table of Contents
- [Description](#description)
- [Implemented Greedy Algorithms](#implemented-greedy-algorithms)
- [Program Structure](#program-structure)
- [Requirements & Installation](#requirements--installation)
- [Building and Compiling Bot](#building-and-compiling-bot)
- [Author](#author)
- [References](#references)

## Description
This project contains bot implementations for the Robocode Tank Royale game using various greedy strategies. Each bot is designed with different heuristic approaches to determine the optimal decision in battle.

## Implemented Greedy Algorithms
### 1. Greedy by Lowest Energy Enemy (NyampahBot)
- The bot always targets the enemy with the lowest energy in the arena.
- **Advantage**: Effective in eliminating weakened enemies quickly.
- **Disadvantage**: Does not consider the position and movement of other enemies.
### 2. Greedy by Nearest Enemy (SenggolTembakBot)
- The bot always attacks the closest enemy based on radar scanning.
- **Advantage**: Quickly finds and attacks a target.
- **Disadvantage**: Less effective against fast-moving enemies or those with evasive strategies.
### 3. Greedy by Crowd Evasion (CulunBot)
- The bot avoids areas with the highest enemy density to survive longer.
- **Advantage**: Reduces the risk of being attacked by multiple enemies at once.
- **Disadvantage**: Less effective if enemies are evenly spread across the arena.
### 4. Greedy by Single Target Enemy (GoBot)
- The bot focuses on chasing and attacking a single designated target until it is destroyed.
- **Advantage**: Effective in one-on-one duels.
- **Disadvantage**: Vulnerable to attacks from other surrounding enemies.

## Program Structure
```
├── src/
│   ├── main-bot/
│   │   └── CulunBot/
│   └── alternative-bot/
│       ├── GoBot/
│       ├── NyampahBot/
│       └── SenggolTembakBot/
├── doc/
│   └── akuAdalahStima.pdf
└── README.md       
```
- **src** : contains bot implementations for the Robocode Tank Royale
- **doc** : contains the assignment report and program documentation.

## Requirements & Installation
Before running the bot, ensure that the following dependencies are installed:
### Requirements:
- [Java Development Kit (JDK)](https://www.oracle.com/java/technologies/downloads/)
- [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0 )
- [Robocode Tank Royale GUI](https://github.com/Ariel-HS/tubes1-if2211-starter-pack)
- [Our Starter Pack Bot](https://github.com/AlfianHanifFY/Tubes1_akuAdalahStima/releases/tag/v1.0)
### Installation and Running the Program:
1. Download and Install Dependencies:
    - Install .NET 6.0 and JDK according to your operating system.
    - Download the `robocode-tankroyale-gui-0.30.0.jar` file from the link above.
        
        https://github.com/Ariel-HS/tubes1-if2211-starter-pack
    - Download and extract [Our Starter Pack Bot](https://github.com/AlfianHanifFY/Tubes1_akuAdalahStima/releases/tag/v1.0)
2. Running The Game Engine
    - Open a terminal and navigate to the location where `robocode-tankroyale-gui-0.30.0.jar` is stored.
    - Run the following command:
        ```bash
        java -jar robocode-tankroyale-gui-0.30.0.jar
        ```
3. Adding the Bot to the Engine
    - Click **Config → Bot Root Directories**.
    - Click **Add**, then select the `main-bot` and/or `alternative-bot` folders inside the `src` directory.
    - Click OK to save the configuration.
4. Starting a Battle
    - Click **Battle → Start Battle**.
    - Select the bot to be used, then click **Boot →** to load it.
    - Add the bot to the arena using **Add →** or **Add All →**.
    - Press **Start Battle** to begin the match.
5. Adjusting Game Rules (Optional)
    - Click **Setup Rules** to customize settings such as turn count, arena size, etc.

## Building and Compiling Bot
This is an alternative if you want to build and compile the bot without using GUI. Make sure to have .NET installed
### 1. Go to bot directory
        cd your/bot/directory
### 2. Build
        dotnet build
### 3. Run
        dotnet run

## Author
| **NIM**  | **Nama Anggota**               | **Github** |
| -------- | ------------------------------ | ---------- |
| 13523073 | Alfian Hanif Fitria Yustanto   | [AlfianHanifFY](https://github.com/AlfianHanifFY) |
| 13523091 | Carlo Angkisan                 | [carllix](https://github.com/carllix) | 
| 13523115 | Azfa Radhiyya Hakim            | [azfaradhi](https://github.com/azfaradhi) |

## References
- [Spesifikasi Tugas Besar 1 Stima 2024/2025](https://docs.google.com/document/d/14MCaRiFGiA6Ez5W8-OLxZ9enXyENcep7AzSH6sUHKM8/edit?tab=t.0)
- [Slide Kuliah IF2211 2024/2025 Algoritma Greedy (Bagian 1)](https://informatika.stei.itb.ac.id/~rinaldi.munir/Stmik/2024-2025/04-Algoritma-Greedy-(2025)-Bag1.pdf)
- [Slide Kuliah IF2211 2024/2025 Algoritma Greedy (Bagian 2)](https://informatika.stei.itb.ac.id/~rinaldi.munir/Stmik/2024-2025/05-Algoritma-Greedy-(2025)-Bag2.pdf)
- [Slide Kuliah IF2211 2024/2025 Algoritma Greedy (Bagian 3)](https://informatika.stei.itb.ac.id/~rinaldi.munir/Stmik/2024-2025/06-Algoritma-Greedy-(2025)-Bag3.pdf)
