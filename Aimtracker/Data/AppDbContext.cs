using Aimtracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Aimtracker.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Biathlete> Biathletes { get; set; }

        public DbSet<TrainingSession> Shootings { get; set; }

        public DbSet<Series> Series { get; set; }

        public DbSet<Shot> Shots { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Weather> Weathers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
