using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace PerceptionComponent
{
    public interface ILibraryDB
    {
        public List<ImgDescription> GetImages();
        public ImgDescription GetImage(string name);
        public void AddImage(IFormFile file);
        public void DeleteImages();
    }
}
