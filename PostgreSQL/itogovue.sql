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

SELECT country_name
FROM cities
JOIN countries USING (country_id)
WHERE cities.city_name = 'Кев'

SELECT amname FROM pg_am