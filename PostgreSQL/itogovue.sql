SELECT shop_name AS "Магазин",
count(distinct(album_id)) AS "Всего",
count(count) AS "в том числе"

FROM productranges
JOIN shops USING (shop_id)
GROUP BY shop_name;

SELECT genre_id FROM genres ORDER BY genre_id ASC LIMIT 1

WITH album_first_genre AS (
    SELECT album_id FROM albums
    WHERE albums.genre_id IN (
        SELECT genre_id FROM genres 
        ORDER BY genre_id ASC LIMIT 1
    )
)
SELECT count(*) FROM albums
WHERE albums.genre_id IN (
    SELECT genre_id FROM genres 
    ORDER BY genre_id ASC LIMIT 1
)
JOIN
(SELECT count(*) FROM albums) a;

--Количество альбомов по городам в выбранной стране
SELECT city_name AS "Город",
count(albums.*) AS "Количество"
FROM countries
JOIN cities USING (country_id)
JOIN recordfirms USING (city_id)
JOIN albums USING (recordfirm_id)
WHERE country_id = 1
GROUP BY city_id
ORDER BY "Количество" DESC;

--Количество фирм звукозаписи в городах, которые начинаются на...
SELECT city_name AS "Город",
count(recordfirms.*) AS "Количество"
FROM cities
JOIN recordfirms USING (city_id)
WHERE lower(city_name) LIKE 'а%'
GROUP BY city_id
ORDER BY "Количество" DESC;

--Вывести список магазинов, в которых продается больше чем ... жанра ...
SELECT shop_name AS "Магазин",
count(distinct(album_id)) AS "Количество альбомог этого жанра"
FROM shops
JOIN productranges USING (shop_id)
JOIN albums USING (album_id)
WHERE genre_id = 2
GROUP BY shop_id
HAVING count(*) > 1;

--Вывести список жанров, в которых число альбомов с названием длиннее ... более ...
SELECT genre_name AS "Жанр",
count(album_id) AS "Количество альбомов"
FROM genres
JOIN albums USING (genre_id)
WHERE char_length(album_name) > 30
GROUP BY genre_name
HAVING count(*) > 3;

--Количество альбомов по языкам, написанные в определенной стране
SELECT language_name AS "Язык",
count(album_id) AS "Количество альбомов"
FROM languages
JOIN albums USING (language_id)
WHERE album_id IN (
    SELECT album_id FROM countries
    JOIN cities USING (country_id)
    JOIN recordfirms USING (city_id)
    JOIN albums USING (recordfirm_id)
    WHERE country_id = 7
)
GROUP BY language_name
ORDER BY "Количество альбомов" DESC;

--Все магазины, которые продают альбомы с этим типом записи
SELECT DISTINCT(shop_id) AS "id",
shop_name AS "Магазин"
FROM shops
JOIN productranges USING (shop_id)
WHERE album_id IN (
    SELECT album_id FROM albums
    WHERE recordtype_id = 1
)
ORDER BY shop_id

--Список языков, которые не продаются в этом магазине
SELECT language_id AS "id",
language_name AS "Язык"
FROM languages
WHERE language_id NOT IN (
    SELECT DISTINCT(language_id)
    FROM productranges
    JOIN albums USING (album_id)
    WHERE shop_id = 1
)
ORDER BY language_id

--магазиныСКоличествомАльбомовВСравненииСЧислом
SELECT shop_id AS "id",
shop_name AS "Магазин", 
CASE 
    WHEN count(*) > 9 THEN 'больше 9'
    WHEN count(*) < 9 THEN 'меньше 9'
    ELSE 'равно 9'
END AS "Количество альбомов"
FROM shops
JOIN productranges USING (shop_id)
GROUP BY shop_id
ORDER BY shop_id;
