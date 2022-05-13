CREATE OR REPLACE VIEW Shops_PropertyTypes AS
    SELECT shop_name, propertytype_name
    FROM propertytypes
    JOIN shops USING (propertytype_id)
    ORDER BY propertytype_name;

SELECT * FROM Shops_PropertyTypes;

