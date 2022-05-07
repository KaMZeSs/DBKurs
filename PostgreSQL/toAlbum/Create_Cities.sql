DROP TABLE IF EXISTS Cities;

CREATE TABLE Cities( 
    city_id SERIAL PRIMARY KEY,
    country_id INT NOT NULL,
    city_name TEXT NOT NULL,
    FOREIGN KEY (country_id) REFERENCES Countries(country_id) ON DELETE CASCADE,
    CHECK (char_length(city_name) > 0)
);

CREATE OR REPLACE FUNCTION Get_All_Cities() RETURNS TABLE 
( 
	"id" INT,
    "Страна" TEXT, 
	"Город" TEXT
) AS $$
BEGIN
    RETURN QUERY 
		SELECT Cities.city_id, Countries.country_name, Cities.city_name 
        FROM Cities JOIN Countries ON Cities.country_id = Countries.country_id
        ORDER BY Cities.city_id;

END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION Before_insert_trigger_Cities() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

	max := 0;

	FOR curr IN
		SELECT city_id FROM Cities
	LOOP
		IF max < curr.city_id THEN
			max := curr.city_id;
		END IF;

		IF curr.city_id <> i THEN
			IF NOT EXISTS(SELECT city_id FROM Cities WHERE city_id = i) THEN
				NEW.city_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.city_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Cities
BEFORE
INSERT ON Cities
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Cities();
