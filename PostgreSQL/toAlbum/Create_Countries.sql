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
	"Название" TEXT
) AS $$
BEGIN
    RETURN QUERY 
		SELECT * FROM COUNTRIES;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION Before_insert_trigger_Countries() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN
	SELECT country_id INTO i FROM Countries WHERE country_id = 0;
	
	IF NOT EXISTS(SELECT country_id FROM Countries WHERE country_id = 0) THEN
		RAISE NOTICE 'i <> 0';
		NEW.country_id = 0;
		RETURN NEW;
	ELSE
		RAISE NOTICE 'i = 0';
	END IF;

	max := 0;

	FOR curr IN
		SELECT country_id FROM Countries
	LOOP
		IF max < curr.country_id THEN 
			max := curr.country_id;
		END IF;

		IF curr.country_id <> i THEN
			IF NOT EXISTS(SELECT country_id FROM Countries WHERE country_id = i) THEN
				NEW.country_id = i;
				RAISE NOTICE 'LAST (%)', i;
				RETURN NEW;
			END IF;
		ELSE 
			RAISE NOTICE 'Curr (%)', i;
			i := i + 1;
		END IF;
	END LOOP;
	RAISE NOTICE 'LAST (%)', max + 1;
	NEW.country_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';

CREATE TRIGGER Before_insert_trigger_Countries
BEFORE INSERT ON Countries
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Countries();

INSERT INTO countries (country_name) VALUES ('te21321');

SELECT * FROM Countries;

SELECT * FROM cities WHERE cities.country_id = 104;

DELETE FROM countries WHERE countries.country_id = 0;