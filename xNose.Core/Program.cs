using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using F23.StringSimilarity;
using xNose.Core.Reporters;
using xNose.Core.Smells;
using xNose.Core.Visitors;
using xNose.Core.ResultAnalysis;

namespace xNose.Core
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            var test = new xNose.Core.ResultAnalysis.ResultAnalysis();
            await test.AnalysisResult();
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
            bool isTest = false;
            if (isTest)
            {
                using (var workspace = MSBuildWorkspace.Create())
                {
                    // Print message for WorkspaceFailed event to help diagnosing project load failures.
                    workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);

                    var solutionPath = //args[0];
                                       "/Users/pp_paul/Documents/workstation/Test_Smell/graphql-platform/src/HotChocolate/Core/HotChocolate.Core.sln";
                                       //"/Users/pp_paul/Documents/workstation/Test_Smell/graphql-platform/src/HotChocolate/Caching/HotChocolate.Caching.sln";
                                       //"/Users/pp_paul/Documents/workstation/Test_Smell/graphql-platform/src/GreenDonut/GreenDonut.sln";
                                       //"/Users/pp_paul/Documents/workstation/Test_Smell/refit/Refit.sln";
                                       //"/Users/pp_paul/Documents/workstation/Test_Smell/Scrutor/Scrutor.sln";
                    //"/Users/pp_paul/Documents/workstation/Test_Smell/IdentityServer4.Admin/Skoruba.IdentityServer4.Admin.sln";
                    //"/Users/pp_paul/Documents/workstation/Test_Smell/c4sharp/C4Sharp.sln";
                    //"/Users/pp_paul/Documents/workstation/Test_Smell/aspnetboilerplate/Abp.sln";
                    //"/Users/pp_paul/Documents/workstation/Test_Smell/Ocelot/Ocelot.sln";
                    //"/Users/pp_paul/Documents/workstation/Test_Smell/NLog/src/NLog.sln";
                    //"/Users/pp_paul/Documents/workstation/Test_Smell/eShopOnWeb/eShopOnWeb.sln";
                    Console.WriteLine($"Loading solution '{solutionPath}'");

                    // Attach progress reporter so we print projects as they are loaded.
                    var solution = await workspace.OpenSolutionAsync(solutionPath, new ConsoleProgressReporter());
                    Console.WriteLine($"Finished loading solution '{solutionPath}'");

                    // TODO: Do analysis on the projects in the loaded solution
                    var projects = solution.Projects.Select(p => p).Where(p => p.Name.Contains("Test", StringComparison.InvariantCultureIgnoreCase));
                    List<string> counter = new List<string>();
                    var reporter = new JsonFileReporter(solutionPath);
                    int testClassCount = 0, testMethodCount = 0;
                    foreach (var project in projects)
                    {
                        if (counter.Contains(project.FilePath.ToString()))
                        {
                            continue;
                        }
                        Console.WriteLine(project.AssemblyName.ToString());
                        Console.WriteLine(project.DefaultNamespace.ToString());
                        counter.Add(project.FilePath.ToString());

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
                                        new DuplicateAssertionTestSmell(),
                                        new MagicNumberTestSmell(),
                                        new EagerTestSmell(),
                                        new BoolInAssertEqualSmell(),
                                        new EqualInAssertSmell(),
                                        new SensitiveEqualitySmell(),
                                        new ConstructorInitializationTestSmell(),
                                        new ObscureInLineSetUpSmell()
                                    };
                        Dictionary<string, Dictionary<string, bool>> otherMethodTestSmell = new Dictionary<string, Dictionary<string, bool>>();
                        foreach (var (classDeclaration, methodDeclarations) in classVisitor.ClassWithOtherMethods)
                        {
                            foreach (var methodDeclaration in methodDeclarations)
                            {
                                if (methodDeclaration.Body == null)
                                    continue;
                                if (!otherMethodTestSmell.ContainsKey(methodDeclaration.Identifier.Text))
                                    otherMethodTestSmell[methodDeclaration.Identifier.Text] = new Dictionary<string, bool>();
                                foreach (var smell in testSmells)
                                {
                                    smell.Node = methodDeclaration;

                                    otherMethodTestSmell[methodDeclaration.Identifier.Text][smell.Name()] = smell.HasSmell();
                                }

                            }
                        }
                        Console.WriteLine("Break");
                        foreach (var smell in testSmells)
                        {
                            smell.otherMethodTestSmell = otherMethodTestSmell;
                        }
                        foreach (var (classDeclaration, methodDeclarations) in classVisitor.ClassWithMethods)
                        {
                            List<string> methodBodyCollection = new List<string>();
                            var classReporter = new ClassReporter
                            {
                                Name = classDeclaration.Identifier.ValueText
                            };
                            testClassCount++;
                            testMethodCount += methodDeclarations.Count;
                            Console.WriteLine($"Analysis started for class: {classReporter.Name}, ProjectName: {project.Name.ToString()}");
                            foreach (var methodDeclaration in methodDeclarations)
                            {
                                if (methodDeclaration.Body == null)
                                {
                                    string errorLine = $"Could not load the body for function: {methodDeclaration.Identifier.Text} in class: {classReporter.Name}";
                                    Console.WriteLine(errorLine);
                                    var tempMethodReporter = new MethodReporter
                                    {
                                        Name = methodDeclaration.Identifier.Text,
                                        Body = errorLine
                                    };
                                    classReporter.AddMethodReport(tempMethodReporter);
                                    continue;
                                }
                                var methodReporter = new MethodReporter
                                {
                                    Name = methodDeclaration.Identifier.Text,
                                    Body = methodDeclaration.Body.NormalizeWhitespace().ToFullString()
                                };
                                methodBodyCollection.Add(methodReporter.Body);
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
                            if (HasLackOfCohesion(methodBodyCollection))
                            {
                                classReporter.Message = "This class has Lack of Cohesion of Test Cases";
                            }

                            reporter.AddClassReporter(classReporter);
                            Console.WriteLine($"Analysis ended for class: {classReporter.Name}");
                        }
                    }
                    Console.WriteLine($"Total Test projects: {counter.Count()}, testClassCount: {testClassCount}, testMethodCount: {testMethodCount}");
                    await reporter.SaveReportAsync();
                }
            }
        }
        private static bool HasLackOfCohesion(List<string> methodBodyCollection)
        {
            var cosineInstance = new Cosine();
            double cosineScoreSum = 0.0;
            int pairCount = 0;
            for (int i = 0; i < methodBodyCollection.Count; i++)
            {
                for (int j = 0; j < methodBodyCollection.Count; j++)
                {
                    if (i != j)
                    {
                        cosineScoreSum += cosineInstance.Similarity(methodBodyCollection[i], methodBodyCollection[j]);
                        pairCount++;
                    }
                }
            }
            if (pairCount <= 0)
                return false;

            var testClassCohesionScore = cosineScoreSum / (double)pairCount;
            Console.WriteLine(testClassCohesionScore);
            return ((1.0 - testClassCohesionScore) >= 0.6);//from paper
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
