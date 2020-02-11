using System.Collections.Generic;
using TaxStore.Dto;
using TaxStore.External;

namespace TaxDatabase
{
    public class MemoryRepository : IMunicipalityRepository
    {
        private Dictionary<string, MunicipalityData> storage;
        public MemoryRepository()
        {
            storage = new Dictionary<string, MunicipalityData>();
        }

        public MunicipalityData GetMunicipality(string municipality)
        {
            if (storage.ContainsKey(municipality))
            {
                return storage[municipality];
            }

            return null;
        }

        public void SaveMunicipality(MunicipalityData municipality)
        {
            storage[municipality.MunicipalityID] = municipality;
        }
    }
}
