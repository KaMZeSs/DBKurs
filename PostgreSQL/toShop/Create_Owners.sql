DROP TABLE IF EXISTS Owners;

CREATE TABLE Owners(
	owner_id SERIAL PRIMARY KEY,
	owner_name TEXT NOT NULL,
	CHECK (char_length(owner_name) > 0)
);