using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace VGrad_Empty.Models
{
    public class DataContext: DbContext
    {
        public DataContext()
        : base("VGradDB")
        {

        }
        public DbSet<User> Users { get; set; }
    }
}