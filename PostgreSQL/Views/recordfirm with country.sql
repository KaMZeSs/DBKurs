CREATE OR REPLACE VIEW Recordfirm_Country AS
    SELECT country_name AS "Страна",
    city_name AS "Город",
    recordfirm_name AS "Фирма звукозаписи"
    FROM countries
    JOIN cities USING (country_id)
    JOIN recordfirms USING (city_id)
    ORDER BY country_name;

SELECT * FROM Recordfirm_Country;

