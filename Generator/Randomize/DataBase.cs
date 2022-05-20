using System;
using System.Collections.Generic;

namespace Randomize
{
    class DataBase
    {
        public List<ProductRange> productRanges;
        public List<Shop> shops;
        public List<District> districts;
        public List<PropertyType> propertyTypes;
        public List<Owner> owners;
        public List<Album> albums;
        public List<Country> countries;
        public List<City> cities;
        public List<RecordFirm> recordFirms;
        public List<Genre> genres;
        public List<Executor> executors;
        public List<Language> languages;
        public List<RecordType> recordTypes;

        public int Count
        {
            get { return productRanges.Count; }
        }

        public DataBase()
        {
            productRanges = new List<ProductRange>();
            shops = new List<Shop>();
            districts = new List<District>();
            propertyTypes = new List<PropertyType>();
            owners = new List<Owner>();
            albums = new List<Album>();
            countries = new List<Country>();
            cities = new List<City>();
            recordFirms = new List<RecordFirm>();
            genres = new List<Genre>();
            executors = new List<Executor>();
            languages = new List<Language>();
            recordTypes = new List<RecordType>();
        }

        public List<String[]> GenerateComands()
        {
            var result = new List<String[]>();
            var commands = new List<string>();

            //To albums
            for (int i = 0; i < countries.Count; i++)
            {
                commands.Add(countries[i].ToString());
            }
            result.Add(commands.ToArray());
            commands = new List<string>();

            for (int i = 0; i < cities.Count; i++)
            {
                commands.Add(cities[i].ToString());
            }
            result.Add(commands.ToArray());
            commands = new List<string>();

            for (int i = 0; i < recordFirms.Count; i++)
            {
                commands.Add(recordFirms[i].ToString());
            }
            result.Add(commands.ToArray());
            commands = new List<string>();

            for (int i = 0; i < genres.Count; i++)
            {
                commands.Add(genres[i].ToString());
            }
            result.Add(commands.ToArray());
            commands = new List<string>();

            for (int i = 0; i < executors.Count; i++)
            {
                commands.Add(executors[i].ToString());
            }
            result.Add(commands.ToArray());
            commands = new List<string>();

            for (int i = 0; i < languages.Count; i++)
            {
                commands.Add(languages[i].ToString());
            }
            result.Add(commands.ToArray());
            commands = new List<string>();

            for (int i = 0; i < recordTypes.Count; i++)
            {
                commands.Add(recordTypes[i].ToString());
            }
            result.Add(commands.ToArray());
            commands = new List<string>();

            //To shops
            for (int i = 0; i < districts.Count; i++)
            {
                commands.Add(districts[i].ToString());
            }
            result.Add(commands.ToArray());
            commands = new List<string>();

            for (int i = 0; i < propertyTypes.Count; i++)
            {
                commands.Add(propertyTypes[i].ToString());
            }
            result.Add(commands.ToArray());
            commands = new List<string>();

            for (int i = 0; i < owners.Count; i++)
            {
                commands.Add(owners[i].ToString());
            }
            result.Add(commands.ToArray());
            commands = new List<string>();

            //To productRanges
            for (int i = 0; i < shops.Count; i++)
            {
                commands.Add(shops[i].ToString());
            }
            result.Add(commands.ToArray());
            commands = new List<string>();

            for (int i = 0; i < albums.Count; i++)
            {
                commands.Add(albums[i].ToString());
            }
            result.Add(commands.ToArray());
            commands = new List<string>();

            for (int i = 0; i < productRanges.Count; i++)
            {
                commands.Add(productRanges[i].ToString());
            }
            result.Add(commands.ToArray());
            commands = new List<string>();

            return result;
        }
    }

    class ProductRange
    {
        public int id;
        public int shop_id;
        public int album_id;
        public DateTime receiptDate;
        public int amount;

        public ProductRange(int id, int shop_id, int album_id, DateTime receiptDate, int amount)
        {
            this.id = id;
            this.shop_id = shop_id;
            this.album_id = album_id;
            this.receiptDate = receiptDate;
            this.amount = amount;
        }

        override public string ToString()
        {
            string receipt = receiptDate.ToString("dd-MM-yyyy");
            return $"({id + 1}, {shop_id + 1}, {album_id + 1}, \'{receipt}\', {amount})";
        }
    }

    class Shop
    {
        public int id;
        public string name;
        public int district_id;
        public string adress;
        public int propertyType_id;
        public string license;
        public DateTime licenseExpirationDate;
        public int owner_id;
        public int yearOpened;

        public Shop(int id, string name, int district_id, string adress, int propertyType_id, string license, DateTime licenseExpirationDate, int owner_id, int yearOpened)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
            this.district_id = district_id;
            this.adress = adress.Replace("\'", "\'\'");
            this.propertyType_id = propertyType_id;
            this.license = license;
            this.licenseExpirationDate = licenseExpirationDate;
            this.owner_id = owner_id;
            this.yearOpened = yearOpened;
        }

        public override string ToString()
        {
            string expiry = licenseExpirationDate.ToString("dd-MM-yyyy");
            return $"({id + 1}, {district_id + 1}, {propertyType_id + 1}, {owner_id + 1}, \'{name}\', \'{adress}\', \'{license}\', \'{expiry}\', {yearOpened})";
        }
    }
    class District
    {
        public int id;
        public string name;

        public District(int id, string name)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"({id + 1}, \'{name}\')";
        }
    }
    class PropertyType
    {
        int id;
        public string type;

        public PropertyType(int id, string type)
        {
            this.id = id;
            this.type = type;
        }

        public override string ToString()
        {
            return $"({id + 1}, \'{type}\')";
        }
    }
    class Owner
    {
        public int id;
        public string FIO;

        public Owner(int id, string fIO)
        {
            this.id = id;
            FIO = fIO;
        }

        public override string ToString()
        {
            return $"({id + 1}, \'{FIO}\')";
        }
    }

    class Album
    {
        public int id;
        public string name;
        public int recordFirm_id;
        public DateTime releaseDate;
        public int amount;
        public int songsCount;
        public int recordType_id;
        public bool isCompilation;
        public int executor_id;
        public int genre_id;
        public int language_id;
        public string info;
        public byte[] photo;
        public int time;
        public int[] executors;

        public Album(int id, string name, int recordFirm_id, DateTime releaseDate, int amount, int songsCount, int recordType_id, bool isCompilation, int executor_id, int genre_id, int language_id, string info, byte[] photo, int time, int[] executors)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
            this.recordFirm_id = recordFirm_id;
            this.releaseDate = releaseDate;
            this.amount = amount;
            this.songsCount = songsCount;
            this.recordType_id = recordType_id;
            this.isCompilation = isCompilation;
            this.executor_id = executor_id;
            this.genre_id = genre_id;
            this.language_id = language_id;
            this.info = info;
            this.photo = photo;
            this.time = time;
            this.executors = executors;
        }

        public override string ToString()
        {
            string release = releaseDate.ToString("dd-MM-yyyy");
            string isCol = isCompilation ? "true" : "false";
            string img = Convert.ToBase64String(photo);

            return isCompilation
                ? $"({id + 1}, {recordFirm_id + 1}, {genre_id + 1}, NULL, {language_id + 1}, {recordType_id + 1}, \'{name}\', \'{release}\', {amount}, {songsCount}, \'{isCol}\', \'{info}\', \'{img}\', {time}, '{{{executor_id + 1}, {executors[0] + 1}}}')"
                : $"({id + 1}, {recordFirm_id + 1}, {genre_id + 1}, {executor_id + 1}, {language_id + 1}, {recordType_id + 1}, \'{name}\', \'{release}\', {amount}, {songsCount}, \'{isCol}\', NULL, \'{img}\', {time}, NULL)";
        }
    }
    class Country
    {
        public int id;
        public string name;

        public Country(int id, string name)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"({id + 1}, \'{name}\')";
        }
    }
    class City
    {
        public int id;
        public int country_id;
        public string name;

        public City(int id, int country_id, string name)
        {
            this.id = id;
            this.country_id = country_id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"({id + 1}, {country_id + 1}, \'{name}\')";
        }
    }
    class RecordFirm
    {
        public int id;
        public int city_id;
        public string name;

        public RecordFirm(int id, int city_id, string name)
        {
            this.id = id;
            this.city_id = city_id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"({id + 1}, {city_id + 1}, \'{name}\')";
        }
    }
    class Genre
    {
        public int id;
        public string name;

        public Genre(int id, string name)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"({id + 1}, \'{name}\')";
        }
    }
    class Executor
    {
        public int id;
        public string name;

        public Executor(int id, string name)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"({id + 1}, \'{name}\')";
        }
    }
    class Language
    {
        public int id;
        public string name;

        public Language(int id, string name)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"({id + 1}, \'{name}\')";
        }
    }
    class RecordType
    {
        public int id;
        public string name;

        public RecordType(int id, string name)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
        }
        public override string ToString()
        {
            return $"({id + 1}, \'{name}\')";
        }
    }
}