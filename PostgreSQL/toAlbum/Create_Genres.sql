DROP TABLE IF EXISTS Genres;

CREATE TABLE Genres(
	genre_id SERIAL PRIMARY KEY,
	genre_name TEXT NOT NULL,
	CHECK (char_length(genre_name) > 0)
);

CREATE OR REPLACE FUNCTION Get_All_Genres() 
RETURNS TABLE ("id" INT, "Жанр" TEXT) AS $$
BEGIN
    RETURN QUERY
		SELECT * FROM Genres ORDER BY genre_id;
END; $$ LANGUAGE 'plpgsql';