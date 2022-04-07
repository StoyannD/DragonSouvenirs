namespace DragonSouvenirs.Services.Data
{
    using Microsoft.AspNetCore.Http;

    public class CommonFeaturesService : ICommonFeaturesService
    {
        public bool IsImageFileTypeValid(IFormFile image)
        {
            if (image != null)
            {
                var fileType = image.ContentType.Split('/')[1];
                if (!TypeIsValid(fileType))
                {
                    return false;
                }
            }

            return true;
        }

        public string RenderAddress(
            string city,
            string neighborhood,
            string street,
            int streetNumber,
            string apartmentBuilding,
            string entrance,
            int floor,
            int apartmentNumber)
        {
            var address = "гр. " + city
                                 + ", кв. " + neighborhood
                                 + ", ул. " + street
                                 + " " + streetNumber;

            address += apartmentBuilding != null
                ? ", бл. " + apartmentBuilding : string.Empty;
            address += entrance != null
                ? ", вх. " + entrance : string.Empty;

            address += ", ет. " + floor
                                + ", ап. " + apartmentNumber;

            return address;
        }

        private static bool TypeIsValid(string fileType)
        {
            return fileType is "jpeg" or "jpg" or "png";
        }
    }
}
