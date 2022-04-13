DROP TABLE IF EXISTS RecordTypes;

CREATE TABLE RecordTypes(
	recordType_id SERIAL PRIMARY KEY,
	recordType_name TEXT NOT NULL
);