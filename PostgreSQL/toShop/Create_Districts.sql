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
		SELECT * FROM Districts;
END; $$ LANGUAGE 'plpgsql';