using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace PerceptionComponent
{
    public class LibraryContext: DbContext
    {
        public DbSet<ImgDB> Images { get; set; }
        public DbSet<ObjDescription> Rectangles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder o) =>
            o.UseLazyLoadingProxies().UseSqlite("DataSource=D:\\C#\\Lab1\\PerceptionComponent\\library.db");
    }
}
