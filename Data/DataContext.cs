using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using caseStudy.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace caseStudy.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<User> Users { get; set; }

    }
}