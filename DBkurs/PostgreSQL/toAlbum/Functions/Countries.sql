CREATE OR REPLACE FUNCTION country_insert(_id INT, _name TEXT)
RETURNS INT AS
$$
BEGIN
	INSERT INTO Countries(country_id, country_name) VALUES(_id, _name);
	IF FOUND THEN
		RETURN 1;
	ELSE RETURN 0;
	END IF;
END
$$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION country_select()
RETURNS TABLE (
	id INT,
	name TEXT
) AS
$$
BEGIN
	return query
	select country_id, country_name from Countries order by country_id;
END
$$
LANGUAGE plpgsql;