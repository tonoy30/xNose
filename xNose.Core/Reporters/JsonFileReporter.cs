using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace xNose.Core.Reporters
{
    public class JsonFileReporter
    {
        public List<ClassReporter> Classes { get; private set; } = new List<ClassReporter>();
        public string SolutionPath { get; private set; }

        public JsonFileReporter(string solutionPath) => SolutionPath = solutionPath;

        public void AddClassReporter(ClassReporter reporter)
        {
            Classes.Add(reporter);
        }
        public async Task SaveReportAsync()
        {
            var fileName = $"{Path.GetFileName(SolutionPath).Replace(".sln", "").ToLowerInvariant()}_test_smell_reports.json";
            var dirName = Path.Join(Path.GetDirectoryName(SolutionPath), fileName);
            using var createStream = File.Create(dirName);
            var options = new JsonSerializerOptions { WriteIndented = true };
            await JsonSerializer.SerializeAsync(createStream, Classes, options);
            await createStream.DisposeAsync();
        }
    }

    public class ClassReporter
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public List<MethodReporter> Methods { get; private set; } = new List<MethodReporter>();
        public void AddMethodReport(MethodReporter reporter)
        {
            Methods.Add(reporter);
        }
    }

    public class MethodReporter
    {
        public string Name { get; set; }
        public string Body { get; set; }
        public List<MethodReporterMessage> Smells { get; private set; } = new List<MethodReporterMessage>();
        public void AddMessage(MethodReporterMessage message)
        {
            Smells.Add(message);
        }
    }
    public class MethodReporterMessage
    {
        public string Name { get; set; }
        public string Status { get; set; }
    }
}

