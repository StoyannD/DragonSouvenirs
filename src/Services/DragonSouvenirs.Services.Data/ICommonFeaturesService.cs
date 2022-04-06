namespace DragonSouvenirs.Services.Data
{
    using Microsoft.AspNetCore.Http;

    public interface ICommonFeaturesService
    {
        bool IsImageFileTypeValid(IFormFile image);

        string RenderAddress(
            string city,
            string neighborhood,
            string street,
            int streetNumber,
            string apartmentBuilding,
            string entrance,
            int floor,
            int apartmentNumber);
    }
}
