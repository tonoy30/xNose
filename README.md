# xNose: A Test Smell Detector for C#
This tool can detect test smells written with xUnit package in C# projects.
Currently, we support the following test smells:
1. Assertion Roulette Test Smell
2. Conditional Test Smell
3. Constructor Initialization Test Smell
4. Duplicate Assert Test Smell
5. Empty Test Smell
6. Eager Test Smell
7. Ignored Test Smell
8. Lack of Cohesion Smell
9. Magic Number Test Smell
10. Obscure In-Line Setup Test Smell
11. Redundant Assertion Test Smell
12. Redundant Print Smell
13. Sleepy Test Smell
14. Sensitive Equality Test Smell
15. Unknown Test Smell
16. Equal in Assert Test Smell (C# specific)
17. Bool in Assert Equal Test Smell (C# specific)
18. Cyclomatic Complexity Test Smell (C# specific)

## Setup
To run this tool you have to download this repository in your local device.
Then follow the steps to run it with command line(CMD).
> ** Prerequisite: You have already installed C# environment in your pc and it's already associated with your command line. **

1. Go to the xNose directory
2. Open terminal and type the following command

> ``dotnet run --project xNose.Core/xNose.Core.csproj   "<solution_path>"``

Here the `solution_path` is the path to your C# project solution file.

This will generate a `JSON` file of the given project to the root path of that project. This `JSON` file will have the info about test smells of the given project.

Otherwise, you can run the `xNose.sln` file with your visual studio. You have to provide the solution path of the desired project in the argument section.
