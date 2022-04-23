DROP TABLE IF EXISTS Cities;

CREATE TABLE Cities( 
    city_id SERIAL PRIMARY KEY,
    country_id INT NOT NULL,
    city_name TEXT NOT NULL,
    FOREIGN KEY (country_id) REFERENCES Countries(country_id) ON DELETE CASCADE,
    CHECK (char_length(city_name) > 0)
);

CREATE OR REPLACE FUNCTION citi_Check() RETURNS TRIGGER AS $$
DECLARE
    
BEGIN
    
    RETURN NEW;
END
$$ LANGUAGE plpgsql;

CREATE TRIGGER citi_Check BEFORE INSERT OR UPDATE ON Cities
    FOR EACH ROW EXECUTE PROCEDURE citi_Check();
