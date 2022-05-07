DROP TABLE IF EXISTS Languages;

CREATE TABLE Languages(
	language_id SERIAL PRIMARY KEY,
	language_name TEXT NOT NULL,
	CHECK (char_length(language_name) > 0)
);

CREATE OR REPLACE FUNCTION Get_All_Languages() 
RETURNS TABLE ("id" INT, "Язык" TEXT) AS $$
BEGIN
    RETURN QUERY
		SELECT * FROM Languages ORDER BY language_id;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION Before_insert_trigger_Languages() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

	max := 0;

	FOR curr IN
		SELECT language_id FROM Languages
	LOOP
		IF max < curr.language_id THEN
			max := curr.language_id;
		END IF;

		IF curr.language_id <> i THEN
			IF NOT EXISTS(SELECT language_id FROM Languages WHERE language_id = i) THEN
				NEW.language_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.language_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Languages
BEFORE INSERT ON Languages
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Languages();