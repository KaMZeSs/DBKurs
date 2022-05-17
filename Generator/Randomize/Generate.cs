using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Npgsql;

namespace Randomize
{
    public class Generate
    {
        City[] cities;
        Country[] countries;
        PartOfName[] firstNames, midNames, lastNames;
        string[] propertyTypes;
        string[] districts;
        string[] genres;
        string[] languages;
        string[] recordTypes;
        string[] executors;
        string[] companies; // Магазин и фирма звукозаписи
        string[] streets;
        string[] songNames;
        List<byte[]> images; //Нужны фото

        public class City
        {
            public int city_id;
            public int country_id;
            public string name;
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
            public string name;
        }

        Generator.SetInfo writeInfo;
        public Generate(Generator.SetInfo set)
        {
            writeInfo = set;
            writeInfo.Invoke(DateTime.Now, "Начало работы");

            this.DeserialaizeCities();
            this.DeserializeCounties();
            this.DeserializePartsOfName();
            this.DeserializeDistricts();
            this.DeserializePropertyTypes();
            this.DeserializeGenres();
            this.DeserializeLanguages();
            this.DeserializeRecordTypes();
            this.DeserializeExecutors();
            this.DeserializeCompanies();
            this.DeserializeStreets();
            this.DeserializeSongName();
            this.ReadAllImages();
            writeInfo.Invoke(DateTime.Now, "Все данные для работы загружены");
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
            var arrSerializer = new XmlSerializer(typeof(string[]));

            using (var reader = new StreamReader("Data/PropertyTypes.xml"))
            {
                propertyTypes = (string[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeDistricts()
        {
            var arrSerializer = new XmlSerializer(typeof(string[]));

            using (var reader = new StreamReader("Data/Districts.xml"))
            {
                districts = (string[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeGenres()
        {
            var arrSerializer = new XmlSerializer(typeof(string[]));

            using (var reader = new StreamReader("Data/Genres.xml"))
            {
                genres = (string[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeLanguages()
        {
            var arrSerializer = new XmlSerializer(typeof(string[]));

            using (var reader = new StreamReader("Data/Languages.xml"))
            {
                languages = (string[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeRecordTypes()
        {
            var arrSerializer = new XmlSerializer(typeof(string[]));

            using (var reader = new StreamReader("Data/RecordTypes.xml"))
            {
                recordTypes = (string[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeExecutors()
        {
            var arrSerializer = new XmlSerializer(typeof(string[]));

            using (var reader = new StreamReader("Data/Executors.xml"))
            {
                executors = (string[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeCompanies()
        {
            var arrSerializer = new XmlSerializer(typeof(string[]));

            using (var reader = new StreamReader("Data/Companies.xml"))
            {
                companies = (string[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeStreets()
        {
            var arrSerializer = new XmlSerializer(typeof(string[]));

            using (var reader = new StreamReader("Data/Streets.xml"))
            {
                streets = (string[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserializeSongName()
        {

            using (var reader = new StreamReader("Data/songs.txt"))
            {
                songNames = reader.ReadToEnd().Trim().Split("\n");
            }
        }
        private void ReadAllImages()
        {
            images = new List<byte[]>();
            try
            {
                string[] files = Directory.GetFiles("Data\\images\\", "*.jpg");

                for (int i = 0; i < files.Length; i++)
                {
                    using (var ms = new MemoryStream())
                    {
                        var img = Image.FromFile(files[i]);
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
        DataBase db;
        public Generate Generation(int len = 10000, int minNew = 1000)
        {
            writeInfo.Invoke(DateTime.Now, "Начало генерации структуры БД");
            db = new DataBase();

            string temp;
            for (int i = 0; i < len; i++)
            {
                int album_id;
                //Album
                if (i < minNew | rnd.Next(0, 4) == 0)
                {
                    int recordFirm_id;
                    //RecordFirm
                    if (i < minNew | rnd.Next(0, 2) == 0) //Использовать новую фирму звукозаписи
                    {
                        int city_id;
                        if (i < minNew | rnd.Next(0, 2) == 0) //Использовать новый город
                        {
                            int country_id;
                            if (i < minNew | rnd.Next(0, 2) == 0) //Использовать новую страну
                            {
                                string country_name = countries[rnd.Next(0, countries.Length)].Name;
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
                        IEnumerable<Randomize.RecordFirm> t = db.recordFirms.Where(x => x.name == temp & x.city_id == city_id);
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
                    string tempGenre = genres[rnd.Next(0, genres.Length)];
                    if (db.genres.Where(x => x.name == tempGenre).Count() == 0)
                    {
                        genre_id = db.genres.Count;
                        db.genres.Add(new Genre(genre_id, tempGenre));
                    }
                    else
                    {
                        genre_id = db.genres.Where(x => x.name == tempGenre).First().id;
                    }

                    int executor_id;
                    //Executor
                    if (i < minNew | rnd.Next(0, 20) == 0) //Если 0 - новый исполнитель
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
                    if (i < minNew | rnd.Next(0, 4) == 0) //Если 0 - новый язык
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
                    string albumName;
                    int counter = 0;
                    do
                    {
                        albumName = songNames[rnd.Next(0, songNames.Length)];
                        counter++;
                        if (counter > 100)
                        {
                            albumName = executors[rnd.Next(0, executors.Length)];
                            break;
                        }

                    }
                    while (db.albums.Where(x => x.name.Equals(albumName)).Count() != 0);

                    //AlbumReleaseDate
                    DateTime releaseDate = this.RandomDay(new DateTime(1950, 1, 1));

                    //AlbumAmount
                    int album_amount = rnd.Next(1000, 15000000);

                    //SongCount
                    int songCount = rnd.Next(1, 100);

                    //isCompilation
                    bool isCompilation = rnd.Next(0, 4) == 0;
                    if (db.executors.Count == 1)
                        isCompilation = false;

                    //Если сборник - нужен еще один исполнитель
                    int secondExecutor_id = -1;
                    //Информация (если два исполнителя)
                    string info = String.Empty;
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
                        releaseDate: releaseDate, amount: album_amount, songsCount: songCount,
                        recordType_id: recordType_id, isCompilation: isCompilation,
                        executor_id: executor_id, genre_id: genre_id, language_id: language_id,
                        info: info, photo: image, time: albumLength));
                }
                else
                {
                    album_id = db.albums[rnd.Next(0, db.albums.Count)].id;
                }

                int shop_id;
                //Shop
                if (i < minNew | rnd.Next(0, 4) == 0) // Если 0 - использовать новое значение
                {
                    int district_id;
                    //District
                    if (i < minNew | rnd.Next(0, 20) == 0) //Если 0 - использовать новое значение
                    {
                        temp = districts[rnd.Next(0, districts.Length)];
                        if (db.districts.Where(x => x.name == temp).Count() == 0) //Если попался тот, которого еще не было, то используем его
                        {
                            district_id = db.districts.Count;
                            db.districts.Add(new District(db.districts.Count, temp));
                        }
                        else //если он уже был, берем случайный из тех, что уже были
                        {
                            district_id = rnd.Next(0, db.districts.Count);
                        }
                    }
                    else //Существующее
                    {
                        district_id = rnd.Next(0, db.districts.Count);
                    }

                    int propertyType_id;
                    //PropertyType
                    if (i < minNew | rnd.Next(0, 2) == 0) //Если 0 - использовать новое значение
                    {
                        temp = propertyTypes[rnd.Next(0, propertyTypes.Length)];
                        if (db.propertyTypes.Where(x => x.type == temp).Count() == 0)
                        {
                            propertyType_id = db.propertyTypes.Count;
                            db.propertyTypes.Add(new PropertyType(db.propertyTypes.Count, temp));
                        }
                        else
                        {
                            propertyType_id = rnd.Next(0, db.propertyTypes.Count);
                        }
                        if (propertyType_id == 4)
                            MessageBox.Show("");
                    }
                    else //Существующее
                    {
                        propertyType_id = rnd.Next(0, db.propertyTypes.Count);
                    }

                    int owner_id;
                    //Owner
                    if (i < minNew | rnd.Next(0, 2) == 0) //Если 0 - использовать новое значение
                    {
                        temp = PartOfName.GenerateFIO(firstNames, midNames, lastNames);
                        owner_id = db.owners.Count;
                        db.owners.Add(new Owner(owner_id, temp));
                    }
                    else //Существующее
                    {
                        owner_id = rnd.Next(0, db.owners.Count);
                    }

                    int counter = 0;
                    string shop_name;
                    do
                    {
                        shop_name = companies[rnd.Next(0, companies.Length)];
                        if (counter > 1000)
                            break;
                        counter++;
                    }
                    while (db.shops.Where(x => x.name == shop_name).Count() != 0); //Пока в базе есть магазины с данным именем

                    string adress = $"{streets[rnd.Next(0, streets.Length)]}, {rnd.Next(0, 300)}";

                    string license = String.Empty;
                    char[] licPart_arr = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Z', 'A', 'E', 'U', 'Y' };
                    int license_length = rnd.Next(5, 15);
                    while (license.Length <= license_length)
                        license += licPart_arr[rnd.Next(licPart_arr.Length)];


                    int yearOpened = rnd.Next(1900, db.albums[album_id].releaseDate.Year - 1);

                    DateTime licenseExpiration = this.RandomDay(DateTime.Now, DateTime.Now.AddYears(10));

                    shop_id = db.shops.Count;
                    db.shops.Add(new Shop(
                        id: shop_id, name: shop_name, district_id: district_id, adress: adress,
                        propertyType_id: propertyType_id, license: license,
                        licenseExpirationDate: licenseExpiration,
                        owner_id: owner_id, yearOpened: yearOpened));
                }
                else //Использовать существующий магазин
                {
                    DateTime album_release = db.albums[album_id].releaseDate;
                    Shop[] shops = db.shops.Where(x => x.yearOpened < album_release.Year & x.licenseExpirationDate > album_release).ToArray();
                    if (shops.Length == 0)
                    {
                        int district_id = rnd.Next(0, db.districts.Count);
                        int propertyType_id = rnd.Next(0, db.propertyTypes.Count);
                        int owner_id = rnd.Next(0, db.owners.Count);
                        string shop_name;
                        int counter = 0;
                        do
                        {
                            shop_name = companies[rnd.Next(0, companies.Length)];
                            if (counter > 1000)
                                break;
                            counter++;
                        }
                        while (db.shops.Where(x => x.name == shop_name).Count() != 0);
                        string adress = $"{streets[rnd.Next(0, streets.Length)]}, {rnd.Next(0, 300)}";
                        string license = String.Empty;
                        char[] licPart_arr = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Z', 'A', 'E', 'U', 'Y' };
                        int license_length = rnd.Next(5, 15);
                        while (license.Length <= license_length)
                            license += licPart_arr[rnd.Next(licPart_arr.Length)];
                        int yearOpened = rnd.Next(1900, db.albums[album_id].releaseDate.Year - 1);

                        DateTime licenseExpiration = this.RandomDay(db.albums[album_id].releaseDate, DateTime.Now.AddYears(10));

                        shop_id = db.shops.Count;
                        db.shops.Add(new Shop(
                            id: shop_id, name: shop_name, district_id: district_id, adress: adress,
                            propertyType_id: propertyType_id, license: license,
                            licenseExpirationDate: licenseExpiration,
                            owner_id: owner_id, yearOpened: yearOpened));
                    }
                    else
                    {
                        shop_id = shops[rnd.Next(0, shops.Length)].id;
                    }
                }

                //ProductRange
                DateTime a = db.albums[album_id].releaseDate;
                var b = new DateTime(db.shops[shop_id].yearOpened, 1, 1);

                DateTime c = db.shops[shop_id].licenseExpirationDate;
                DateTime d = DateTime.Now;

                DateTime gotAlbum = this.RandomDay(a > b ? a : b, c < d ? c : d);

                int albumCount = db.albums[album_id].amount;
                int albumInShops = 0;

                ProductRange[] rangesWithThatAlbum = db.productRanges.Where(x => x.album_id == album_id).ToArray();

                for (int j = 0; j < rangesWithThatAlbum.Length; j++)
                    albumInShops += rangesWithThatAlbum[j].amount;

                int amount = rnd.Next(0, albumCount - albumInShops);

                db.productRanges.Add(new ProductRange(id: db.productRanges.Count, shop_id: shop_id,
                    album_id: album_id, receiptDate: gotAlbum, amount: amount));
            }

            writeInfo.Invoke(DateTime.Now, "Конец генерации структуры БД");
            return this;
        }

        public void Send(string connectString = "Host=localhost;Port=5432;User Id=postgres;Password=1310;Database=Kurs")
        {
            var conn = new NpgsqlConnection(connectString);
            string sql;
            NpgsqlCommand cmd;
            try
            {
                writeInfo.Invoke(DateTime.Now, "Установление связи с сервером");

                conn.Open();

                writeInfo.Invoke(DateTime.Now, "Очистка данных на сервере");
                //очистка данных на сервере
                string[] toClear = { "DELETE FROM ProductRanges", "DELETE FROM albums", "DELETE FROM shops",
                    "DELETE FROM countries", "DELETE FROM cities", "DELETE FROM executors", "DELETE FROM genres",
                    "DELETE FROM languages", "DELETE FROM recordfirms", "DELETE FROM recordtypes",
                    "DELETE FROM districts", "DELETE FROM owners", "DELETE FROM propertytypes" };

                for (int i = 0; i < toClear.Length; i++)
                {
                    cmd = new NpgsqlCommand(toClear[i], conn);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                }

                writeInfo.Invoke(DateTime.Now, "Генерация команд для записи на сервер");

                string[] commands = db.GenerateComands();

                writeInfo.Invoke(DateTime.Now, "Выполнение команд на сервере");

                for (int i = 0; i < commands.Length; i++)
                {
                    sql = commands[i];

                    cmd = new NpgsqlCommand(sql, conn);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
                writeInfo.Invoke(DateTime.Now, "Ошибка на сервере: " + except.Message);
            }
            finally
            {
                conn.Close();
                writeInfo.Invoke(DateTime.Now, "Генерация завершена");
            }
        }
    }
}
