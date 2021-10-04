using System;
using System.Collections.Generic;
using System.Text;

namespace YOLOv4MLNet
{
    public class ObjDescription
    {
        double x, y;
        double height, width;
        string cls;
        public ObjDescription(double x, double y, double height, double width, string cls)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
            this.cls = cls;
        }
        public override string ToString()
        {
            return "Class: " + cls + ".\nBottom left point coordinates: ("
                + x.ToString() + "; " + y.ToString() + ").\nHeight = " +
                height.ToString() + ".\nWidth = " + width.ToString() + ".\n";
        }
        public string Cls
        {
            get
            {
                return cls;
            }
        }
    }
}
