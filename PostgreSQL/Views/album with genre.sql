CREATE OR REPLACE VIEW Albums_Genres AS
    SELECT album_name, genre_name
    FROM genres
    JOIN albums USING (genre_id)
    ORDER BY genre_name;

SELECT * FROM Albums_Genres;

