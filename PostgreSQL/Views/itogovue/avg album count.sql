CREATE OR REPLACE VIEW avg_album_count AS
    SELECT avg(count)::INTEGER
    FROM (
        SELECT count(DISTINCT(album_id)) AS "count"
        FROM productranges
        GROUP BY productranges.shop_id
    ) AS a;

DROP VIEW avg_album_count
SELECT * FROM avg_album_count