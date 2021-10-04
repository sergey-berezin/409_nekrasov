using System.Collections.Concurrent;

namespace YOLOv4MLNet
{
    public class ImgDescription
    {
        string name;
        ConcurrentBag<ObjDescription> objs;
        public ImgDescription(string name)
        {
            this.name = name;
            objs = new ConcurrentBag<ObjDescription>();
        }
        public void Add(ObjDescription obj)
        {
            objs.Add(obj);
        }
        public override string ToString()
        {
            string res = name + "\n";
            foreach(ObjDescription desc in objs)
            {
                res += desc.ToString();
            }
            return res;
        }
        public ConcurrentBag<ObjDescription> Objs
        {
            get
            {
                return objs;
            }
        }
    }
}
