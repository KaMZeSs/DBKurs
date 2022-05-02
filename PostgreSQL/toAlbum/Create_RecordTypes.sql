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
		SELECT * FROM RecordTypes;
END; $$ LANGUAGE 'plpgsql';