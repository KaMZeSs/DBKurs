-- Если количество альбомов принимать за топовость
CREATE OR REPLACE VIEW Count_Top3_Genres AS
    SELECT genres.genre_name AS "Жанр",
    count(distinct(albums.album_id)) AS "Количество проданных копий"
    FROM productranges
    JOIN albums ON productranges.album_id = albums.album_id
    JOIN genres ON genres.genre_id = albums.genre_id
    GROUP BY genres.genre_id
    ORDER BY "Количество" DESC;


-- Если количество проданных альбомов из всего тиража принимать за топовость
CREATE OR REPLACE VIEW Count_Top3_Genres AS
    SELECT genres.genre_name AS "Жанр",
    sum(ProductRanges.amount) AS "Количество проданных копий"
    FROM productranges
    JOIN albums ON productranges.album_id = albums.album_id
    JOIN genres ON genres.genre_id = albums.genre_id
    GROUP BY genres.genre_id
    ORDER BY "Количество проданных копий" DESC;

SELECT * FROM Count_Top3_Genres;

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