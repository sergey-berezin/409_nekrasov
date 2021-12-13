using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PerceptionComponent
{
    public class InMemoryLibrary : ILibraryDB
    {
        List<ImgDescription> images;
        public InMemoryLibrary()
        {
            images = new List<ImgDescription>();
        }
        public async void AddImage(IFormFile fl)
        {
            Perception perc = new Perception(@"D:\MachineLearning\yolov4.onnx");
            Task<ImgDescription> task = perc.StartPerception(fl);
            images.Add(task.Result);
        }
        public List<ImgDescription> GetImages()
        {
            return images;
        }
        public ImgDescription GetImage(string name)
        {
            for (int i = 0; i < images.Count; ++i)
            {
                if (images[i].ImgName == name)
                {
                    return images[i];
                }
            }
            throw new Exception();
        }
        public void DeleteImages()
        {
            images.Clear();
        }
    }
}
