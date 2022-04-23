DROP TABLE IF EXISTS Genres;

CREATE TABLE Genres(
	genre_id SERIAL PRIMARY KEY,
	genre_name TEXT NOT NULL,
	CHECK (char_length(genre_name) > 0)
);