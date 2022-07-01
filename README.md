# UnrealEngine.Gvas
---
## I currently do not have time to maintain this project, or to reply to issues. If you encounter a crash and you can fix it yourself, feel free to open a pull request, and I'll merge it if it works.
---
A .NET 6 library that can parse, modify and save Unreal Engine 4 save files (GVAS format). This repository also contains an abstraction layer on top of the Gvas parser to load the save format of [Five Nights at Freddy's: Security Breach](https://store.steampowered.com/app/747660/Five_Nights_at_Freddys_Security_Breach/), and also a small project that can convert save files to `.xml` (but not back to .sav!).

The library was made primarily for the new FNaF game, but it has also been tested on a few other UE4 titles. Games tested so far:
- Five Nights at Freddy's: Security Breach
- Five Nights at Freddy's: Help Wanted
- Poppy Playtime: Chapter 1
- Tetris Effect
- The Turing Test

## Example Usage
```c#
const string fileName = @"C:\Users\Sparky\AppData\Local\fnaf9\Saved\SaveGames\SaveGameSlot0.sav";
var saveFile = SaveGameFile.LoadFrom(fileName);
var fnafSave = FNAFSaveData.Load(saveFile);

saveFile.Save(fileName + "_new.sav");
```

Keep in mind that changes made to the `FNAFSaveData` object will not be saved, as that feature is not yet supported. You'll have to modify the `SaveGameFile` object if you want to  modify a save file.
