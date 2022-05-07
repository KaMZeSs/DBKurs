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
        JOIN PropertyTypes ON PropertyTypes.propertyType_id = Shops.propertyType_id 
        ORDER BY Shops.shop_id;

END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION Before_insert_trigger_Shops() RETURNS TRIGGER AS $$
DECLARE
	i INT;
	max INT;
	curr RECORD;
BEGIN

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

CREATE OR REPLACE FUNCTION Before_update_trigger_Shops() RETURNS TRIGGER AS $$
DECLARE
    minDATE DATE;
BEGIN

    SELECT min(dateOfReceipt) INTO minDATE FROM productranges WHERE productranges.shop_id = NEW.shop_id;

    IF minDATE < to_date(NEW.yearOpened::text, 'YYYY') THEN
        RAISE EXCEPTION 'Один или несколько альбомов были поставлены в магазин до данного года открытия';
    END IF;

	RETURN NEW;

END; $$ LANGUAGE 'plpgsql';

CREATE TRIGGER Before_update_trigger_Shops
BEFORE
UPDATE ON Shops
FOR EACH ROW EXECUTE PROCEDURE Before_update_trigger_Shops();

SELECT min(dateOfReceipt) FROM productranges WHERE productranges.shop_id = ANY('{1, 2, 3}');