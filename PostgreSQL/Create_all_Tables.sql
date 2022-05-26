CREATE TABLE Countries(
	country_id SERIAL PRIMARY KEY,
	country_name TEXT NOT NULL UNIQUE,
	CHECK (char_length(country_name) > 0)
);

CREATE TABLE Cities( 
    city_id SERIAL PRIMARY KEY,
    country_id INT NOT NULL,
    city_name TEXT NOT NULL,
    FOREIGN KEY (country_id) REFERENCES Countries(country_id) ON DELETE CASCADE,
    CHECK (char_length(city_name) > 0)
);

CREATE TABLE RecordFirms(
	recordFirm_id SERIAL PRIMARY KEY,
    city_id INT NOT NULL,
	recordFirm_name TEXT NOT NULL,
    FOREIGN KEY (city_id) REFERENCES Cities(city_id) ON DELETE CASCADE,
    CHECK (char_length(recordFirm_name) > 0)
);

CREATE TABLE Executors(
	executor_id SERIAL PRIMARY KEY,
	executor_name TEXT NOT NULL,
	CHECK (char_length(executor_name) > 0)
);

CREATE TABLE Genres(
	genre_id SERIAL PRIMARY KEY,
	genre_name TEXT NOT NULL UNIQUE,
	CHECK (char_length(genre_name) > 0)
);

CREATE TABLE Languages(
	language_id SERIAL PRIMARY KEY,
	language_name TEXT NOT NULL UNIQUE,
	CHECK (char_length(language_name) > 0)
);

CREATE TABLE RecordTypes(
	recordType_id SERIAL PRIMARY KEY,
	recordType_name TEXT NOT NULL UNIQUE,
	CHECK (char_length(recordType_name) > 0)
);

CREATE TABLE Districts(
	district_id SERIAL PRIMARY KEY,
	district_name TEXT NOT NULL,
	CHECK (char_length(district_name) > 0)
);

CREATE TABLE Owners(
	owner_id SERIAL PRIMARY KEY,
	owner_name TEXT NOT NULL,
	CHECK (char_length(owner_name) > 0)
);

CREATE TABLE PropertyTypes(
	propertyType_id SERIAL PRIMARY KEY,
	propertyType_name TEXT NOT NULL UNIQUE,
	CHECK (char_length(propertyType_name) > 0)
);

CREATE TABLE Shops(
	shop_id SERIAL PRIMARY KEY,
	district_id INT NOT NULL,
    propertyType_id INT NOT NULL,
    owner_id INT NOT NULL,
    shop_name TEXT NOT NULL,
    addres TEXT NOT NULL,
    license TEXT NOT NULL UNIQUE,
    expiryDate DATE NOT NULL,
    yearOpened INT NOT NULL,
    FOREIGN KEY (district_id) REFERENCES Districts(district_id) ON DELETE CASCADE,
    FOREIGN KEY (propertyType_id) REFERENCES PropertyTypes(propertyType_id) ON DELETE CASCADE,
    FOREIGN KEY (owner_id) REFERENCES Owners(owner_id) ON DELETE CASCADE,
    CHECK (extract(YEAR FROM expiryDate) >= yearOpened)
);

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
	all_executors INT[],
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

CREATE TABLE ProductRanges(
	productRange_id SERIAL PRIMARY KEY,
	shop_id INT NOT NULL,
    album_id INT NOT NULL,
    dateOfReceipt DATE NOT NULL 
    CONSTRAINT date_check CHECK (GetAlbumReleaseDate(album_id) <= dateOfReceipt),
    amount INT NOT NULL 
    CONSTRAINT amount_check CHECK (amount > 0 AND amount <= GetAlbumAmount(album_id) AND
    GetAlbumAmountInShops(album_id) <= GetAlbumAmount(album_id)),
    FOREIGN KEY (shop_id) REFERENCES Shops(shop_id) ON DELETE CASCADE,
    FOREIGN KEY (album_id) REFERENCES Albums(album_id) ON DELETE CASCADE
);

--Архив жанров-------------------------------------------------------------------------------------------
CREATE TABLE genres_archieve (
	genre_archieve_id SERIAL PRIMARY KEY,
	genre_archieve_name TEXT
);

select * from Show_archieve_genres

CREATE OR REPLACE FUNCTION Before_Delete_Genres () RETURNS TRIGGER AS $$
BEGIN

	INSERT INTO genres_archieve(genre_archieve_name)
	VALUES(OLD.genre_name);
	RETURN OLD;

END; $$ LANGUAGE 'plpgsql';

CREATE TRIGGER Before_Delete_Genres
BEFORE DELETE ON genres
FOR EACH ROW EXECUTE PROCEDURE Before_Delete_Genres();

CREATE OR REPLACE FUNCTION Before_insert_trigger_genres_archieve() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

	IF NOT EXISTS(SELECT genre_archieve_id FROM genres_archieve WHERE genre_archieve_id = 1) THEN
		NEW.genre_archieve_id = 1;
		RETURN NEW;
	END IF;

	max := 0;

	FOR curr IN
		SELECT genre_archieve_id FROM genres_archieve
	LOOP
		IF max < curr.genre_archieve_id THEN
			max := curr.genre_archieve_id;
		END IF;

		IF curr.genre_archieve_id <> i THEN
			IF NOT EXISTS(SELECT genre_archieve_id FROM genres_archieve WHERE genre_archieve_id = i) THEN
				NEW.genre_archieve_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.genre_archieve_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_genres_archieve
BEFORE INSERT ON genres_archieve
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_genres_archieve();

--Триггеры---------------------------------------------------------------------------------------
CREATE OR REPLACE FUNCTION Before_insert_trigger_Cities() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

    IF (NEW.city_id != 1) THEN
        RETURN NEW;
    END IF;

	max := 0;

	FOR curr IN
		SELECT city_id FROM Cities
	LOOP
		IF max < curr.city_id THEN
			max := curr.city_id;
		END IF;

		IF curr.city_id <> i THEN
			IF NOT EXISTS(SELECT city_id FROM Cities WHERE city_id = i) THEN
				NEW.city_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.city_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Cities
BEFORE
INSERT ON Cities
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Cities();


CREATE OR REPLACE FUNCTION Before_insert_trigger_Countries() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN
    IF (NEW.country_id != 1) THEN
        RETURN NEW;
    END IF;
	max := 0;
	i := 1;
	FOR curr IN
		SELECT country_id FROM Countries
	LOOP
		IF max < curr.country_id THEN 
			max := curr.country_id;
		END IF;
		IF NOT EXISTS(SELECT country_id FROM Countries WHERE country_id = i) THEN
			NEW.country_id = i;
			RETURN NEW;
		ELSE 
			i := i + 1;
		END IF;
	END LOOP;
	NEW.country_id = max + 1;
	RETURN NEW;
END; $$ LANGUAGE 'plpgsql';

CREATE TRIGGER Before_insert_trigger_Countries
BEFORE INSERT ON Countries
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Countries();


CREATE OR REPLACE FUNCTION Before_insert_trigger_Executors() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

    IF (NEW.executor_id != 1) THEN
        RETURN NEW;
    END IF;

	max := 0;

	FOR curr IN
		SELECT executor_id FROM Executors
	LOOP
		IF max < curr.executor_id THEN
			max := curr.executor_id;
		END IF;

		IF curr.executor_id <> i THEN
			IF NOT EXISTS(SELECT executor_id FROM Executors WHERE executor_id = i) THEN
				NEW.executor_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.executor_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Executors
BEFORE
INSERT ON Executors
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Executors();

CREATE OR REPLACE FUNCTION Before_insert_trigger_Genres() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

    IF (NEW.genre_id != 1) THEN
        RETURN NEW;
    END IF;

	max := 0;

	FOR curr IN
		SELECT genre_id FROM Genres
	LOOP
		IF max < curr.genre_id THEN
			max := curr.genre_id;
		END IF;

		IF curr.genre_id <> i THEN
			IF NOT EXISTS(SELECT genre_id FROM Genres WHERE genre_id = i) THEN
				NEW.genre_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.genre_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Genres
BEFORE
INSERT ON Genres
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Genres();

CREATE OR REPLACE FUNCTION Before_insert_trigger_Languages() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

    IF (NEW.language_id != 1) THEN
        RETURN NEW;
    END IF;

	max := 0;

	FOR curr IN
		SELECT language_id FROM Languages
	LOOP
		IF max < curr.language_id THEN
			max := curr.language_id;
		END IF;

		IF curr.language_id <> i THEN
			IF NOT EXISTS(SELECT language_id FROM Languages WHERE language_id = i) THEN
				NEW.language_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.language_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Languages
BEFORE INSERT ON Languages
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Languages();

CREATE OR REPLACE FUNCTION Before_insert_trigger_RecordFirms() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

    IF (NEW.recordFirm_id != 1) THEN
        RETURN NEW;
    END IF;

	max := 0;

	FOR curr IN
		SELECT recordFirm_id FROM RecordFirms
	LOOP
		IF max < curr.recordFirm_id THEN
			max := curr.recordFirm_id;
		END IF;

		IF curr.recordFirm_id <> i THEN
			IF NOT EXISTS(SELECT recordFirm_id FROM RecordFirms WHERE recordFirm_id = i) THEN
				NEW.recordFirm_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.recordFirm_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_RecordFirms
BEFORE
INSERT ON RecordFirms
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_RecordFirms();

CREATE OR REPLACE FUNCTION Before_insert_trigger_RecordTypes() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

    IF (NEW.recordType_id != 1) THEN
        RETURN NEW;
    END IF;

	max := 0;

	FOR curr IN
		SELECT recordType_id FROM RecordTypes
	LOOP
		IF max < curr.recordType_id THEN
			max := curr.recordType_id;
		END IF;

		IF curr.recordType_id <> i THEN
			IF NOT EXISTS(SELECT recordType_id FROM RecordTypes WHERE recordType_id = i) THEN
				NEW.recordType_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.recordType_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_RecordTypes
BEFORE
INSERT ON RecordTypes
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_RecordTypes();

CREATE OR REPLACE FUNCTION Before_insert_trigger_Districts() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

    IF (NEW.district_id != 1) THEN
        RETURN NEW;
    END IF;

	max := 0;

	FOR curr IN
		SELECT district_id FROM Districts
	LOOP
		IF max < curr.district_id THEN
			max := curr.district_id;
		END IF;

		IF curr.district_id <> i THEN
			IF NOT EXISTS(SELECT district_id FROM Districts WHERE district_id = i) THEN
				NEW.district_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.district_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Districts
BEFORE
INSERT ON Districts
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Districts();

CREATE OR REPLACE FUNCTION Before_insert_trigger_Owners() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

    IF (NEW.owner_id != 1) THEN
        RETURN NEW;
    END IF;

	max := 0;

	FOR curr IN
		SELECT owner_id FROM Owners
	LOOP
		IF max < curr.owner_id THEN
			max := curr.owner_id;
		END IF;

		IF curr.owner_id <> i THEN
			IF NOT EXISTS(SELECT owner_id FROM Owners WHERE owner_id = i) THEN
				NEW.owner_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.owner_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Owners
BEFORE
INSERT ON Owners
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Owners();

CREATE OR REPLACE FUNCTION Before_insert_trigger_PropertyTypes() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

    IF (NEW.propertyType_id != 1) THEN
        RETURN NEW;
    END IF;

	max := 0;

	FOR curr IN
		SELECT propertyType_id FROM PropertyTypes
	LOOP
		IF max < curr.propertyType_id THEN
			max := curr.propertyType_id;
		END IF;

		IF curr.propertyType_id <> i THEN
			IF NOT EXISTS(SELECT propertyType_id FROM PropertyTypes WHERE propertyType_id = i) THEN
				NEW.propertyType_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.propertyType_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_PropertyTypes
BEFORE
INSERT ON PropertyTypes
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_PropertyTypes();


CREATE OR REPLACE FUNCTION Before_insert_trigger_Shops() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

    IF (NEW.shop_id != 1) THEN
        RETURN NEW;
    END IF;

	max := 0;

	FOR curr IN
		SELECT shop_id FROM Shops
	LOOP
		IF max < curr.shop_id THEN
			max := curr.shop_id;
		END IF;

		IF curr.shop_id <> i THEN
			IF NOT EXISTS(SELECT shop_id FROM Shops WHERE shop_id = i) THEN
				NEW.shop_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.shop_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_insert_trigger_Shops
BEFORE
INSERT ON Shops
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_Shops();

CREATE OR REPLACE FUNCTION Before_insert_trigger_ProductRanges() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

    IF (NEW.productRange_id != 1) THEN
        RETURN NEW;
    END IF;

    IF (EXISTS(SELECT * FROM productranges WHERE album_id = NEW.album_id AND shop_id = NEW.shop_id)) THEN
        RAISE EXCEPTION 'В данном магазине уже продается данный альбом';
    END IF;

	max := 0;

	FOR curr IN
		SELECT productRange_id FROM ProductRanges
	LOOP
		IF max < curr.productRange_id THEN
			max := curr.productRange_id;
		END IF;

		IF curr.productRange_id <> i THEN
			IF NOT EXISTS(SELECT productRange_id FROM ProductRanges WHERE productRange_id = i) THEN
				NEW.productRange_id = i;
				RETURN NEW;
			END IF;
		ELSE
			i := i + 1;
		END IF;
	END LOOP;
	NEW.productRange_id = max + 1;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER ProductRanges
BEFORE
INSERT ON ProductRanges
FOR EACH ROW EXECUTE PROCEDURE Before_insert_trigger_ProductRanges();

CREATE OR REPLACE FUNCTION Before_update_trigger_ProductRanges() RETURNS TRIGGER AS $$
DECLARE
BEGIN

    IF (EXISTS(SELECT * FROM productranges WHERE album_id = NEW.album_id AND shop_id = NEW.shop_id)) THEN
        RAISE EXCEPTION 'В данном магазине уже продается данный альбом';
    END IF;
    RETURN NEW;

END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER Before_update_ProductRanges
BEFORE
INSERT ON ProductRanges
FOR EACH ROW EXECUTE PROCEDURE Before_update_trigger_ProductRanges();

CREATE OR REPLACE FUNCTION Before_insert_trigger_Albums() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

    IF (NEW.album_id != 1) THEN
        RETURN NEW;
    END IF;

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

CREATE OR REPLACE FUNCTION After_delete_trigger_Executors() RETURNS TRIGGER AS $$
BEGIN 
	DELETE FROM albums WHERE album_id = ANY(
		SELECT album_id FROM Albums 
		WHERE iscollection = TRUE 
		AND albumInfo LIKE '%' || OLD.executor_name || '%'
	);
	RETURN NULL;
END;
$$ LANGUAGE 'plpgsql';

CREATE TRIGGER After_delete_trigger_Executors 
AFTER DELETE ON executors 
FOR EACH ROW EXECUTE PROCEDURE After_delete_trigger_Executors();

CREATE OR REPLACE FUNCTION After_delete_executors() RETURNS TRIGGER AS $$
DECLARE    
    alb_inf TEXT;
    temp_str TEXT;
    curr RECORD;
    arr INT[];
BEGIN
    FOR curr IN (
        SELECT album_id FROM albums 
        WHERE iscollection = TRUE
        AND OLD.executor_id = ANY(all_executors::INT[])
    ) LOOP
        SELECT all_executors INTO arr FROM albums WHERE album_id = curr.album_id;
        SELECT albuminfo INTO alb_inf FROM albums WHERE album_id = curr.album_id;
        alb_inf = replace(alb_inf, OLD.executor_name || ', ', '');
        alb_inf = replace(alb_inf, ', ' || OLD.executor_name, '');
        arr = array_remove(arr, OLD.executor_id);
        UPDATE albums SET all_executors = arr, albumInfo = alb_inf WHERE album_id = curr.album_id;
    END LOOP;

    RETURN OLD;
END; $$ LANGUAGE 'plpgsql';

CREATE TRIGGER After_delete_executors
AFTER DELETE ON Executors
FOR EACH ROW EXECUTE PROCEDURE After_delete_executors();

CREATE OR REPLACE FUNCTION After_update_executors() RETURNS TRIGGER AS $$
DECLARE    
    alb_inf TEXT;
    temp_str TEXT;
    curr RECORD;
    arr INT[];
BEGIN
    FOR curr IN (
        SELECT album_id FROM albums 
        WHERE iscollection = TRUE
        AND OLD.executor_id = ANY(all_executors::INT[])
    ) LOOP
        SELECT all_executors INTO arr FROM albums WHERE album_id = curr.album_id;
        SELECT albuminfo INTO alb_inf FROM albums WHERE album_id = curr.album_id;
        alb_inf = replace(alb_inf, OLD.executor_name, NEW.executor_name);
        UPDATE albums SET albumInfo = alb_inf WHERE album_id = curr.album_id;
    END LOOP;

    RETURN OLD;
END; $$ LANGUAGE 'plpgsql';

CREATE TRIGGER After_update_executors
AFTER UPDATE ON Executors
FOR EACH ROW EXECUTE PROCEDURE After_update_executors();

CREATE OR REPLACE FUNCTION After_update_albums() RETURNS TRIGGER AS $$
DECLARE    
    alb_inf TEXT;
    temp_str TEXT;
    curr RECORD;
    arr INT[];
BEGIN

    IF array_length(NEW.all_executors, 1) = 1 THEN
        NEW.executor_id = NEW.all_executors[1];
        NEW.all_executors = NULL;
        NEW.albumInfo = NULL;
		NEW.isCollection = FALSE;
    ELSE
		NEW.isCollection = TRUE;
        RETURN NEW;
    END IF;

    RETURN NEW;
END; $$ LANGUAGE 'plpgsql';


CREATE TRIGGER After_update_albums
AFTER UPDATE ON Albums
FOR EACH ROW EXECUTE PROCEDURE After_update_albums();

CREATE OR REPLACE FUNCTION After_insert_albums() RETURNS TRIGGER AS $$
DECLARE
BEGIN
	IF NEW.albumInfo = NULL THEN
		NEW.isCollection = FALSE;
	ELSE
		NEW.isCollection = TRUE;
	END IF;
	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';

CREATE TRIGGER After_insert_albums
AFTER INSERT ON Albums
FOR EACH ROW EXECUTE PROCEDURE After_insert_albums();