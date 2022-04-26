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
    }

    class ProductRange
    {
        public int id;
        public int shop_id;
        public int album_id;
        public String receiptDate;
        public int amount;

        public ProductRange(int id, int shop_id, int album_id, string receiptDate, int amount)
        {
            this.id = id;
            this.shop_id = shop_id;
            this.album_id = album_id;
            this.receiptDate = receiptDate;
            this.amount = amount;
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

        public Shop(int id, string name, int district_id, string adress, int propertyType_id, string license, DateTime licenseExpirationDate, int owner_id, int yearOpened)
        {
            this.id = id;
            this.name = name;
            this.district_id = district_id;
            this.adress = adress;
            this.propertyType_id = propertyType_id;
            this.license = license;
            this.licenseExpirationDate = licenseExpirationDate;
            this.owner_id = owner_id;
            this.yearOpened = yearOpened;
        }
    }
    class District
    {
        public int id;
        public String name;

        public District(int id, string name)
        {
            this.id = id;
            this.name = name;
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
            this.name = name;
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
    }
    class Country
    {
        public int id;
        public String name;

        public Country(int id, string name)
        {
            this.id = id;
            this.name = name;
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
            this.name = name;
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
            this.name = name;
        }
    }
    class Genre
    {
        public int id;
        public String name;

        public Genre(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
    class Executor
    {
        public int id;
        public String name;

        public Executor(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
    class Language
    {
        public int id;
        public String name;

        public Language(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
    class RecordType
    {
        public int id; 
        public String name;

        public RecordType(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}