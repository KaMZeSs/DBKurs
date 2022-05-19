-- Для кажд исполнителя определить к-во альбомов и доход по ним за опред время
SELECT executors.executor_id, executors.executor_name, 
sum(productranges.amount) AS "amount", 
count(distinct(albums.album_name)) AS "album_count" 
FROM executors
JOIN albums ON albums.executor_id = executors.executor_id
OR albums.albumInfo LIKE '%' || executors.executor_name || '%'
JOIN productranges ON productranges.album_id = albums.album_id 
WHERE (productranges.dateofreceipt > '01-01-1953' 
AND productranges.dateofreceipt < '01-01-1956')
GROUP BY executors.executor_id ORDER BY "amount" DESC;


--Определить к-во альбомов по каждому жанру по всем мегазинам в целом
SELECT genres.genre_id AS "id", 
genres.genre_name AS "Жанр", 
count(distinct(albums.album_id)) AS "Количество альбомов"
FROM albums
JOIN genres ON albums.genre_id = genres.genre_id
GROUP BY genres.genre_id 
ORDER BY "Количество альбомов" DESC;

--Определить к-во альбомов по каждому жанру по каждому магазину района
SELECT genres.genre_name AS "Жанр",
count(albums.album_id) AS "Количество"
FROM shops
JOIN districts ON shops.district_id = districts.district_id
JOIN productranges ON shops.shop_id = productranges.shop_id
JOIN albums ON albums.album_id = productranges.album_id
JOIN genres ON genres.genre_id = albums.genre_id
WHERE districts.district_id = 90
GROUP BY genres.genre_id
ORDER BY genres.genre_name;

--Определить к-во альбомов по каждому жанру по всем магазинам
SELECT genres.genre_name AS "Жанр",
count(distinct(albums.album_id)) AS "Количество"
FROM shops
JOIN districts ON shops.district_id = districts.district_id
JOIN productranges ON shops.shop_id = productranges.shop_id
JOIN albums ON albums.album_id = productranges.album_id
JOIN genres ON genres.genre_id = albums.genre_id
GROUP BY genres.genre_id
ORDER BY genres.genre_name;

SELECT genres.genre_name AS "Жанр",
count(albums.album_id) AS "Количество"
FROM albums
JOIN genres ON genres.genre_id = albums.genre_id
GROUP BY genres.genre_id
ORDER BY "Количество" DESC;

SELECT count(*) FROM albums;

--Список альбомов, которые продаются в данном районе
SELECT distinct(albums.album_id), albums.album_name
FROM shops
JOIN districts ON shops.district_id = districts.district_id
JOIN productranges ON shops.shop_id = productranges.shop_id
JOIN albums ON albums.album_id = productranges.album_id
WHERE districts.district_id = 1
ORDER BY album_id;

--список магазинов, которые продают данный альбом
SELECT shops.shop_id, shops.shop_name
FROM shops
JOIN productranges ON shops.shop_id = productranges.shop_id
JOIN albums ON albums.album_id = productranges.album_id
WHERE albums.genre_id = 2
ORDER BY shops.shop_id;

SELECT productranges.productrange_id
FROM productranges
WHERE productranges.shop_id = 1

SELECT * FROM Get_All_Albums_From_Shop('1')

SELECT * FROM Get_All_Albums()
JOIN productranges ON productranges.album_id = "id"
JOIN shops ON shops.shop_id = productranges.shop_id
WHERE shops.shop_id = 1 ORDER BY "Жанр";

--Топ 3 жанра по магазину
SELECT genres.genre_name AS "Жанр",
count(albums.album_id) AS "Количество"
FROM shops
JOIN districts ON shops.district_id = districts.district_id
JOIN productranges ON shops.shop_id = productranges.shop_id
JOIN albums ON albums.album_id = productranges.album_id
JOIN genres ON genres.genre_id = albums.genre_id
WHERE shops.shop_id = 1
GROUP BY genres.genre_id
ORDER BY genres.genre_name;

--Все альбомы, которые продаются у данного владельца
SELECT distinct(albums.album_id) AS "id", 
albums.album_name AS "Альбом", owners.owner_name
FROM owners
JOIN shops ON shops.owner_id = owners.owner_id
JOIN productranges ON productranges.shop_id = shops.shop_id
JOIN albums ON albums.album_id = productranges.album_id
WHERE owners.owner_id = 1;

--Все альбомы из этой страны
SELECT albums.album_id AS "id",
albums.album_name AS "Альбом"
FROM countries
JOIN cities USING (country_id)
JOIN recordfirms USING (city_id)
JOIN albums USING (recordfirm_id)
WHERE country_id = 2;

--Все альбомы, которые поставлены в определенный магазин после какой-то даты
SELECT album_id AS "id",
album_name AS "Альбом",
dateOfReceipt AS "Дата поступления"
FROM shops
JOIN productranges USING (shop_id)
JOIN albums USING (album_id)
WHERE shop_id = 1 AND dateOfReceipt > '16-10-1996'
ORDER BY dateOfReceipt DESC;

--Запрос на запросе по принципу левого соединения
--Список магазинов, в которых продается альбом, у которого только один исполнитель
SELECT distinct(albums.album_id)
FROM executors
LEFT JOIN albums USING (executor_id)
WHERE albums.album_name is NOT NULL;

SELECT distinct(shop_id), shop_name
FROM shops
JOIN productranges USING (shop_id)
WHERE productranges.album_id != ANY (
    SELECT album_id
    FROM albums
    WHERE executor_id IS NULL
) 
ORDER BY shop_id;

SELECT shop_id FROM shops WHERE shop_name = 'Silver Microsystems';

--Список городов, в которых использовался этот жанр
SELECT album_id FROM genres
LEFT JOIN albums USING (genre_id)
WHERE albums.genre_id = '1';

SELECT city_id, city_name
FROM cities
JOIN recordfirms USING (city_id)
JOIN albums USING (recordfirm_id)
WHERE 

--Список жанров в этом магазине
--Список альбомов в этом магазине
SELECT DISTINCT(genre_id), genre_name
FROM albums
LEFT JOIN genres USING (genre_id)
WHERE album_id = ANY (
    SELECT DISTINCT(album_id)
    FROM productranges WHERE shop_id = '1'
)
ORDER BY genre_id;

--Топ 3 жанра по магазину
SELECT genres.genre_name AS "Жанр",
sum(ProductRanges.amount) AS "Количество проданных копий"
FROM shops
JOIN districts ON shops.district_id = districts.district_id
JOIN productranges ON shops.shop_id = productranges.shop_id
JOIN albums ON albums.album_id = productranges.album_id
JOIN genres ON genres.genre_id = albums.genre_id
WHERE shops.shop_id = 1
GROUP BY genres.genre_id
ORDER BY "Количество проданных копий" DESC
LIMIT 3;

