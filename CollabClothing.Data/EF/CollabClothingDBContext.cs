using CollabClothing.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.EF
{
    public class CollabClothingDBContext : DbContext
    {
        public CollabClothingDBContext()
        {
        }

        public CollabClothingDBContext(DbContextOptions options) : base(options)
        {
        }
        public CollabClothingDBContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
