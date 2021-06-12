namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Common.Repositories;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels.Administration.Admin.Offices;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class OfficeService : IOfficeService
    {
        private readonly IDeletableEntityRepository<Office> officeRepository;
        private readonly IDeletableEntityRepository<City> citiesRepository;

        public OfficeService(
            IDeletableEntityRepository<Office> officeRepository,
            IDeletableEntityRepository<City> citiesRepository)
        {
            this.officeRepository = officeRepository;
            this.citiesRepository = citiesRepository;
        }

        public async Task UpdateOfficesAsync()
        {
            var request = WebRequest
                .Create(GlobalConstants.Offices.RequestUrl);
            request.Method = "GET";

            var response = (HttpWebResponse)await request.GetResponseAsync();

            string result = null;
            await using (var stream = response.GetResponseStream())
            {
                StreamReader sr = new(stream);
                result = await sr.ReadToEndAsync();
                sr.Close();
            }

            var offices = JsonConvert.DeserializeObject<OfficesViewModel>(result);

            foreach (var office in this.officeRepository.AllWithDeleted())
            {
                this.officeRepository.HardDelete(office);
            }

            foreach (var city in this.citiesRepository.AllWithDeleted())
            {
                this.citiesRepository.HardDelete(city);
            }

            var input = offices
                .Offices
                .Where(o => o.Address.City.Country.Name == GlobalConstants.Offices.Country)
                .Select(o => new Office()
                {
                    Name = o.Name,
                    Country = o.Address.City.Country.Name,
                    City = o.Address.City.Name,
                    Address = o.Address.FullAddress,
                    Neighborhood = o.Address.Quarter,
                    Street = o.Address.Street,
                    StreetNumber = o.Address.Num,
                })
                .ToList();

            foreach (var office in input)
            {
                await this.officeRepository.AddAsync(office);
            }

            var cities = input
                .Select(i => new City
                {
                    Name = i.City,
                })
                .GroupBy(c => c.Name)
                .Select(g => g.First())
                .ToList();

            foreach (var city in cities)
            {
                await this.citiesRepository.AddAsync(city);
            }

            await this.citiesRepository.SaveChangesAsync();
            await this.officeRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Web.ViewModels.Offices.OfficeViewModel>> GetAllOfficesAsync()
        {
            var groupBySubQuery = this.officeRepository
                .All()
                .GroupBy(x => x.City)
                .Select(x => new
                {
                    city = x.Key,
                    count = x.Count(),
                });

            var officesList = await this.officeRepository
                .All()
                .Join(groupBySubQuery, offices =>
                        offices.City,
                    subQ => subQ.city,
                    (offices, subQ) => new { offices, subQ })
                .OrderByDescending(x => x.subQ.count)
                .Select(x => new Web.ViewModels.Offices.OfficeViewModel
                {
                    Name = x.offices.Name,
                    Address = x.offices.Address,
                    City = x.offices.City,
                    Neighborhood = x.offices.Neighborhood,
                    Street = x.offices.Street,
                    StreetNumber = x.offices.StreetNumber,
                })
                .ToListAsync();

            return officesList;
        }

        public async Task<IEnumerable<Web.ViewModels.Offices.CityViewModel>> GetAllCitiesAsync()
        {
            var citiesList = await this.officeRepository
                .All()
                .GroupBy(x => x.City)
                .OrderByDescending(x => x.Count())
                .Select(x => new Web.ViewModels.Offices.CityViewModel
                {
                    Name = x.Key,
                })
                .ToListAsync();

            return citiesList;
        }
    }
}
