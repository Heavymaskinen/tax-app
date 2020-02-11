using System;
using TaxStore;
using TaxStore.Dto;
using TaxStore.External;
using TaxStore.Model;

namespace TaxStoreTests.Stubs
{
    public class FakeRepository : IMunicipalityRepository
    {
        private MunicipalityData municipality;

        public MunicipalityData savedMunicipality;

        public FakeRepository(MunicipalityData municipality)
        {
            this.municipality = municipality;
        }

        public MunicipalityData GetMunicipality(string municipalityName)
        {
            return this.municipality;
        }

        public void SaveMunicipality(MunicipalityData municipality)
        {
            savedMunicipality = municipality;
        }
    }
}
