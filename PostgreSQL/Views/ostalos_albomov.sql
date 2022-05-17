CREATE OR REPLACE VIEW ostalos_albomov AS
    SELECT album_id,
    album_name,
    albumCount - sum(productranges.amount) AS "Оставшееся количество альбомов"
    FROM productranges
    JOIN albums USING (album_id)
    GROUP BY productranges.album_id, albums.album_id
    ORDER BY "Оставшееся количество альбомов" DESC

SELECT * FROM ostalos_albomov