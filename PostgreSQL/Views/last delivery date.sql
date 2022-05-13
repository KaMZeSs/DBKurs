CREATE OR REPLACE VIEW Last_delivery AS
    SELECT shop_id AS "id",
    shop_name AS "Магазин",
    max(dateOfReceipt) AS "Дата последней поставки"
    FROM shops
    JOIN productranges USING (shop_id)
    GROUP BY shop_id
    ORDER BY "Дата последней поставки" DESC;

SELECT * FROM Last_delivery