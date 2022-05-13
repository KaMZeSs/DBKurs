--Список альбомов на магазины
SELECT shops.shop_name AS "Магазин",
count(DISTINCT(album_id)) AS "Количество альбомов"
FROM productranges
JOIN shops USING (shop_id)
GROUP BY shops.shop_id;

--Среднее к-во альбомов на магазины
SELECT avg(count)
FROM (
    SELECT count(DISTINCT(album_id)) AS "count"
    FROM productranges
    GROUP BY productranges.shop_id
) AS a