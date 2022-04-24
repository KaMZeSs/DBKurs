DROP TABLE IF EXISTS Cities;

CREATE TABLE Cities( 
    city_id SERIAL PRIMARY KEY,
    country_id INT NOT NULL,
    city_name TEXT NOT NULL,
    FOREIGN KEY (country_id) REFERENCES Countries(country_id) ON DELETE CASCADE,
    CHECK (char_length(city_name) > 0)
);

CREATE OR REPLACE FUNCTION Get_All_Cities() RETURNS TABLE 
( 
	"id" INT,
    "Страна" TEXT, 
	"Город" TEXT
) AS $$
BEGIN
    RETURN QUERY 
		SELECT Cities.city_id, Countries.country_name, 
        Cities.city_name FROM Cities INNER JOIN Countries 
        ON Cities.country_id = Countries.country_id;


END; $$ LANGUAGE 'plpgsql';
