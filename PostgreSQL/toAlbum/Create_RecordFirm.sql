DROP TABLE IF EXISTS RecordFirms;

CREATE TABLE RecordFirms(
	recordFirm_id SERIAL PRIMARY KEY,
    city_id INT NOT NULL,
	recordFirm_name TEXT NOT NULL,
    FOREIGN KEY (city_id) REFERENCES Cities(city_id) ON DELETE CASCADE,
    CHECK (char_length(recordFirm_name) > 0)
);

CREATE OR REPLACE FUNCTION Get_All_RecordFirms() 
RETURNS TABLE (
    "id" INT, 
    "Город" TEXT,
    "Фирма звукозаписи" TEXT
) AS $$
BEGIN
    RETURN QUERY
		SELECT RecordFirms.recordFirm_id, 
        Cities.city_name, RecordFirms.recordFirm_name
        FROM RecordFirms JOIN Cities ON Cities.city_id = RecordFirms.city_id
        ORDER BY RecordFirms.recordFirm_id;

END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION Before_insert_trigger_RecordFirms() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

	max := 0;

	FOR curr IN
		SELECT recordFirm_id FROM RecordFirms
	LOOP
		IF max < curr.recordFirm_id THEN
			max := curr.recordFirm_id;
		END IF;

		IF curr.recordFirm_id <> i THEN
			IF NOT EXISTS(SELECT recordFirm_id FROM RecordFirms WHERE recordFirm_id = i) THEN
				NEW.recordFirm_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.recordFirm_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_RecordFirms
BEFORE
INSERT ON RecordFirms
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_RecordFirms();