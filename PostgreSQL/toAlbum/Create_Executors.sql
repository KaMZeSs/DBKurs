DROP TABLE IF EXISTS Executors;

CREATE TABLE Executors(
	executor_id SERIAL PRIMARY KEY,
	executor_name TEXT NOT NULL,
	CHECK (char_length(executor_name) > 0)
);

CREATE OR REPLACE FUNCTION Get_All_Executors() 
RETURNS TABLE ("id" INT, "Исполнитель" TEXT) AS $$
BEGIN
    RETURN QUERY
		SELECT * FROM Executors;
END; $$ LANGUAGE 'plpgsql';