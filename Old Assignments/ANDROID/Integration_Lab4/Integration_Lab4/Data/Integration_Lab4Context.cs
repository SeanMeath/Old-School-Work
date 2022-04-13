using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Integration_Lab4.Models;

namespace Integration_Lab4.Models
{
    public class Integration_Lab4Context : DbContext
    {
        public Integration_Lab4Context (DbContextOptions<Integration_Lab4Context> options)
            : base(options)
        {
        }

        public DbSet<Integration_Lab4.Models.Staff> Staff { get; set; }

        public DbSet<Integration_Lab4.Models.Course> Course { get; set; }

        public DbSet<Integration_Lab4.Models.Book> Book { get; set; }
    }
}
