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
    albumTime INT NOT NULL,
    FOREIGN KEY (recordFirm_id) REFERENCES RecordFirms(recordFirm_id) ON DELETE CASCADE,
    FOREIGN KEY (genre_id) REFERENCES Genres(genre_id) ON DELETE CASCADE,
    FOREIGN KEY (executor_id) REFERENCES Executors(executor_id) ON DELETE CASCADE,
    FOREIGN KEY (language_id) REFERENCES Languages(language_id) ON DELETE CASCADE,
    FOREIGN KEY (recordType_id) REFERENCES RecordTypes(recordType_id) ON DELETE CASCADE,
    CHECK (char_length(album_name) > 0),
    CHECK (releaseDate > to_date('1860-04-09', 'YYYY-MM-DD')),
    CHECK (albumCount > 0),
    CHECK (songsCount > 0),
    CHECK (albumTime > 0)
);

CREATE OR REPLACE FUNCTION Get_All_Albums() RETURNS 
TABLE (
    "id" INT, 
    "Название альбома" TEXT, 
    "Фирма звукозаписи" TEXT, 
    "Дата выпуска альбома" DATE, 
    "Тираж албома" INT, 
    "Количество песен" INT, 
    "Тип записи" TEXT, 
    "Альбом сборник" BOOLEAN, 
    "Исполнитель" TEXT,
    "Жанр" TEXT,
    "Язык исполнения" TEXT,
    "Информация" TEXT,
    "Титул альбома" BYTEA,
    "Время звучания, мин." INT
) AS $$
BEGIN
    RETURN QUERY
		SELECT
        Albums.album_id,
        Albums.album_name,
        RecordFirms.recordFirm_name,
        Albums.releaseDate,
        Albums.albumCount,
        Albums.songsCount,
        RecordTypes.recordType_name,
        Albums.isCollection,
        Executors.executor_name,
        Genres.genre_name,
        Languages.language_name,
        Albums.albumInfo,
        Albums.Photo,
        Albums.albumTime
        FROM Albums
        JOIN Executors ON Executors.executor_id = Albums.executor_id
        JOIN Genres ON Genres.genre_id = Albums.genre_id
        JOIN Languages ON Languages.language_id = Albums.language_id
        JOIN RecordFirms ON RecordFirms.recordFirm_id = Albums.recordFirm_id
        JOIN RecordTypes ON RecordTypes.recordType_id = Albums.recordType_id;


END; $$ LANGUAGE 'plpgsql';