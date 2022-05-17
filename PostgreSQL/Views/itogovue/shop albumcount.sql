CREATE OR REPLACE VIEW shop_album_count AS
    SELECT shops.shop_name AS "Магазин",
    count(DISTINCT(album_id)) AS "Количество альбомов"
    FROM productranges
    JOIN shops USING (shop_id)
    GROUP BY shops.shop_id
    ORDER BY "Количество альбомов" DESC;

SELECT * FROM shop_album_count