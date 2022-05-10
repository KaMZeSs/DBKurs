DROP TABLE IF EXISTS Albums;
DELETE FROM albums;

CREATE TABLE Albums(
	album_id SERIAL PRIMARY KEY,
	recordFirm_id INT NOT NULL,
    genre_id INT NOT NULL,
    executor_id INT,
    language_id INT NOT NULL,
    recordType_id INT NOT NULL,
    album_name TEXT NOT NULL,
    releaseDate DATE NOT NULL,
    albumCount INT NOT NULL,
    songsCount INT NOT NULL,
    isCollection BOOLEAN NOT NULL,
    albumInfo TEXT,
    Photo BYTEA,
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
    "Время звучания" INT
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
        FULL JOIN Executors ON Executors.executor_id = Albums.executor_id
        FULL JOIN Genres ON Genres.genre_id = Albums.genre_id
        FULL JOIN Languages ON Languages.language_id = Albums.language_id
        JOIN RecordFirms ON RecordFirms.recordFirm_id = Albums.recordFirm_id
        JOIN RecordTypes ON RecordTypes.recordType_id = Albums.recordType_id
        ORDER BY Albums.album_id;


END; $$ LANGUAGE 'plpgsql';

DROP FUNCTION Get_All_Albums_From_Shop;

CREATE OR REPLACE FUNCTION Get_All_Albums_From_Shop(shop_toFind_id INT) 
RETURNS TABLE ( 
    "id" INT, 
    "Название альбома" TEXT, 
    "Фирма звукозаписи" TEXT, 
    "Дата поступления" DATE,
    "Дата выпуска альбома" DATE,
    "Ассортимент" INT, 
    "Тираж албома" INT, 
    "Количество песен" INT, 
    "Тип записи" TEXT, 
    "Альбом сборник" BOOLEAN, 
    "Исполнитель" TEXT, 
    "Жанр" TEXT, 
    "Язык исполнения" TEXT, 
    "Информация" TEXT, 
    "Титул альбома" BYTEA, 
    "Время звучания" INT
) AS $$
BEGIN
    RETURN QUERY
		SELECT
        Albums.album_id,
        Albums.album_name,
        RecordFirms.recordFirm_name,
        TempProductRanges.dateOfReceipt,
        Albums.releaseDate,
        TempProductRanges.amount,
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
        LEFT JOIN Executors ON Executors.executor_id = Albums.executor_id
        JOIN Genres ON Genres.genre_id = Albums.genre_id
        JOIN Languages ON Languages.language_id = Albums.language_id
        JOIN RecordFirms ON RecordFirms.recordFirm_id = Albums.recordFirm_id
        JOIN RecordTypes ON RecordTypes.recordType_id = Albums.recordType_id
        JOIN (SELECT * FROM ProductRanges WHERE ProductRanges.shop_id = shop_toFind_id) 
        AS TempProductRanges ON TempProductRanges.album_id = Albums.album_id
        ORDER BY Albums.album_id;
END; $$ LANGUAGE 'plpgsql';


CREATE OR REPLACE FUNCTION Before_insert_trigger_Albums() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

	max := 0;

	FOR curr IN
		SELECT album_id FROM Albums
	LOOP
		IF max < curr.album_id THEN
			max := curr.album_id;
		END IF;

		IF curr.album_id <> i THEN
			IF NOT EXISTS(SELECT album_id FROM Albums WHERE album_id = i) THEN
				NEW.album_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.album_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Albums
BEFORE
INSERT ON Albums
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Albums();