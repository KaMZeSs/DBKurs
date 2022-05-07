DROP TABLE IF EXISTS RecordTypes;

CREATE TABLE RecordTypes(
	recordType_id SERIAL PRIMARY KEY,
	recordType_name TEXT NOT NULL,
	CHECK (char_length(recordType_name) > 0)
);

CREATE OR REPLACE FUNCTION Get_All_RecordTypes() 
RETURNS TABLE ("id" INT, "Тип записи" TEXT) AS $$
BEGIN
    RETURN QUERY
		SELECT * FROM RecordTypes ORDER BY recordType_id;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION Before_insert_trigger_RecordTypes() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

	max := 0;

	FOR curr IN
		SELECT recordType_id FROM RecordTypes
	LOOP
		IF max < curr.recordType_id THEN
			max := curr.recordType_id;
		END IF;

		IF curr.recordType_id <> i THEN
			IF NOT EXISTS(SELECT recordType_id FROM RecordTypes WHERE recordType_id = i) THEN
				NEW.recordType_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.recordType_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_RecordTypes
BEFORE
INSERT ON RecordTypes
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_RecordTypes();