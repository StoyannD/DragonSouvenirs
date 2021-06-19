namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Common.Repositories;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Web.ViewModels.Administration.Admin.Offices;
    using HtmlAgilityPack;
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
            foreach (var office in this.officeRepository.AllWithDeleted())
            {
                this.officeRepository.HardDelete(office);
            }

            foreach (var city in this.citiesRepository.AllWithDeleted())
            {
                this.citiesRepository.HardDelete(city);
            }

            await this.UpdateEcontOfficesAsync();
            await this.UpdateSpeedyOfficesAsync();
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

        private async Task UpdateEcontOfficesAsync()
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

            var input = offices
                .Offices
                .Where(o => o.Address.City.Country.Name == GlobalConstants.Offices.Country)
                .Select(o => new Office()
                {
                    OfficeBrand = "Econt",
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

        private async Task UpdateSpeedyOfficesAsync()
        {
            var requestUrl = "https://www.speedy.bg/bg/speedy-offices-automats?city=all&formToken=-false";

            HtmlWeb web = new();
            var document = web.Load(requestUrl);

            var nodes = document.DocumentNode
                .SelectNodes("//*[@id=\"main\"]/div/div/div/article/div[2]/div[2]/div");

            foreach (var node in nodes)
            {
                var name = node.Elements("p").First().InnerHtml;

                var addressRaw = node.Elements("p").ElementAt(1).InnerHtml;
                var addressRegex = Regex.Match(addressRaw, "<br>(?'address'.*)");

                var address = addressRegex.Groups["address"].Value;

                if (!string.IsNullOrEmpty(name) &&
                    !string.IsNullOrEmpty(address))
                {
                    var office = new Office()
                    {
                        OfficeBrand = "Speedy",
                        Name = name,
                        Address = address,
                    };

                    await this.officeRepository.AddAsync(office);
                }
            }

            await this.citiesRepository.SaveChangesAsync();
            await this.officeRepository.SaveChangesAsync();
        }
    }
}
