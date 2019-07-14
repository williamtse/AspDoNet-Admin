using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcMovieContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MvcMovieContext>>()))
            {
                // Look for any movies.
                if (context.User.Any())
                {
                    return;   // DB has been seeded
                }

                context.User.AddRange(
                    new User
                    {
                        Username = "admin",
                        Password = "/OOoOer10+tGwTRDTrQSoeCxVTFr6dtYly7d0cPxIak=",
                        Salt = "NZsP6NnmfBuYeJrrAKNuVQ==",
                        Name = "管理员"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
