CREATE OR REPLACE VIEW albums_count_more_10kk AS
    SELECT t2.*, t1.*
    FROM (
        SELECT count(*) AS "В том числе, с тиражом более 10kk"
        FROM albums
        WHERE albumcount > 10000000
    ) t1
    CROSS JOIN (
        SELECT count(*) AS "Всего альбомов" FROM albums
    ) t2;

DROP VIEW albums_count_more_10kk