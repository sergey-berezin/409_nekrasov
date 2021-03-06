using System.Collections.Concurrent;
using System.ComponentModel;
using System.Collections.Immutable;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PerceptionComponent
{
    public class Model: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ImmutableDictionary<string, int> classCount;
        ImmutableDictionary<string, ImmutableList<string>> classImages;
        ImmutableList<string> imagesInProcess;
        ConcurrentBag<ImgDescription> descs;
        public Model()
        {
            descs = new ConcurrentBag<ImgDescription>();
            classCount = ImmutableDictionary<string, int>.Empty;
            classImages = ImmutableDictionary<string, ImmutableList<string>>.Empty;
            imagesInProcess = ImmutableList<string>.Empty;
        }
        public void Add(ImgDescription desc, string pathToImage)
        {
            lock(descs)
            {
                descs.Add(desc);
                foreach (ObjDescription o in desc.Objs)
                {
                    if (classCount.ContainsKey(o.Cls))
                    {
                        classCount = classCount.SetItem(o.Cls, classCount[o.Cls] + 1);
                    } else
                    {
                        classCount = classCount.Add(o.Cls, 1);
                    }

                    if (ClassImages.ContainsKey(o.Cls))
                    {
                        if (!classImages[o.Cls].Contains(pathToImage))
                        {
                            classImages = classImages.SetItem(o.Cls, classImages[o.Cls].Add(pathToImage));
                        }
                    } else
                    {
                        classImages = classImages.Add(o.Cls, (ImmutableList<string>.Empty).Add(pathToImage));
                    }
                }
                OnPropertyChanged(nameof(ClassCount));
            }
        }
        public void AddImage(string pathToImage)
        {
            lock (imagesInProcess)
            {
                imagesInProcess = imagesInProcess.Add(pathToImage);
                OnPropertyChanged(nameof(ImagesInProcess));
            }
        }
        public void RemoveImage(int ind)
        {
            lock (imagesInProcess)
            {
                imagesInProcess = imagesInProcess.RemoveAt(ind);
                OnPropertyChanged(nameof(ImagesInProcess));
            }
        }
        public void InsertImage(int idx, string pathToImage)
        {
            lock (imagesInProcess)
            {
                imagesInProcess = imagesInProcess.Insert(idx, pathToImage);
                OnPropertyChanged(nameof(ImagesInProcess));
            }
        }
        public ImmutableDictionary<string, int> ClassCount
        {
            get
            {
                return classCount;
            }
        }
        public ImmutableDictionary<string, ImmutableList<string>> ClassImages
        {
            get
            {
                return classImages;
            }
        }
        public ConcurrentBag<ImgDescription> Descs
        {
            get
            {
                return descs;
            }
        }
        public ImmutableList<string> ImagesInProcess
        {
            get
            {
                return imagesInProcess;
            }
            set
            {
                imagesInProcess = value;
            }
        }
        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}