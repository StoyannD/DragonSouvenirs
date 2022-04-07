namespace DragonSouvenirs.Services.Data.Tests
{
    using Xunit;

    public class CommonFeaturesServiceTests
    {
        [Theory]
        [InlineData("Varna", "Asparukhovo", "Kiril Peychinovich", 4, "A", "2", 4, 23)]
        public void GetRenderAddressFull(
            string city,
            string neighborhood,
            string street,
            int streetNumber,
            string apartmentBuilding,
            string entrance,
            int floor,
            int apartmentNumber)
        {
            var service = new CommonFeaturesService();

            var result = service.RenderAddress(city, neighborhood, street, streetNumber, apartmentBuilding, entrance,
                floor, apartmentNumber);

            // var address = "гр. " + city
            //                      + ", кв. " + neighborhood
            //                      + ", ул. " + street
            //                      + " " + streetNumber;
            // address += apartmentBuilding != null
            //     ? ", бл. " + apartmentBuilding + " " : string.Empty;
            // address += entrance != null
            //     ? ", вх. " + entrance + " " : string.Empty;
            // address += ", ет. " + floor
            //                     + ", ап. " + apartmentNumber;
            Assert.Equal("гр. Varna, кв. Asparukhovo, ул. Kiril Peychinovich 4, бл. A, вх. 2, ет. 4, ап. 23", result);
        }

        [Theory]
        [InlineData("Varna", "Asparukhovo", "Kiril Peychinovich", 4, null, "2", 4, 23)]
        public void GetRenderAddressWithoutApartmentBuilding(
            string city,
            string neighborhood,
            string street,
            int streetNumber,
            string apartmentBuilding,
            string entrance,
            int floor,
            int apartmentNumber)
        {
            var service = new CommonFeaturesService();

            var result = service.RenderAddress(city, neighborhood, street, streetNumber, apartmentBuilding, entrance,
                floor, apartmentNumber);

            Assert.Equal("гр. Varna, кв. Asparukhovo, ул. Kiril Peychinovich 4, вх. 2, ет. 4, ап. 23", result);
        }

        [Theory]
        [InlineData("Varna", "Asparukhovo", "Kiril Peychinovich", 4, "A", null, 4, 23)]
        public void GetRenderAddressWithoutEntrance(
            string city,
            string neighborhood,
            string street,
            int streetNumber,
            string apartmentBuilding,
            string entrance,
            int floor,
            int apartmentNumber)
        {
            var service = new CommonFeaturesService();

            var result = service.RenderAddress(city, neighborhood, street, streetNumber, apartmentBuilding, entrance,
                floor, apartmentNumber);

            Assert.Equal("гр. Varna, кв. Asparukhovo, ул. Kiril Peychinovich 4, бл. A, ет. 4, ап. 23", result);
        }

        [Theory]
        [InlineData("Varna", "Asparukhovo", "Kiril Peychinovich", 4, null, null, 4, 23)]
        public void GetRenderAddressWithoutApartmentBuildingAndEntrance(
            string city,
            string neighborhood,
            string street,
            int streetNumber,
            string apartmentBuilding,
            string entrance,
            int floor,
            int apartmentNumber)
        {
            var service = new CommonFeaturesService();

            var result = service.RenderAddress(city, neighborhood, street, streetNumber, apartmentBuilding, entrance,
                floor, apartmentNumber);

            Assert.Equal("гр. Varna, кв. Asparukhovo, ул. Kiril Peychinovich 4, ет. 4, ап. 23", result);
        }
    }
}
