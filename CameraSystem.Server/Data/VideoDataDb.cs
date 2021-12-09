using CameraSystem.Shared;
using Microsoft.EntityFrameworkCore;

namespace CameraSystem.Server.Data
{
    public class VideoDataDb : DbContext
    {
        public DbSet<LoggedCameraTelemetry> CameraTelemetries { get; set; }

        private readonly IConfiguration _configuration;
        private readonly string dbPath;
        public VideoDataDb(IConfiguration config)
        {
            _configuration = config;
            dbPath = config.GetConnectionString("database");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LoggedCameraTelemetry>(entity =>
            {
                entity.HasKey(e=>e.LogId).HasName("log_ids_pkey"); 

                entity.ToTable("camera_telemetry_data");
                entity.Property(e => e.LogId).HasColumnName("log_id")
                .HasColumnType("character varying")
                .IsRequired();
                entity.Property(e => e.CardId).HasColumnName("card_id").HasColumnType("character varying");
                entity.Property(e => e.PassDateTime).HasColumnName("pass_datetime").HasColumnType("datetime");
                entity.Property(e => e.Log).HasColumnName("video_log");
            });
        }
    }
}
