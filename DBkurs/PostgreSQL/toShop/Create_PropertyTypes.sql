DROP TABLE IF EXISTS PropertyTypes;

CREATE TABLE PropertyTypes(
	propertyType_id SERIAL PRIMARY KEY,
	propertyType_name TEXT NOT NULL
);