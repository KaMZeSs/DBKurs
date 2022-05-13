DROP TABLE IF EXISTS Countries;

CREATE TABLE Countries(
	country_id SERIAL PRIMARY KEY,
	country_name TEXT NOT NULL,
	CHECK (char_length(country_name) > 0)
);

DROP FUNCTION Get_All_Countries;

CREATE OR REPLACE FUNCTION Get_All_Countries() RETURNS TABLE 
( 
	"id" INT, 
	"Страна" TEXT
) AS $$
BEGIN
    RETURN QUERY 
		SELECT * FROM COUNTRIES ORDER BY country_id;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION Before_insert_trigger_Countries() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN
	max := 0;
	i := 1;
	FOR curr IN
		SELECT country_id FROM Countries
	LOOP
		IF max < curr.country_id THEN 
			max := curr.country_id;
		END IF;
		IF NOT EXISTS(SELECT country_id FROM Countries WHERE country_id = i) THEN
			NEW.country_id = i;
			RETURN NEW;
		ELSE 
			i := i + 1;
		END IF;
	END LOOP;
	NEW.country_id = max + 1;
	RETURN NEW;
END; $$ LANGUAGE 'plpgsql';

CREATE TRIGGER Before_insert_trigger_Countries
BEFORE INSERT ON Countries
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Countries();

INSERT INTO countries (country_id, country_name) VALUES (124, 'te21321');

SELECT * FROM Countries;

SELECT * FROM cities WHERE cities.country_id = 2;

DELETE FROM countries WHERE country_id = 2;