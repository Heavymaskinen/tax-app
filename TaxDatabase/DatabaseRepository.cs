using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using TaxStore.Dto;
using TaxStore.External;
using TaxStore.Model;

namespace TaxDatabase
{
    public class DatabaseRepository : DbContext, IMunicipalityRepository
    {
        public DbSet<MunicipalityData> Municipalities { get; set; }
        public DbSet<TaxData> Taxes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Filename=./taxes.db");

        public MunicipalityData GetMunicipality(string municipality)
        {
            
            MunicipalityData municipalityData =
                Municipalities.AsNoTracking()
                .Where(m => m.MunicipalityID.Equals(municipality))
                .Include(m => m.TaxSchedules)
                .SingleOrDefault();

            return municipalityData;
        }

        public void SaveMunicipality(MunicipalityData municipality)
        {
            Municipalities.Update(municipality);
            Taxes.AddRange(municipality.TaxSchedules);
            SaveChanges();
        }

    }
}
