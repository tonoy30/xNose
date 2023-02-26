using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace xNose.Core.Reporters
{
    public class JsonFileReporter
    {
        public List<ClassReporter> ClassReporters { get; private set; } = new List<ClassReporter>();
        public string SolutionPath { get; private set; }

        public JsonFileReporter(string solutionPath) => SolutionPath = solutionPath;

        public void AddClassReporter(ClassReporter reporter)
        {
            ClassReporters.Add(reporter);
        }
        public async Task SaveReportAsync()
        {
            var fileName = $"{Path.GetFileName(SolutionPath).Replace(".sln", "")}_test-smell_reports.json";
            var dirName = Path.Join(Path.GetDirectoryName(SolutionPath), fileName);
            using var createStream = File.Create(dirName);
            var options = new JsonSerializerOptions { WriteIndented = true };
            await JsonSerializer.SerializeAsync(createStream, ClassReporters, options);
            await createStream.DisposeAsync();
        }
    }

    public class ClassReporter
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public List<MethodReporter> MethodReporters { get; private set; } = new List<MethodReporter>();
        public void AddMethodReport(MethodReporter reporter)
        {
            MethodReporters.Add(reporter);
        }
    }

    public class MethodReporter
    {
        public string Name { get; set; }
        public string Body { get; set; }
        public List<string> Messages { get; private set; } = new List<string>();
        public void AddMessage(string message)
        {
            Messages.Add(message);
        }
    }
}

