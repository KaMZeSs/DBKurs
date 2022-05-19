DROP TABLE IF EXISTS ProductRanges;

CREATE TABLE ProductRanges(
	productRange_id SERIAL PRIMARY KEY,
	shop_id INT NOT NULL,
    album_id INT NOT NULL,
    dateOfReceipt DATE NOT NULL,
    amount INT NOT NULL,
    FOREIGN KEY (shop_id) REFERENCES Shops(shop_id) ON DELETE CASCADE,
    FOREIGN KEY (album_id) REFERENCES Albums(album_id) ON DELETE CASCADE,
    CHECK (GetAlbumReleaseDate(album_id) <= dateOfReceipt),
    CHECK (amount > 0 AND amount <= GetAlbumAmount(album_id)),
    CHECK (GetAlbumAmountInShops(album_id) <= GetAlbumAmount(album_id))
);

CREATE OR REPLACE FUNCTION GetAlbumReleaseDate(_id INT) RETURNS DATE AS $$
DECLARE
    d DATE;
BEGIN
    SELECT releaseDate INTO d FROM Albums WHERE Albums.album_id = _id;
    RETURN d;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetAlbumAmount(_id INT) RETURNS INT AS $$
DECLARE
    d INT;
BEGIN
    SELECT albumCount INTO d FROM Albums WHERE Albums.album_id = _id;
    RETURN d;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetAlbumAmountInShops(_id INT) RETURNS INT AS $$
DECLARE
    d INT;
BEGIN
    SELECT SUM(amount) INTO d FROM ProductRanges WHERE ProductRanges.album_id = _id;
    RETURN d;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION Get_All_ProductRanges() 
RETURNS TABLE (
    "id" INT, 
    "Магазин" TEXT, 
    "Альбом" TEXT,
    "Дата поступления" DATE,
    "Количество единиц" INT
) AS $$
BEGIN
    RETURN QUERY
		SELECT ProductRanges.productRange_id,
        Shops.shop_name, albums.album_name, 
        ProductRanges.dateOfReceipt, 
        ProductRanges.amount
        FROM ProductRanges 
        JOIN Shops ON ProductRanges.shop_id = Shops.shop_id
        JOIN Albums ON ProductRanges.album_id = Albums.album_id
        ORDER BY ProductRanges.productRange_id;

END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION Before_insert_trigger_ProductRanges() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

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