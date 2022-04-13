DROP TABLE IF EXISTS Shops;

CREATE TABLE Shops(
	shop_id SERIAL PRIMARY KEY,
	district_id INT NOT NULL,
    propertyType_id INT NOT NULL,
    owner_id INT NOT NULL,
    shop_name TEXT NOT NULL,
    addres TEXT NOT NULL,
    license TEXT NOT NULL,
    expiryDate DATE NOT NULL,
    yearOpened INT NOT NULL,
    FOREIGN KEY (district_id) REFERENCES Districts(district_id) ON DELETE CASCADE,
    FOREIGN KEY (propertyType_id) REFERENCES PropertyTypes(propertyType_id) ON DELETE CASCADE,
    FOREIGN KEY (owner_id) REFERENCES Owners(owner_id) ON DELETE CASCADE
);