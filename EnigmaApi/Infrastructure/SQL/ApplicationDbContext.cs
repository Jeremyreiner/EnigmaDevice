using Enigma.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnigmaApi.Infrastructure.SQL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreatedAsync().Wait();
            //Database.MigrateAsync().Wait();
        }

        public DbSet<RotorEntity> Rotors { get; set; }
        public DbSet<DeviceEntity> Devices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RotorEntity>().Property(a => a.Id).HasDefaultValueSql("(UUID())");
            modelBuilder.Entity<DeviceEntity>().Property(a => a.Id).HasDefaultValueSql("(UUID())");

            modelBuilder.Entity<DeviceRotorEntity>()
                .HasKey(dr => new { dr.RotorId, dr.DeviceId });

            modelBuilder.Entity<DeviceRotorEntity>()
                .HasOne(dr => dr.Device)
                .WithMany(dr => dr.DeviceRotors)
                .HasForeignKey(dr => dr.DeviceId);


            modelBuilder.Entity<DeviceRotorEntity>()
                .HasOne(dr => dr.Rotor)
                .WithMany(dr => dr.DeviceRotors)
                .HasForeignKey(dr => dr.RotorId);

            //hard push device model to database
            modelBuilder.Entity<DeviceEntity>().HasData(new DeviceEntity
            {
                Id = Guid.NewGuid(),
                Name = "M1",
                Description = "This is the description of the default machine"
            });

            string[] NameModelVars = { "I", "II", "III", "IV", "V", "A", "B" };
            string[] TypeList = { "rotor", "reflector" };
            string[] WiringList =
            {
            "DMTWSILRUYQNKFEJCAZBPGXOHV",
            "HQZGPJTMOBLNCIFDYAWVEUSRKX",
            "UQNTLSZFMREHDPXKIBVYGJCWOA",
            "EHRVXGAOBQUSIMZFLYNWKTPDJC",
            "NTZPSFBOKMWRCJDIVLAEYUXHGQ",
            "EJMZALYXVBWFCRQUONTSPIKHGD",
            "IMETCGFRAYSQBZXWLHKDVUPOJN"
            };

            //hard push original rotors to database
            for (int i = 0; i <= NameModelVars.Length - 1; i++)
            {
                string RotorType = "rotor";
                bool IsReflect = false;
                if (NameModelVars[i] == "A" || NameModelVars[i] == "B")
                {
                    RotorType = TypeList[1];
                    IsReflect = true;
                }
                modelBuilder.Entity<RotorEntity>().HasData(new RotorEntity
                {
                    Id = Guid.NewGuid(),
                    RotorId = i,
                    Name = NameModelVars[i],
                    Type = RotorType,
                    IsReflect = IsReflect,
                    Wiring = WiringList[i]
                });
            }
        }


    }
}
