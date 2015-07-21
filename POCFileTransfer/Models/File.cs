using System;
using System.Data.Entity;


namespace POCFileTransfer.Models
{
    public class File
    {
        
        public int ID { get; set; }
        public Boolean isChecked { get; set; }
        public string Filename { get; set; }
        public DateTime LastUploaded { get; set; }
    }
    public class FileDBContext : DbContext
    {
        public DbSet<File> Movies { get; set; }
    }
}
