using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public String[] GenerateComands()
        {
            var commands = new List<string>();

            //To albums
            for (int i = 0; i < countries.Count; i++)
            {
                commands.Add(countries[i].ToString());
            }
            for (int i = 0; i < cities.Count; i++)
            {
                commands.Add(cities[i].ToString());
            }
            for (int i = 0; i < recordFirms.Count; i++)
            {
                commands.Add(recordFirms[i].ToString());
            }
            for (int i = 0; i < genres.Count; i++)
            {
                commands.Add(genres[i].ToString());
            }
            for (int i = 0; i < executors.Count; i++)
            {
                commands.Add(executors[i].ToString());
            }
            for (int i = 0; i < languages.Count; i++)
            {
                commands.Add(languages[i].ToString());
            }
            for (int i = 0; i < recordTypes.Count; i++)
            {
                commands.Add(recordTypes[i].ToString());
            }

            //To shops
            for (int i = 0; i < districts.Count; i++)
            {
                commands.Add(districts[i].ToString());
            }
            for (int i = 0; i < propertyTypes.Count; i++)
            {
                commands.Add(propertyTypes[i].ToString());
            }
            for (int i = 0; i < owners.Count; i++)
            {
                commands.Add(owners[i].ToString());
            }

            //To productRanges
            for (int i = 0; i < shops.Count; i++)
            {
                commands.Add(shops[i].ToString());
            }
            for (int i = 0; i < albums.Count; i++)
            {
                commands.Add(albums[i].ToString());
            }

            for (int i = 0; i < productRanges.Count; i++)
            {
                commands.Add(productRanges[i].ToString());
            }

            return commands.ToArray();
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

        override public String ToString()
        {
            String receipt = receiptDate.ToString("dd-MM-yyyy");
            return "INSERT INTO ProductRanges (productRange_id, shop_id, album_id, dateOfReceipt, amount) " + 
                $"VALUES ({id}, {shop_id}, {album_id}, \'{receipt}\', {amount})";
        }
    }

    class Shop
    {
        public int id; 
        public String name;
        public int district_id;
        public String adress;
        public int propertyType_id;
        public String license;
        public DateTime licenseExpirationDate;
        public int owner_id;
        public int yearOpened;

        public Shop(int id, String name, int district_id, String adress, int propertyType_id, String license, DateTime licenseExpirationDate, int owner_id, int yearOpened)
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
            String expiry = licenseExpirationDate.ToString("dd-MM-yyyy");
            return "INSERT INTO Shops (shop_id, district_id, propertyType_id, owner_id, shop_name, addres, license, expiryDate, yearOpened) " + 
                $"VALUES ({id}, {district_id}, {propertyType_id}, {owner_id}, \'{name}\', \'{adress}\', \'{license}\', \'{expiry}\', {yearOpened})";
        }
    }
    class District
    {
        public int id;
        public String name;

        public District(int id, string name)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"INSERT INTO Districts (district_id, district_name) VALUES ({id}, \'{name}\')";
        }
    }
    class PropertyType
    {
        int id;
        public String type;

        public PropertyType(int id, string type)
        {
            this.id = id;
            this.type = type;
        }

        public override string ToString()
        {
            return $"INSERT INTO PropertyTypes (propertyType_id, propertyType_name) VALUES ({id}, \'{type}\')";
        }
    }
    class Owner
    {
        public int id;
        public String FIO;

        public Owner(int id, string fIO)
        {
            this.id = id;
            FIO = fIO;
        }

        public override string ToString()
        {
            return $"INSERT INTO Owners (owner_id, owner_name) VALUES ({id}, \'{FIO}\')";
        }
    }

    class Album
    {
        public int id; 
        public String name;
        public int recordFirm_id;
        public DateTime releaseDate;
        public int amount;
        public int songsCount;
        public int recordType_id;
        public bool isCompilation;
        public int executor_id;
        public int genre_id;
        public int language_id;
        public String info;
        public Byte[] photo;
        public int time;

        public Album(int id, string name, int recordFirm_id, DateTime releaseDate, int amount, int songsCount, int recordType_id, bool isCompilation, int executor_id, int genre_id, int language_id, string info, Byte[] photo, int time)
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
        }

        public override string ToString()
        {
            String release = releaseDate.ToString("dd-MM-yyyy");
            String isCol = isCompilation ? "true" : "false";
            String img = Convert.ToBase64String(photo);
            return "INSERT INTO Albums (album_id, recordFirm_id, genre_id, executor_id, language_id, recordType_id, album_name, releaseDate, albumCount, songsCount, isCollection, albumInfo, Photo, albumTime) " + 
                $"VALUES ({id}, {recordFirm_id}, {genre_id}, {executor_id}, {language_id}, {recordType_id}, \'{name}\', \'{release}\', {amount}, {songsCount}, \'{isCol}\', \'{info}\', \'{img}\', {time})";
        }
    }
    class Country
    {
        public int id;
        public String name;

        public Country(int id, string name)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"INSERT INTO Countries (country_id, country_name) VALUES ({id}, \'{name}\')";
        }
    }
    class City
    {
        public int id;
        public int country_id;
        public String name;

        public City(int id, int country_id, string name)
        {
            this.id = id;
            this.country_id = country_id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"INSERT INTO Cities (city_id, country_id, city_name) VALUES ({id}, {country_id}, \'{name}\')";
        }
    }
    class RecordFirm
    {
        public int id;
        public int city_id;
        public String name;

        public RecordFirm(int id, int city_id, string name)
        {
            this.id = id;
            this.city_id = city_id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"INSERT INTO RecordFirms (recordFirm_id, city_id, recordFirm_name) VALUES ({id}, {city_id}, \'{name}\')";
        }
    }
    class Genre
    {
        public int id;
        public String name;

        public Genre(int id, string name)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"INSERT INTO Genres (genre_id, genre_name) VALUES ({id}, \'{name}\')";
        }
    }
    class Executor
    {
        public int id;
        public String name;

        public Executor(int id, string name)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"INSERT INTO Executors (executor_id, executor_name) VALUES ({id}, \'{name}\')";
        }
    }
    class Language
    {
        public int id;
        public String name;

        public Language(int id, string name)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
        }

        public override string ToString()
        {
            return $"INSERT INTO Languages (language_id, language_name) VALUES ({id}, \'{name}\')";
        }
    }
    class RecordType
    {
        public int id; 
        public String name;

        public RecordType(int id, string name)
        {
            this.id = id;
            this.name = name.Replace("\'", "\'\'");
        }
        public override string ToString()
        {
            return $"INSERT INTO RecordTypes (recordType_id, recordType_name) VALUES ({id}, \'{name}\')";
        }
    }
}