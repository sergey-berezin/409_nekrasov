using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.ComponentModel;

namespace YOLOv4MLNet
{
    public class Model: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ConcurrentDictionary<string, int> classCount;
        ConcurrentBag<ImgDescription> descs;
        public Model()
        {
            descs = new ConcurrentBag<ImgDescription>();
            classCount = new ConcurrentDictionary<string, int>();
            for (int i = 0; i < Perception.classesNames.Length; ++i)
            {
                classCount.TryAdd(Perception.classesNames[i], 0);
            }
        }
        public void Add(ImgDescription desc)
        {
            lock(descs)
            {
                descs.Add(desc);
                foreach (ObjDescription o in desc.Objs)
                {
                    ++classCount[o.Cls];
                }
                OnPropertyChanged();
            }
        }
        public ConcurrentDictionary<string, int> ClassCount
        {
            get
            {
                return classCount;
            }
        }
        public ConcurrentBag<ImgDescription> Descs
        {
            get
            {
                return descs;
            }
        }
        private void OnPropertyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Descs"));
            }
        }
    }
}
