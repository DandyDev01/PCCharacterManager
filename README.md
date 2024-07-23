Player Character Manager (PCManager)

A program for managing table top RPG characters. Currently supports D&D 5e, Starfinder, and the Dark Souls table top RPG. 

Features:
- Inventory management
- Spellbook management
- Notebook management
- Character stat management

Install Instructions:
1. Clone or download the files as a zip.

Run Instructions: 
1. navigate to the root directory and run the following command.
2. dotnet publish -c Release --self-contained -r win-x64 -p:PublishSingleFile=false
3. nagigatae to PCCharacterManager\PCCharacterManager\bin\Release\net6.0-windows\win-x64\publish
4. Run the .exe file

Run Code Coverage (https://www.youtube.com/watch?v=xwMWGYD8rgk&t=303s):
1. dotnet build // from the project test directory
2. PCCharacterManager\PCCharacterManagerTests> coverlet .\bin\Debug\net6.0-windows\PCCharacterManager.dll --target "dotnet" --targetargs "test --no-build" --exclude "[*]PCCharacterManager.Views*"
3. PCCharacterManager\PCCharacterManagerTests> dotnet test --collect: "XPlat Code Coverage"
4. PCCharacterManager\PCCharacterManagerTests> reportgenerator -reports:".\TestResults\576786d5-640f-4431-b111-51c844f5df05\coverage.cobertura.xml" -targetdir:"coverageresults" -reporttypes:Html

 

