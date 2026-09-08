# Fængselsflugts-simulator 🚨

Et C# konsolprogram, der simulerer et fængsel, hvor fanger forsøger at gennemføre forskellige flugtplaner, mens vagter og fængslets kontrolsystem reagerer på hændelser.

Projektet bruges til at demonstrere centrale emner fra undervisningen i objektorienteret programmering og C#.

## Fængslet

Simulatoren indeholder et visuelt fængselskort med blandt andet:

- Celleblok A og B
- Kantine
- Gård
- Bad
- Sygestue
- Vagtrum
- Kontrolrum
- Lager
- Hovedindgang

Kortet vises direkte i konsollen med farver og symboler.

## Faglige emner

Projektet skal demonstrere:

- Arv og abstrakte klasser
- Polymorfi og override
- Indkapsling
- Access modifiers
- Interfaces
- Collections
- Generics
- Exceptions
- Delegates og callbacks
- Lambda expressions
- Dependency Inversion
- Løs kobling
- UML
- XML-dokumentation
- Git og GitHub

## Simulatoren

Der vil være forskellige typer fanger med forskellige egenskaber og evner.

Fangerne kan blandt andet forsøge forskellige handlinger og flugtmetoder. De konkrete fangetyper skal arve fra en fælles abstrakt basisklasse og kunne reagere forskelligt gennem polymorfi.

Interfaces bruges til evner, som kun nogle fanger har.

Fængslets kontrolcentral holder styr på personer og hændelser. Hændelser kan eksempelvis være:

- Flugtforsøg
- Slagsmål
- Strømsvigt
- Alarm
- Ulovlig adgang til et område

Programmet skal kunne tildele personer til hændelser gennem en udskiftelig strategi, så tildelingslogikken ikke er direkte afhængig af kontrolcentralen.

## UML-diagram

UML-diagrammet bliver løbende opdateret, når projektets design udvikler sig.

![UML-diagram](PrisonEscape-UML.png)

## Teknologier

- C#
- .NET
- Visual Studio
- Git
- GitHub
- Mermaid UML

## Status

Projektet er under udvikling.