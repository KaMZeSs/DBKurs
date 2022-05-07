DROP TABLE IF EXISTS PropertyTypes;

CREATE TABLE PropertyTypes(
	propertyType_id SERIAL PRIMARY KEY,
	propertyType_name TEXT NOT NULL,
	CHECK (char_length(propertyType_name) > 0)
);

CREATE OR REPLACE FUNCTION Get_All_PropertyTypes() 
RETURNS TABLE ("id" INT, "Тип собственности" TEXT) AS $$
BEGIN
    RETURN QUERY
		SELECT * FROM PropertyTypes ORDER BY propertyType_id;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION Before_insert_trigger_PropertyTypes() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

	max := 0;

	FOR curr IN
		SELECT propertyType_id FROM PropertyTypes
	LOOP
		IF max < curr.propertyType_id THEN
			max := curr.propertyType_id;
		END IF;

		IF curr.propertyType_id <> i THEN
			IF NOT EXISTS(SELECT propertyType_id FROM PropertyTypes WHERE propertyType_id = i) THEN
				NEW.propertyType_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.propertyType_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_PropertyTypes
BEFORE
INSERT ON PropertyTypes
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_PropertyTypes();