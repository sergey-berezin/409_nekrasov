using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PerceptionComponent
{
    public class ImgDescription
    {
        string imgName;
        ConcurrentBag<ObjDescription> objs;
        public ImgDescription()
        {

        }
        public ImgDescription(string imgName)
        {
            this.imgName = imgName;
            objs = new ConcurrentBag<ObjDescription>();
        }
        public void Add(ObjDescription obj)
        {
            objs.Add(obj);
        }
        public override string ToString()
        {
            string res = imgName + "\n";
            foreach(ObjDescription desc in objs)
            {
                res += desc.ToString();
            }
            return res;
        }
        public string ImgName
        {
            get
            {
                return imgName;
            }
        }
        public ConcurrentBag<ObjDescription> Objs
        {
            get
            {
                return objs;
            }
            set
            {
                objs = value;
            }
        }
    }
}
