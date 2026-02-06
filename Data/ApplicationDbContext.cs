using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimSapi.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace SimSapi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, 
            IHttpContextAccessor? httpContextAccessor = null)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Sapi> Sapi { get; set; }
        public DbSet<ProduksiSusu> ProduksiSusu { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Peternak> Peternak { get; set; }
        public DbSet<JadwalKegiatan> JadwalKegiatan { get; set; }
        public DbSet<PesertaKegiatan> PesertaKegiatan { get; set; }
        public DbSet<ProduksiOlahan> ProduksiOlahan { get; set; }
        public DbSet<KesehatanSapi> KesehatanSapi { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Konfigurasi Index untuk Sapi
    modelBuilder.Entity<Sapi>()
        .HasIndex(s => s.KodeSapi)
        .IsUnique();

    // Konfigurasi Relasi Sapi -> ProduksiSusu
    modelBuilder.Entity<ProduksiSusu>()
        .HasOne(p => p.Sapi)
        .WithMany(s => s.ProduksiSusu)
        .HasForeignKey(p => p.SapiId)
        .OnDelete(DeleteBehavior.Cascade);

    // Konfigurasi Peternak
    modelBuilder.Entity<Peternak>()
        .HasIndex(p => p.KodePeternak)
        .IsUnique();

    modelBuilder.Entity<Peternak>()
        .HasOne(p => p.User)
        .WithMany()
        .HasForeignKey(p => p.UserId)
        .OnDelete(DeleteBehavior.SetNull);

    // FIX: Pastikan Primary Key Auto Increment
    modelBuilder.Entity<Peternak>()
        .Property(p => p.Id)
        .ValueGeneratedOnAdd();

    // Konfigurasi Jadwal Kegiatan
    modelBuilder.Entity<PesertaKegiatan>()
        .HasOne(p => p.JadwalKegiatan)
        .WithMany(j => j.Peserta)
        .HasForeignKey(p => p.JadwalKegiatanId)
        .OnDelete(DeleteBehavior.Cascade);

    // FIX: Auto Increment untuk semua ID
    modelBuilder.Entity<JadwalKegiatan>()
        .Property(j => j.Id)
        .ValueGeneratedOnAdd();

    modelBuilder.Entity<PesertaKegiatan>()
        .Property(p => p.Id)
        .ValueGeneratedOnAdd();
        // Konfigurasi Kesehatan Sapi
modelBuilder.Entity<KesehatanSapi>()
    .HasOne(k => k.Sapi)
    .WithMany()
    .HasForeignKey(k => k.SapiId)
    .OnDelete(DeleteBehavior.Cascade);
}

        public override int SaveChanges()
        {
            LogChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            LogChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void LogChanges()
        {
            var userId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "System";

            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Sapi || e.Entity is ProduksiSusu)
                .Where(e => e.State == EntityState.Added || 
                           e.State == EntityState.Modified || 
                           e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in entries)
            {
                var log = new ActivityLog
                {
                    UserId = userId,
                    EntityName = entry.Entity.GetType().Name,
                    Action = entry.State.ToString(),
                    Timestamp = DateTime.Now
                };

                if (entry.State == EntityState.Modified)
                {
                    var oldValues = entry.OriginalValues.Properties
                        .ToDictionary(p => p.Name, p => entry.OriginalValues[p]);
                    var newValues = entry.CurrentValues.Properties
                        .ToDictionary(p => p.Name, p => entry.CurrentValues[p]);

                    log.OldData = JsonConvert.SerializeObject(oldValues);
                    log.NewData = JsonConvert.SerializeObject(newValues);
                }
                else if (entry.State == EntityState.Added)
                {
                    var newValues = entry.CurrentValues.Properties
                        .ToDictionary(p => p.Name, p => entry.CurrentValues[p]);
                    log.NewData = JsonConvert.SerializeObject(newValues);
                }
                else if (entry.State == EntityState.Deleted)
                {
                    var oldValues = entry.OriginalValues.Properties
                        .ToDictionary(p => p.Name, p => entry.OriginalValues[p]);
                    log.OldData = JsonConvert.SerializeObject(oldValues);
                }

                ActivityLogs.Add(log);
            }
        }
    }
}