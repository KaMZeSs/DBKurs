SELECT * FROM ProductRanges;

SELECT * FROM albums;
SELECT * FROM shops;

SELECT * FROM countries;
SELECT * FROM cities;
SELECT * FROM executors;
SELECT * FROM genres;
SELECT * FROM languages;
SELECT * FROM recordfirms;
SELECT * FROM recordtypes;

SELECT * FROM districts;
SELECT * FROM owners WHERE owners.owner_id = 1000;
SELECT * FROM propertytypes;

INSERT INTO Countries (country_id, country_name) VALUES (0, 'Великобритания');
DELETE FROM Countries WHERE country_id = 0;

INSERT INTO Shops (shop_id, district_id, propertyType_id, owner_id, shop_name, addres, license, expiryDate, yearOpened) VALUES (0, 0, 0, 0, 'Rhinowheels', 'Площадь Борко, 135', 'AE745UNBTY', '08-04-2020', 1929);

INSERT INTO ProductRanges (productRange_id, shop_id, album_id, dateOfReceipt, amount) VALUES (4600, 254, 942, '17-10-2003', 0);

Select GetAlbumReleaseDate(254);
SELECT * FROM albums WHERE album_id = 0;

SELECT cities.city_name, country_name FROM cities 
JOIN countries ON countries.country_id = cities.country_id 
ORDER BY countries.country_name;

SELECT Owners.owner_name FROM owners WHERE Owners.owner_name LIKE '%Туп%';

SELECT "district_name" as "Район", 
"owner_name" as "Владелец" FROM districts
JOIN shops ON shops.district_id = districts.district_id
JOIN owners ON owners.owner_id = shops.owner_id
ORDER BY owner_name; 

SELECT * FROM Get_All_Countries();

SELECT country_name FROM Countries;

SELECT album_id FROM ProductRanges WHERE ProductRanges.shop_id = 5;

SELECT shops.addres FROM shops WHERE shops.owner_id = 1;

SELECT shop_id, shop_name FROM shops;

SELECT album_id, album_name FROM albums;