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