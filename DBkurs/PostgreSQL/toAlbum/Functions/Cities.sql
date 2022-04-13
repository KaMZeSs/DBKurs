DROP FUNCTION city_insert;
CREATE OR REPLACE FUNCTION city_insert(_id INT, _name TEXT, coun_id INT) RETURNS INT AS $$
BEGIN
	INSERT INTO Cities(city_id, city_name, country_id) VALUES(_id, _name, coun_id);
	IF FOUND THEN
		RETURN 1;
	ELSE RETURN 0;
	END IF;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION city_select() RETURNS TABLE ( id INT, name TEXT, country_id INT) AS $$
BEGIN
	return query
	select city_id, city_name, country_id from Cities order by city_id;
END
$$ LANGUAGE plpgsql;