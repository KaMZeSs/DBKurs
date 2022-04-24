DROP TABLE IF EXISTS Countries;

CREATE TABLE Countries(
	country_id SERIAL PRIMARY KEY,
	country_name TEXT NOT NULL,
	CHECK (char_length(country_name) > 0)
);

DROP FUNCTION Get_All_Countries;

CREATE OR REPLACE FUNCTION Get_All_Countries() RETURNS TABLE 
( 
	"id" INT, 
	"Название" TEXT
) AS $$
BEGIN
    RETURN QUERY 
		SELECT * FROM COUNTRIES;
END; $$ LANGUAGE 'plpgsql';

