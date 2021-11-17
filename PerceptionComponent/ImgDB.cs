using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace PerceptionComponent
{
    public class ImgDB
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public byte[] blob { get; set; }
        virtual public ICollection<ObjDescription> descs { get; set; }
        public int hash { get; set; }
        public ImgDB()
        {

        }
        public ImgDB(ImgDescription imgDesc)
        {
            this.name = imgDesc.ImgName;
            descs = new List<ObjDescription>();
        }
        public override string ToString()
        {
            return name + "\n" + "Hash: " + hash.ToString() + "\n";
        }
    }
}
