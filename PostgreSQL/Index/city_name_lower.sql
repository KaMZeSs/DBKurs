CREATE INDEX city_name_lower ON Cities (lower(city_name))

SELECT * FROM city_name_lower