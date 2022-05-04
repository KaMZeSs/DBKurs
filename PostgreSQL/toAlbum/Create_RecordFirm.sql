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