using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using YOLOv4MLNet;

namespace ConsoleUI
{
    public class Program
    {
        static CancellationTokenSource cts;
        private static void Handler(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            cts.Cancel();
        }
        static async Task Main()
        {
            cts = new CancellationTokenSource();
            Console.CancelKeyPress += Handler;
            CancellationToken ct = cts.Token;
            Console.WriteLine("Для отмены выполнения нажмите Ctrl+с на английской раскладке.");
            Perception perc = new Perception(@"D:\MachineLearning\yolov4.onnx");
            DirectoryInfo di = new DirectoryInfo(@"D:\MachineLearning\Input");
            ConsoleView cv = new ConsoleView(di.GetFiles().Length);
            IEnumerable<FileInfo> fi = new List<FileInfo>(di.GetFiles());
            var tasks = fi.Select(flinf => perc.StartPerception(cv.Model, flinf.FullName, ct));
            await Task.WhenAll(tasks);
            cts.Dispose();
        }
    }
}