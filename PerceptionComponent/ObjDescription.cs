using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PerceptionComponent
{
    public class ObjDescription
    {
        [Key]
        public int id { get; set; }
        virtual public ImgDB image { get; set; }
        virtual public double x { get; set; }
        virtual public double y { get; set; }
        virtual public double height { get; set; }
        virtual public double width { get; set; }

        virtual public string Cls { get; set; }
        public ObjDescription()
        {

        }
        public ObjDescription(double x, double y, double height, double width, string cls, ImgDB image)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
            this.Cls = cls;
            this.image = image;
        }
        public ObjDescription(double x, double y, double height, double width, string cls)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
            this.Cls = cls;
        }
        public override string ToString()
        {
            return "Class: " + Cls + ".\nBottom left point coordinates: ("
                + x.ToString() + "; " + y.ToString() + ").\nHeight = " +
                height.ToString() + ".\nWidth = " + width.ToString() + ".\n";
        }
    }
}
