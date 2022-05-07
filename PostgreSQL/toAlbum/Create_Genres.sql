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

CREATE OR REPLACE FUNCTION Before_insert_trigger_Genres() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

	max := 0;

	FOR curr IN
		SELECT genre_id FROM Genres
	LOOP
		IF max < curr.genre_id THEN
			max := curr.genre_id;
		END IF;

		IF curr.genre_id <> i THEN
			IF NOT EXISTS(SELECT genre_id FROM Genres WHERE genre_id = i) THEN
				NEW.genre_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.genre_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Genres
BEFORE
INSERT ON Genres
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Genres();