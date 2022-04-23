DROP TABLE IF EXISTS RecordTypes;

CREATE TABLE RecordTypes(
	recordType_id SERIAL PRIMARY KEY,
	recordType_name TEXT NOT NULL,
	CHECK (char_length(recordType_name) > 0)
);