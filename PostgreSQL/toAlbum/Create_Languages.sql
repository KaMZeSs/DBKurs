DROP TABLE IF EXISTS Languages;

CREATE TABLE Languages(
	language_id SERIAL PRIMARY KEY,
	language_name TEXT NOT NULL,
	CHECK (char_length(language_name) > 0)
);