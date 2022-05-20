-- Если количество проданных альбомов из всего тиража принимать за топовость
CREATE OR REPLACE VIEW Count_Top3_Genres AS
    SELECT genres.genre_name AS "Жанр",
    sum(ProductRanges.amount) AS "Количество проданных копий"
    FROM productranges
    JOIN albums ON productranges.album_id = albums.album_id
    JOIN genres ON genres.genre_id = albums.genre_id
    GROUP BY genres.genre_id
    ORDER BY "Количество проданных копий" DESC;

CREATE OR REPLACE VIEW Shops_PropertyTypes AS
    SELECT shop_name, propertytype_name
    FROM propertytypes
    JOIN shops USING (propertytype_id)
    ORDER BY propertytype_name;

CREATE OR REPLACE VIEW Recordfirm_Country AS
    SELECT country_name AS "Страна",
    city_name AS "Город",
    recordfirm_name AS "Фирма звукозаписи"
    FROM countries
    JOIN cities USING (country_id)
    JOIN recordfirms USING (city_id)
    ORDER BY country_name;

CREATE OR REPLACE VIEW ostalos_albomov AS
    SELECT album_id,
    album_name,
    albumCount - sum(productranges.amount) AS "Оставшееся количество альбомов"
    FROM productranges
    JOIN albums USING (album_id)
    GROUP BY productranges.album_id, albums.album_id
    ORDER BY "Оставшееся количество альбомов" DESC;

CREATE OR REPLACE VIEW Last_delivery AS
    SELECT shop_id AS "id",
    shop_name AS "Магазин",
    max(dateOfReceipt) AS "Дата последней поставки"
    FROM shops
    JOIN productranges USING (shop_id)
    GROUP BY shop_id
    ORDER BY "Дата последней поставки" DESC;

CREATE OR REPLACE VIEW CountriesWithoutCities AS
    SELECT country_id AS "id", 
    country_name AS "Страна"
    FROM Countries 
    LEFT OUTER JOIN Cities USING (country_id) 
    WHERE cities.city_id is NULL
    ORDER BY country_name;

CREATE OR REPLACE VIEW Count_Genres AS
    SELECT genres.genre_name AS "Жанр",
    count(distinct(albums.album_id)) AS "Количество"
    FROM productranges
    JOIN albums ON productranges.album_id = albums.album_id
    JOIN genres ON genres.genre_id = albums.genre_id
    GROUP BY genres.genre_id
    ORDER BY "Количество" DESC;

CREATE OR REPLACE VIEW Albums_Genres AS
    SELECT album_name, genre_name
    FROM genres
    JOIN albums USING (genre_id)
    ORDER BY genre_name;

CREATE OR REPLACE VIEW Albums_one_executor AS
    SELECT album_id AS "id",
    album_name AS "Альбом",
    executor_name AS "Исполнитель"
    FROM executors
    RIGHT OUTER JOIN albums USING (executor_id)
    WHERE executor_id IS NOT NULL
    ORDER BY executor_name;

CREATE OR REPLACE VIEW albums_count_more_10kk AS
    SELECT t2.*, t1.*
    FROM (
        SELECT count(*) AS "В том числе, с тиражом более 10kk"
        FROM albums
        WHERE albumcount > 10000000
    ) t1
    CROSS JOIN (
        SELECT count(*) AS "Всего альбомов" FROM albums
    ) t2;

CREATE OR REPLACE VIEW avg_album_count AS
    SELECT avg(count)::INTEGER
    FROM (
        SELECT count(DISTINCT(album_id)) AS "count"
        FROM productranges
        GROUP BY productranges.shop_id
    ) AS a;

SELECT * FROM avg_album_count

CREATE OR REPLACE VIEW get_all_count AS
    SELECT 'Типы записи' AS "Таблица", count(*) AS "Количество" FROM recordtypes
    UNION SELECT 'Языки', count(*) FROM languages
    UNION SELECT 'Исполнители', count(*) FROM executors
    UNION SELECT 'Жанры', count(*) FROM genres
    UNION SELECT 'Фирмы звукозаписи', count(*) FROM recordfirms
    UNION SELECT 'Города', count(*) FROM cities
    UNION SELECT 'Страны', count(*) FROM countries
    UNION SELECT 'Владельцы', count(*) FROM owners
    UNION SELECT 'Типы собственности', count(*) FROM propertytypes
    UNION SELECT 'Районы', count(*) FROM districts
    UNION SELECT 'Альбомы', count(*) FROM albums
    UNION SELECT 'Магазины', count(*) FROM shops
    UNION SELECT 'Ассортимент', count(*) FROM productranges
    ORDER BY "Количество" DESC;

CREATE OR REPLACE VIEW shop_album_count AS
    SELECT shops.shop_name AS "Магазин",
    count(DISTINCT(album_id)) AS "Количество альбомов"
    FROM productranges
    JOIN shops USING (shop_id)
    GROUP BY shops.shop_id
    ORDER BY "Количество альбомов" DESC;