using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerceptionComponent;
using System.IO;
using Microsoft.AspNetCore.Cors;

namespace ASPApp
{


    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private ILibraryDB db;
        public ImagesController(ILibraryDB db)
        {
            this.db = db;
        }
        [EnableCors]
        [HttpGet]
        public List<ImgDescription> GetImages()
        {
            return db.GetImages();
        }
        [EnableCors]
        [HttpPost]
        public void AddImage([FromForm]IFormFile file)
        {
            if (file != null)
            {
                db.AddImage(file);
            } else
            {
                throw new Exception();
            }
            //db.AddImage(image);
        }
        [EnableCors]
        public ImgDescription GetImage(string name)
        {
            return db.GetImage(name);
        }
        [HttpDelete]
        public void DeleteImages()
        {
            db.DeleteImages();
        }
    }
}
