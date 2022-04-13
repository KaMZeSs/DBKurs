DROP TABLE IF EXISTS Albums;

CREATE TABLE Albums(
	album_id SERIAL PRIMARY KEY,
	recordFirm_id INT NOT NULL,
    genre_id INT NOT NULL,
    executor_id INT,
    language_id INT,
    recordType_id INT,
    album_name TEXT NOT NULL,
    releaseDate DATE NOT NULL,
    albumCount INT NOT NULL,
    songsCount INT NOT NULL,
    isCollection BOOLEAN NOT NULL,
    albumInfo TEXT,
    Photo BYTEA NOT NULL,
    FOREIGN KEY (recordFirm_id) REFERENCES RecordFirms(recordFirm_id) ON DELETE CASCADE,
    FOREIGN KEY (genre_id) REFERENCES Genres(genre_id) ON DELETE CASCADE,
    FOREIGN KEY (executor_id) REFERENCES Executors(executor_id) ON DELETE CASCADE,
    FOREIGN KEY (language_id) REFERENCES Languages(language_id) ON DELETE CASCADE,
    FOREIGN KEY (recordType_id) REFERENCES RecordTypes(recordType_id) ON DELETE CASCADE
);