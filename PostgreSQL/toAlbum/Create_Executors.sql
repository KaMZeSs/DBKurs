DROP TABLE IF EXISTS Executors;

CREATE TABLE Executors(
	executor_id SERIAL PRIMARY KEY,
	executor_name TEXT NOT NULL,
	CHECK (char_length(executor_name) > 0)
);