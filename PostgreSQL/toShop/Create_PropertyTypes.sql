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
		SELECT * FROM PropertyTypes;
END; $$ LANGUAGE 'plpgsql';