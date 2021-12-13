using System;
using System.ComponentModel;
using System.Collections.Specialized;

namespace PerceptionComponent
{
    public class ConsoleView
    {
        Model model;
        int maxImages;
        int currentImages;
        public ConsoleView(int maxImages)
        {
            this.maxImages = maxImages;
            currentImages = 0;
            model = new Model();
            model.PropertyChanged += Print;
        }
        private void Print(object sender, PropertyChangedEventArgs e)
        {
            ++currentImages;
            Console.Clear();
            foreach (ImgDescription img in model.Descs)
            {
                Console.WriteLine(img.ToString());
            }
            foreach (string s in Perception.classesNames)
            {
                if (model.ClassCount[s] > 0)
                {
                    Console.WriteLine(s + ": " + model.ClassCount[s]);
                }
            }
            Console.WriteLine();
            Console.WriteLine(((double)currentImages / maxImages * 100).ToString() + " %");
        }
        public Model Model
        {
            get
            {
                return model;
            }
        }
    }
}