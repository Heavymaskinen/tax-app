using TaxStore.Dto;

namespace TaxStore.External
{
    public interface IMunicipalityRepository
    {
        MunicipalityData GetMunicipality(string municipality);
        void SaveMunicipality(MunicipalityData municipality);
    }
}
