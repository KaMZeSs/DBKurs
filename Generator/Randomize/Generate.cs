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
        String[] executors;
        String[] companies; // Магазин и фирма звукозаписи
        String[] streets;


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
        public class RecordFirm
        {
            public int city_id;
            public int firm_id;
            public String name;
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
            DeserializeExecutors();
            DeserializeCompanies();
            DeserializeStreets();
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
        private void DeserializeExecutors()
        {
            var arrSerializer = new XmlSerializer(typeof(String[]));

            using (var reader = new StreamReader("Data/Executors.xml"))
            {
                executors = (String[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeCompanies()
        {
            var arrSerializer = new XmlSerializer(typeof(String[]));

            using (var reader = new StreamReader("Data/Companies.xml"))
            {
                companies = (String[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeStreets()
        {
            var arrSerializer = new XmlSerializer(typeof(String[]));

            using (var reader = new StreamReader("Data/Streets.xml"))
            {
                streets = (String[])arrSerializer.Deserialize(reader);
            }
        }


        private City GetRandomCity()
        {
            return cities[new Random().Next(0, cities.Length)];
        }
        private Country GetCountry(City city)
        {
            return Array.Find(countries, (Country x) => x.id == city.country_id);
        }
        private String GetRandomAdress()
        {
            return $"{streets[new Random().Next(0, streets.Length)]}, {new Random().Next(0, 200)}";
            // return $"{districts[new Random().Next(0, districts.Length)].Split(' ')[0]}, д. {new Random().Next(0, 300)} {(new Random().Next(0, 2) == 0 ? new Random().Next(0, 300) : )}";
        }

        Random rnd = new Random();

        public void Generation(int len = 10000)
        {
            // Идея в том, чтобы сгенерировать n-ю часть запрошенного количества, а потом с k-м% шансом добавлять новые данные
            // Если сгенерить 100 раз, то получается 10000 вариантов ассортимента, из которых 100 уже есть
            DataBase db = new DataBase();

            String temp;
            for (int i = 0; i < len; i++)
            {
                int shop_id;
                //Shop
                if (rnd.Next(0, 4) == 0) // Если 0 - использовать новое значение
                {
                    int district_id;
                    //District
                    if (rnd.Next(0, 2) == 0) //Если 0 - использовать новое значение
                    {
                        temp = districts[rnd.Next(0, districts.Length)];
                        if (db.districts.Where(x => x.name == temp).Count() == 0) //Если попался тот, которого еще не было, то используем его
                        {
                            district_id = districts.Length;
                            db.districts.Add(new District(db.districts.Count, temp));
                        }
                        else //если он уже был, берем случайный из тех, что уже были
                        {
                            district_id = rnd.Next(0, districts.Length);
                        }
                    }
                    else //Существующее
                    {
                        district_id = rnd.Next(0, districts.Length);
                    }

                    int propertyType_id;
                    //PropertyType
                    if (rnd.Next(0, 2) == 0) //Если 0 - использовать новое значение
                    {
                        temp = propertyTypes[rnd.Next(0, propertyTypes.Length)];
                        if (db.propertyTypes.Where(x => x.type == temp).Count() == 0)
                        {
                            propertyType_id = propertyTypes.Length;
                            db.propertyTypes.Add(new PropertyType(db.propertyTypes.Count, temp));
                        }
                        else
                        {
                            propertyType_id = rnd.Next(0, propertyTypes.Length);
                        }
                    }
                    else //Существующее
                    {
                        propertyType_id = rnd.Next(0, propertyTypes.Length);
                    }

                    int owner_id;
                    //Owner
                    if (rnd.Next(0, 2) == 0) //Если 0 - использовать новое значение
                    {
                        temp = PartOfName.GenerateFIO(firstNames, midNames, lastNames);
                        owner_id = db.owners.Count;
                        db.owners.Add(new Owner(db.Count, temp));
                    }
                    else //Существующее
                    {
                        owner_id = rnd.Next(0, db.owners.Count);
                    }

                    int counter = 0;
                    String shop_name;
                    do
                    {
                        if (counter == 1000)
                            break;
                        shop_name = companies[rnd.Next(0, companies.Length)];
                        counter++;
                    }
                    while (db.shops.Where(x => x.name == shop_name).Count() != 0); //Пока в базе есть магазины с данным

                    Добавить магазин туть
                }
                else //Использовать существующий магазин
                {

                }
            }
        }
    }
}
