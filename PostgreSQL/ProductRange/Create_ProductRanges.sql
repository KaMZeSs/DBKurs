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
        JOIN Albums ON ProductRanges.album_id = Albums.album_id;


END; $$ LANGUAGE 'plpgsql';

