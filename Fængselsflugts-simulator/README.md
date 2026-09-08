# Fængselsflugts-simulator 🚨

Et konsolprogram i C#, hvor fanger forsøger at flygte fra et fængsel, mens vagter patruljerer og forsøger at stoppe dem.

## Projektidé

Programmet skal simulere et fængsel med fanger, vagter, flugtplaner, genstande og forskellige hændelser.

Fangerne kan forsøge forskellige flugtmetoder, mens vagterne patruljerer fængslet. Programmet skal blandt andet bruge threading til at simulere flere handlinger på samme tid.

## Funktioner

- Fanger med forskellige statusser og flugtplaner
- Vagter der patruljerer fængslet
- Inventory med forskellige items
- Forskellige flugtmetoder
- Alarm ved flugtforsøg
- Flere samtidige handlinger med Thread eller Task
- Trådsikkerhed med lock og Interlocked
- ConcurrentDictionary til fælles data
- Events og callbacks
- Lambda og Func til filtrering
- Generiske collections

## UML-diagram

![UML-diagram](PrisonEscape-UML.png)

## Teknologier

- C#
- .NET
- Visual Studio
- Git og GitHub
- Mermaid UML