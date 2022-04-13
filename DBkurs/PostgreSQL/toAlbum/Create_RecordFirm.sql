DROP TABLE IF EXISTS RecordFirms;

CREATE TABLE RecordFirms(
	recordFirm_id SERIAL PRIMARY KEY,
    city_id INT NOT NULL,
	recordFirm_name TEXT NOT NULL,
    FOREIGN KEY (city_id) REFERENCES Cities(city_id) ON DELETE CASCADE
);