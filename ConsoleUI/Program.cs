using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using YOLOv4MLNet;

namespace ConsoleUI
{
    public class Program
    {
        private static void Handler(object sender, ConsoleCancelEventArgs args)
        {
            Environment.Exit(0);
        }
        static async Task Main()
        {

            Console.WriteLine("Для отмены выполнения нажмите Ctrl+с на английской раскладке.");
            Perception perc = new Perception(@"D:\MachineLearning\yolov4.onnx");
            DirectoryInfo di = new DirectoryInfo(@"D:\MachineLearning\Input");
            FileInfo[] imgs = di.GetFiles();
            ConsoleView cv = new ConsoleView(imgs.Length);
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;
            Console.CancelKeyPress += Handler;
            foreach (FileInfo flinf in imgs)
            {
                await perc.StartPerception(cv.Model, flinf.FullName, ct);
            }
        }
    }
}
