CREATE OR REPLACE VIEW Albums_one_executor AS
    SELECT album_id AS "id",
    album_name AS "Альбом",
    executor_name AS "Исполнитель"
    FROM executors
    RIGHT OUTER JOIN albums USING (executor_id)
    WHERE executor_id IS NOT NULL
    ORDER BY executor_name;

SELECT * FROM Albums_one_executor;

