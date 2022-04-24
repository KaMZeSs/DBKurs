using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Generate;
using Npgsql;

namespace Randomize
{
    public class Generate
    {
        private readonly static String connectString = "Host=localhost;Port=5432;User Id=postgres;Password=1310;Database=Kurs";
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;

        City[] cities;
        Country[] countries;
        PartOfName[] firstNames, midNames, lastNames;
        String[] propertyTypes;
        String[] districts;
        String[] genres;
        String[] languages;
        String[] recordTypes;


        public class City
        {
            public int city_id;
            public int country_id;
            public String name;
        }
        public class Country
        {
            public string Name;
            public int id;
        }

        public Generate()
        {
            DeserialaizeCities();
            DeserializeCounties();
            DeserializePartsOfName();
            DeserializeDistricts();
            DeserializePropertyTypes();
            DeserializeGenres();
            DeserializeLanguages();
            DeserializeRecordTypes();
        }

        private void DeserializePartsOfName()
        {
            var serializer = new XmlSerializer(typeof(PartOfName[]));

            using (var reader = new StreamReader("Data/Firstnames.xml"))
            {
                firstNames = (PartOfName[])serializer.Deserialize(reader);
            }
            using (var reader = new StreamReader("Data/Midnames.xml"))
            {
                midNames = (PartOfName[])serializer.Deserialize(reader);
            }
            using (var reader = new StreamReader("Data/Lastnames.xml"))
            {
                lastNames = (PartOfName[])serializer.Deserialize(reader);
            }
        }
        private void DeserializeCounties()
        {
            var arrSerializer = new XmlSerializer(typeof(Country[]));

            using (var reader = new StreamReader("Data/Countries.xml"))
            {
                countries = (Country[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserialaizeCities()
        {
            var arrSerializer = new XmlSerializer(typeof(City[]));

            using (var reader = new StreamReader("Data/Cities.xml"))
            {
                cities = (City[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializePropertyTypes()
        {
            var arrSerializer = new XmlSerializer(typeof(String[]));

            using (var reader = new StreamReader("Data/PropertyTypes.xml"))
            {
                propertyTypes = (String[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeDistricts()
        {
            var arrSerializer = new XmlSerializer(typeof(String[]));

            using (var reader = new StreamReader("Data/Districts.xml"))
            {
                districts = (String[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeGenres()
        {
            var arrSerializer = new XmlSerializer(typeof(String[]));

            using (var reader = new StreamReader("Data/Genres.xml"))
            {
                genres = (String[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeLanguages()
        {
            var arrSerializer = new XmlSerializer(typeof(String[]));

            using (var reader = new StreamReader("Data/Languages.xml"))
            {
                languages = (String[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeRecordTypes()
        {
            var arrSerializer = new XmlSerializer(typeof(String[]));

            using (var reader = new StreamReader("Data/RecordTypes.xml"))
            {
                recordTypes = (String[])arrSerializer.Deserialize(reader);
            }
        }



        public City GetRandomCity()
        {
            return cities[new Random().Next(0, cities.Length)];
        }
        public Country GetCountry(City city)
        {
            return Array.Find(countries, (Country x) => x.id == city.country_id);
        }
        public String GetRandomAdress()
        {
            return $"{districts[new Random().Next(0, districts.Length)].Split(' ')[0]}, д. {new Random().Next(0, 300)} {(new Random().Next(0, 2) == 0 ? new Random().Next(0, 300) : )}";
        }
        
    }
}
