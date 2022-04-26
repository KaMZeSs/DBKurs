DROP TABLE IF EXISTS Districts;

CREATE TABLE Districts(
	district_id SERIAL PRIMARY KEY,
	district_name TEXT NOT NULL,
	CHECK (char_length(district_name) > 0)
);