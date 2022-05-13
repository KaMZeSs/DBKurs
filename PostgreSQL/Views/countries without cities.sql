CREATE OR REPLACE VIEW CountriesWithoutCities AS
    SELECT country_id AS "id", 
    country_name AS "Страна"
    FROM Countries 
    LEFT OUTER JOIN Cities USING (country_id) 
    WHERE cities.city_id is NULL
    ORDER BY country_name;

SELECT * FROM CountriesWithoutCities;

