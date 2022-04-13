DROP TABLE IF EXISTS ProductRanges;

CREATE TABLE ProductRanges(
	productRange_id SERIAL PRIMARY KEY,
	shop_id INT NOT NULL,
    album_id INT NOT NULL,
    dateOfReceipt DATE NOT NULL,
    amount INT NOT NULL,
    FOREIGN KEY (shop_id) REFERENCES Shops(shop_id) ON DELETE CASCADE,
    FOREIGN KEY (album_id) REFERENCES Albums(album_id) ON DELETE CASCADE
);