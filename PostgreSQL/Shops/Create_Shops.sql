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
    FOREIGN KEY (owner_id) REFERENCES Owners(owner_id) ON DELETE CASCADE,
    CHECK (extract(YEAR FROM expiryDate) >= yearOpened)
);

CREATE OR REPLACE FUNCTION Get_All_Shops() 
RETURNS TABLE ( 
    "id" INT, 
    "Название магазина" TEXT, 
    "Район города" TEXT, 
    "Адрес" TEXT, 
    "Тип собственности" TEXT,
    "Лицензия" TEXT,
    "Дата окончания лицензии" DATE,
    "Владелец" TEXT,
    "Год открытия" INT
) AS $$
BEGIN
    RETURN QUERY
		SELECT 
        Shops.shop_id,
        Shops.shop_name, 
        Districts.district_name,
        Shops.addres,
        PropertyTypes.propertyType_name,
        Shops.license,
        Shops.expiryDate,
        Owners.owner_name,
        Shops.yearOpened
        FROM Shops
        JOIN Districts ON Districts.district_id = Shops.district_id
        JOIN Owners ON Owners.owner_id = Shops.owner_id
        JOIN PropertyTypes ON PropertyTypes.propertyType_id = Shops.propertyType_id;


END; $$ LANGUAGE 'plpgsql';