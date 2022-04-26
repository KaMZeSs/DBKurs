using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        String[] songNames;
        List<Byte[]> images; //Нужны фото

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
            DeserializeSongName();
            ReadAllImages();
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
        private void DeserializeSongName()
        {
            var arrSerializer = new XmlSerializer(typeof(String[]));

            using (var reader = new StreamReader("Data/SongNames.xml"))
            {
                songNames = (String[])arrSerializer.Deserialize(reader);
            }
        }
        private void ReadAllImages()
        {
            images = new List<byte[]>();
            try
            {
                String[] files = Directory.GetFiles("Data\\images\\", "*.jpg");

                for (int i = 0; i < files.Length; i++)
                {
                    using (var ms = new MemoryStream())
                    {
                        Image img = Image.FromFile(files[i]);
                        img.Save(ms, img.RawFormat);

                        images.Add(ms.ToArray());
                    }
                }
            }
            catch (Exception excpet)
            {
                MessageBox.Show(excpet.Message);
            }
        }

        private DateTime RandomDay(DateTime start)
        {
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rnd.Next(range));
        }
        private DateTime RandomDay(DateTime start, DateTime end)
        {
            int range = (end - start).Days;
            return start.AddDays(rnd.Next(range));
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
                        shop_name = companies[rnd.Next(0, companies.Length)];
                        if (counter > 1000)
                            break;
                        counter++;
                    }
                    while (db.shops.Where(x => x.name == shop_name).Count() != 0); //Пока в базе есть магазины с данным именем

                    String adress = $"{streets[rnd.Next(0, streets.Length)]}, {rnd.Next(0, 300)}";

                    String license = string.Empty;
                    Char[] licPart_arr = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Z', 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z', 'A', 'E', 'U', 'Y', 'a', 'e', 'i', 'o', 'u', 'y' };
                    int license_length = rnd.Next(0, 15);
                    while (license.Length <= license_length)
                        license += licPart_arr[rnd.Next(licPart_arr.Length)];

                    DateTime licenseExpiration = RandomDay(new DateTime(1950, 1, 1));
                    //licenseExpiration = d.ToString("dd-MM-yyyy");

                    int yearOpened = rnd.Next(1950, licenseExpiration.Year);

                    shop_id = db.shops.Count;
                    db.shops.Add(new Shop(
                        id: shop_id, name: shop_name, district_id: district_id, adress: adress, 
                        propertyType_id: propertyType_id, license: license, 
                        licenseExpirationDate: licenseExpiration, 
                        owner_id: owner_id, yearOpened: yearOpened));
                }
                else //Использовать существующий магазин
                {
                    shop_id = db.shops[rnd.Next(0, db.shops.Count)].id;
                }

                int album_id;
                //Album
                if (rnd.Next(0, 4) == 0)
                {
                    int recordFirm_id;
                    //RecordFirm
                    if (rnd.Next(0, 2) == 0) //Использовать новую фирму звукозаписи
                    {
                        int city_id;
                        if (rnd.Next(0, 2) == 0) //Использовать новый город
                        {
                            int country_id;
                            if (rnd.Next(0, 2) == 0) //Использовать новую страну
                            {
                                String country_name = countries[rnd.Next(0, countries.Length)].Name;
                                if (db.countries.Where(x => x.name == country_name).Count() == 0) //Если такой страны нет
                                {
                                    country_id = db.countries.Count;
                                    db.countries.Add(new Randomize.Country(country_id, country_name));
                                }
                                else //Если такая страна уже есть, используем случайную страну, которая уже есть
                                {
                                    country_id = rnd.Next(0, db.countries.Count);
                                }
                            }
                            else
                            {
                                country_id = rnd.Next(0, db.countries.Count);
                            }

                            //Берем случайный город в этой стране
                            int toFindCity = Array.Find(countries, (Generate.Country x) => x.Name == db.countries[country_id].name).id;

                            Generate.City[] cityFromCountry = cities.Where(x => x.country_id == toFindCity).ToArray();
                            int foundCity = rnd.Next(0, cityFromCountry.Length);
                            if (db.cities.Where(x => x.name == cityFromCountry[foundCity].name & x.country_id == country_id).Count() == 0) //Если такого города нет
                            {
                                city_id = db.cities.Count;
                                db.cities.Add(new Randomize.City(city_id, country_id, cityFromCountry[foundCity].name));
                            }
                            else 
                            {
                                city_id = db.cities.Where(x => x.country_id == country_id & x.name == cityFromCountry[foundCity].name).ToArray()[0].id;
                            }
                        }
                        else
                        {
                            city_id = rnd.Next(0, db.cities.Count);
                        }

                        temp = companies[rnd.Next(0, companies.Length)];
                        var t = db.recordFirms.Where(x => x.name == temp & x.city_id == city_id);
                        if (t.Count() == 0)
                        {
                            recordFirm_id = db.recordFirms.Count;
                            db.recordFirms.Add(new Randomize.RecordFirm(recordFirm_id, city_id, temp));
                        }
                        else
                        {
                            recordFirm_id = t.First().id;
                        }
                    }
                    else
                    {
                        recordFirm_id = rnd.Next(0, db.recordFirms.Count);
                    }

                    int genre_id;
                    //Genre
                    String tempGenre = genres[rnd.Next(0, genres.Length)];
                    if (db.genres.Where(x => x.name == tempGenre).Count() == 0)
                    {
                        genre_id = db.genres.Count;
                        db.genres.Add(new Genre(genre_id, tempGenre));
                    }
                    else
                    {
                        genre_id = db.genres.Where(x => x.name == tempGenre).First().id;
                    }


                    //Нужно добавить проверку на сборник (неск исполнителей)
                    int executor_id;
                    //Executor
                    if (rnd.Next(0, 4) == 0) //Если 0 - новый исполнитель
                    {
                        temp = executors[rnd.Next(executors.Length)];
                        if (db.executors.Where(x => x.name == temp).Count() == 0)
                        {
                            executor_id = db.executors.Count;
                            db.executors.Add(new Executor(executor_id, temp));
                        }
                        else
                        {
                            executor_id = rnd.Next(0, db.executors.Count);
                        }
                    }
                    else
                    {
                        executor_id = rnd.Next(0, db.executors.Count);
                    }

                    int language_id;
                    //Language
                    if (rnd.Next(0, 4) == 0) //Если 0 - новый язык
                    {
                        temp = languages[rnd.Next(languages.Length)];
                        if (db.languages.Where(x => x.name == temp).Count() == 0)
                        {
                            language_id = db.languages.Count;
                            db.languages.Add(new Language(language_id, temp));
                        }
                        else
                        {
                            language_id = rnd.Next(0, db.languages.Count);
                        }
                    }
                    else
                    {
                        language_id = rnd.Next(0, db.languages.Count);
                    }

                    int recordType_id;
                    //RecordType
                    temp = recordTypes[rnd.Next(0, recordTypes.Length)];
                    if (db.recordTypes.Where(x => x.name == temp).Count() == 0)
                    {
                        recordType_id = db.recordTypes.Count;
                        db.recordTypes.Add(new RecordType(recordType_id, temp));
                    }
                    else
                    {
                        recordType_id = rnd.Next(0, db.recordTypes.Count);
                    }

                    //AlbumName
                    String albumName;
                    int counter = 0;
                    do
                    {
                        albumName = songNames[rnd.Next(0, songNames.Length)];
                        counter++;
                        if (counter > 1000)
                            break;
                    } 
                    while (db.albums.Where(x => x.name == albumName).Count() != 0);

                    //AlbumReleaseDate
                    DateTime releaseDate = RandomDay(new DateTime(1950, 1, 1));

                    //AlbumAmount
                    int amount = rnd.Next(1000, 10000000);

                    //SongCount
                    int songCount = rnd.Next(1, 100);

                    //isCompilation
                    bool isCompilation = rnd.Next(0, 2) == 0;

                    //Если сборник - нужен еще один исполнитель
                    int secondExecutor_id = -1;
                    //Информация (если два исполнителя)
                    String info = string.Empty;
                    if (isCompilation)
                    {
                        int temp_counter = 0;
                        do
                        {
                            if (temp_counter > 1000)
                                break;
                            secondExecutor_id = rnd.Next(0, db.executors.Count);
                            temp_counter++;
                        } while (secondExecutor_id == executor_id);

                        info = $"Исполнители: {db.executors.Where(x => x.id == executor_id).First().name}, {db.executors.Where(x => x.id == secondExecutor_id).First().name}\n" + 
                            $"Жанр: {db.genres.Where(x => x.id == genre_id).First().name}\nЯзык: {db.languages.Where(x => x.id == language_id).First().name}";
                    }

                    //Изображение
                    byte[] image = images[rnd.Next(0, images.Count)];

                    //Время звучания альбома
                    int albumLength = rnd.Next(songCount, songCount * 10);

                    album_id = db.albums.Count;

                    db.albums.Add(new Album(id: album_id, name: albumName, recordFirm_id: recordFirm_id,
                        releaseDate: releaseDate, amount: amount, songsCount: songCount,
                        recordType_id: recordType_id, isCompilation: isCompilation,
                        executor_id: executor_id, genre_id: genre_id, language_id: language_id, 
                        info: info, photo: image, time: albumLength));
                }
                else
                {
                    album_id = db.albums[rnd.Next(0, db.albums.Count)].id;
                }

                //ProductRange



                //DateTime gotAlbum = RandomDay()
            }
        }
    }
}
