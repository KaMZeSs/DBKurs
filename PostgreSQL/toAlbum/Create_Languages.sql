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