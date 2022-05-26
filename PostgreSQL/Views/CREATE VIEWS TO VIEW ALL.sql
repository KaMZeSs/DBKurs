CREATE OR REPLACE VIEW Show_shops AS
    SELECT Shops.shop_id AS "id",
        Shops.shop_name AS "Название магазина",
        Districts.district_name AS "Район города",
        Shops.addres AS "Адрес",
        PropertyTypes.propertyType_name AS "Тип собственности",
        Shops.license AS "Лицензия",
        Shops.expiryDate AS "Дата окончания лицензии",
        Owners.owner_name AS "Владелец",
        Shops.yearOpened AS "Год открытия"
    FROM Shops
        JOIN Districts USING(district_id)
        JOIN Owners USING(owner_id)
        JOIN PropertyTypes USING(propertyType_id)
    ORDER BY Shops.shop_id;

CREATE OR REPLACE VIEW Show_productranges AS
    SELECT ProductRanges.productRange_id AS "id",
        Shops.shop_name AS "Магазин",
        albums.album_name AS "Альбом",
        ProductRanges.dateOfReceipt AS "Дата поступления",
        ProductRanges.amount AS "Количество единиц"
    FROM ProductRanges
        JOIN Shops USING(shop_id)
        JOIN Albums USING(album_id)
    ORDER BY ProductRanges.productRange_id;

CREATE OR REPLACE VIEW Show_albums AS
    SELECT 
        Albums.album_id AS "id",
        Albums.album_name AS "Название альбома",
        RecordFirms.recordFirm_name AS "Фирма звукозаписи",
        Albums.releaseDate AS "Дата выпуска альбома",
        Albums.albumCount AS "Тираж албома",
        Albums.songsCount AS "Количество песен",
        RecordTypes.recordType_name AS "Тип записи",
        Albums.isCollection AS "Альбом сборник",
        Executors.executor_name AS "Исполнитель",
        Genres.genre_name AS "Жанр",
        Languages.language_name AS "Язык исполнения",
        Albums.albumInfo AS "Информация",
        Albums.Photo AS "Титул альбома",
        Albums.albumTime AS "Время звучания"
    FROM Albums
        FULL JOIN Executors USING(executor_id)
        FULL JOIN Genres USING(genre_id)
        FULL JOIN Languages USING(language_id)
        JOIN RecordFirms USING(recordFirm_id)
        JOIN RecordTypes USING(recordType_id)
    ORDER BY Albums.album_id;

CREATE OR REPLACE VIEW Show_cities AS
    SELECT 
        Cities.city_id AS "id",
        Countries.country_name AS "Страна",
        Cities.city_name AS "Город"
    FROM Cities
        JOIN Countries USING(country_id)
    ORDER BY Cities.city_id;

CREATE OR REPLACE VIEW Show_countries AS
    SELECT 
        country_id AS "id",
        country_name AS "Страна"
    FROM COUNTRIES
    ORDER BY country_id;

SELECT * FROM Show_countries WHERE "id" = 1

CREATE OR REPLACE VIEW Show_executors AS
    SELECT 
        executor_id AS "id",
        executor_name AS "Исполнитель"
    FROM Executors
    ORDER BY executor_id;

CREATE OR REPLACE VIEW Show_genres AS
    SELECT 
        genre_id AS "id",
        genre_name AS "Жанр"
    FROM Genres
    ORDER BY genre_id;

CREATE OR REPLACE VIEW Show_languages AS
    SELECT 
        language_id AS "id",
        language_name AS "Язык"
    FROM Languages
    ORDER BY language_id;

CREATE OR REPLACE VIEW Show_recordFirms AS
    SELECT 
        RecordFirms.recordFirm_id AS "id",
        Cities.city_name AS "Город",
        RecordFirms.recordFirm_name AS "Фирма звукозаписи"
    FROM RecordFirms
        JOIN Cities USING(city_id)
    ORDER BY RecordFirms.recordFirm_id;

CREATE OR REPLACE VIEW Show_recordTypes AS
    SELECT recordtype_id AS "id",
        recordtype_name AS "Тип записи"
    FROM RecordTypes
    ORDER BY recordType_id;

CREATE OR REPLACE VIEW Show_districts AS
    SELECT 
        district_id AS "id",
        district_name AS "Район"
    FROM Districts
    ORDER BY district_id;

CREATE OR REPLACE VIEW Show_owners AS
    SELECT 
        owner_id AS "id",
        owner_name AS "Владелец"
    FROM Owners
    ORDER BY owner_id;

CREATE OR REPLACE VIEW Show_propertyTypes AS
    SELECT 
        propertyType_id AS "id",
        propertyType_name AS "Тип собственности"
    FROM PropertyTypes
    ORDER BY propertyType_id;

CREATE OR REPLACE VIEW Show_archieve_genres AS
    SELECT 
        genre_archieve_id AS "id",
        genre_archieve_name AS "Жанр"
    FROM genres_archieve
    ORDER BY genre_archieve_id;

