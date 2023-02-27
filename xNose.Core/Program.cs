using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xNose.Core.Reporters;
using xNose.Core.Smells;
using xNose.Core.Visitors;

namespace xNose.Core
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            // Attempt to set the version of MSBuild.
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            var instance = visualStudioInstances.Length == 1
                // If there is only one instance of MSBuild on this machine, set that as the one to use.
                ? visualStudioInstances[0]
                // Handle selecting the version of MSBuild you want to use.
                : SelectVisualStudioInstance(visualStudioInstances);

            Console.WriteLine($"Using MSBuild at '{instance.MSBuildPath}' to load projects.");

            // NOTE: Be sure to register an instance with the MSBuildLocator
            //       before calling MSBuildWorkspace.Create()
            //       otherwise, MSBuildWorkspace won't MEF compose.
            MSBuildLocator.RegisterInstance(instance);

            using (var workspace = MSBuildWorkspace.Create())
            {
                // Print message for WorkspaceFailed event to help diagnosing project load failures.
                workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);

                var solutionPath = args[0];
                Console.WriteLine($"Loading solution '{solutionPath}'");

                // Attach progress reporter so we print projects as they are loaded.
                var solution = await workspace.OpenSolutionAsync(solutionPath, new ConsoleProgressReporter());
                Console.WriteLine($"Finished loading solution '{solutionPath}'");

                // TODO: Do analysis on the projects in the loaded solution
                var project = solution.Projects.FirstOrDefault(p => string.Equals(p.Name, "xNose.Example.Test"));

                var compilation = await project.GetCompilationAsync();

                var classVisitor = new ClassVirtualizationVisitor();

                foreach (var syntaxTree in compilation.SyntaxTrees)
                {
                    classVisitor.Visit(syntaxTree.GetRoot());
                }


                var testSmells = new List<ASmell> {
                    new EmptyTestSmell(),
                    new ConditionalTestSmell(),
                    new CyclomaticComplexityTestSmell(),
                    new ExpectedExceptionTestSmell(),
                    new AssertionRouletteTestSmell(),
                    new UnknownTestSmell(),
                    new RedundantPrintTestSmell(),
                    new SleepyTestSmell(),
                    new IgnoreTestSmell(),
                    new RedundantAssertionTestSmell(),
                    new DuplicateAssertionTestSmell()
                };

                var reporter = new JsonFileReporter(solutionPath);

                foreach (var (classDeclaration, methodDeclarations) in classVisitor.ClassWithMethods)
                {
                    var classReporter = new ClassReporter
                    {
                        Name = classDeclaration.Identifier.ValueText
                    };
                    foreach (var methodDeclaration in methodDeclarations)
                    {
                        var methodReporter = new MethodReporter
                        {
                            Name = methodDeclaration.Identifier.Text,
                            Body = methodDeclaration.Body.NormalizeWhitespace().ToFullString()
                        };
                        foreach (var smell in testSmells)
                        {
                            smell.Node = methodDeclaration;
                            var message = new MethodReporterMessage
                            {
                                Name = smell.Name(),
                                Status = smell.HasSmell() ? "Found" : "Not Found"
                            };
                            methodReporter.AddMessage(message);
                        }
                        classReporter.AddMethodReport(methodReporter);
                    }
                    reporter.AddClassReporter(classReporter);
                }
                await reporter.SaveReportAsync();
            }
        }

        private static VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
        {
            Console.WriteLine("Multiple installs of MSBuild detected please select one:");
            for (int i = 0; i < visualStudioInstances.Length; i++)
            {
                Console.WriteLine($"Instance {i + 1}");
                Console.WriteLine($"    Name: {visualStudioInstances[i].Name}");
                Console.WriteLine($"    Version: {visualStudioInstances[i].Version}");
                Console.WriteLine($"    MSBuild Path: {visualStudioInstances[i].MSBuildPath}");
            }

            while (true)
            {
                var userResponse = Console.ReadLine();
                if (int.TryParse(userResponse, out int instanceNumber) &&
                    instanceNumber > 0 &&
                    instanceNumber <= visualStudioInstances.Length)
                {
                    return visualStudioInstances[instanceNumber - 1];
                }
                Console.WriteLine("Input not accepted, try again.");
            }
        }
    }
}
