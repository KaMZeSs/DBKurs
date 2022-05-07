DROP TABLE IF EXISTS Owners;

CREATE TABLE Owners(
	owner_id SERIAL PRIMARY KEY,
	owner_name TEXT NOT NULL,
	CHECK (char_length(owner_name) > 0)
);

CREATE OR REPLACE FUNCTION Get_All_Owners() 
RETURNS TABLE ("id" INT, "Владелец" TEXT) AS $$
BEGIN
    RETURN QUERY
		SELECT * FROM Owners ORDER BY owner_id;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION Before_insert_trigger_Owners() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

	max := 0;

	FOR curr IN
		SELECT owner_id FROM Owners
	LOOP
		IF max < curr.owner_id THEN
			max := curr.owner_id;
		END IF;

		IF curr.owner_id <> i THEN
			IF NOT EXISTS(SELECT owner_id FROM Owners WHERE owner_id = i) THEN
				NEW.owner_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.owner_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Owners
BEFORE
INSERT ON Owners
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Owners();