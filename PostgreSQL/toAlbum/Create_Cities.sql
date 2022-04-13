DROP TABLE IF EXISTS Cities;

CREATE TABLE Cities( 
    city_id SERIAL PRIMARY KEY,
    country_id INT NOT NULL,
    city_name TEXT NOT NULL,
    FOREIGN KEY (country_id) REFERENCES Countries(country_id) ON DELETE CASCADE
);