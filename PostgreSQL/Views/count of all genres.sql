CREATE OR REPLACE VIEW Count_Genres AS
    SELECT genres.genre_name AS "Жанр",
    count(distinct(albums.album_id)) AS "Количество"
    FROM productranges
    JOIN albums ON productranges.album_id = albums.album_id
    JOIN genres ON genres.genre_id = albums.genre_id
    GROUP BY genres.genre_id
    ORDER BY "Количество" DESC;

SELECT * FROM count_genres;

