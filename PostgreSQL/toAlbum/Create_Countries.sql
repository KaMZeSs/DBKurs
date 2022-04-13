DROP TABLE IF EXISTS Countries;

CREATE TABLE Countries(
	country_id SERIAL PRIMARY KEY,
	country_name TEXT NOT NULL
);