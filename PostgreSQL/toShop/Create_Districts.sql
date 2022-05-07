DROP TABLE IF EXISTS Districts;

CREATE TABLE Districts(
	district_id SERIAL PRIMARY KEY,
	district_name TEXT NOT NULL,
	CHECK (char_length(district_name) > 0)
);

CREATE OR REPLACE FUNCTION Get_All_Districts() 
RETURNS TABLE ("id" INT, "Район" TEXT) AS $$
BEGIN
    RETURN QUERY
		SELECT * FROM Districts ORDER BY district_id;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION Before_insert_trigger_Districts() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

	max := 0;

	FOR curr IN
		SELECT district_id FROM Districts
	LOOP
		IF max < curr.district_id THEN
			max := curr.district_id;
		END IF;

		IF curr.district_id <> i THEN
			IF NOT EXISTS(SELECT district_id FROM Districts WHERE district_id = i) THEN
				NEW.district_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.district_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Districts
BEFORE
INSERT ON Districts
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Districts();