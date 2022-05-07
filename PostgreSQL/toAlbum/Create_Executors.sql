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
		SELECT * FROM Executors ORDER BY executor_id;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION Before_insert_trigger_Executors() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

	max := 0;

	FOR curr IN
		SELECT executor_id FROM Executors
	LOOP
		IF max < curr.executor_id THEN
			max := curr.executor_id;
		END IF;

		IF curr.executor_id <> i THEN
			IF NOT EXISTS(SELECT executor_id FROM Executors WHERE executor_id = i) THEN
				NEW.executor_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.executor_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Executors
BEFORE
INSERT ON Executors
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Executors();