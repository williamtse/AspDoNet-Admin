using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Models
{
    public class MvcMovieContext : DbContext
    {
        public MvcMovieContext (DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        public DbSet<MvcMovie.Models.User> User { get; set; }

        public DbSet<MvcMovie.Models.Permission> Permission { get; set; }

        public DbSet<MvcMovie.Models.Role> Role { get; set; }

        public DbSet<MvcMovie.Models.Menu> Menu { get; set; }
    }
}
