CREATE OR REPLACE VIEW get_all_count AS
    SELECT 'Типы записи: ' AS "Таблица", count(*) AS "Количество" FROM recordtypes
    UNION SELECT 'Языки: ', count(*) FROM languages
    UNION SELECT 'Исполнители: ', count(*) FROM executors
    UNION SELECT 'Жанры: ', count(*) FROM genres
    UNION SELECT 'Фирмы звукозаписи: ', count(*) FROM recordfirms
    UNION SELECT 'Города: ', count(*) FROM cities
    UNION SELECT 'Страны: ', count(*) FROM countries
    UNION SELECT 'Владельцы: ', count(*) FROM owners
    UNION SELECT 'Типы собственности: ', count(*) FROM propertytypes
    UNION SELECT 'Районы: ', count(*) FROM districts
    UNION SELECT 'Альбомы: ', count(*) FROM albums
    UNION SELECT 'Магазины: ', count(*) FROM shops
    UNION SELECT 'Ассортимент: ', count(*) FROM productranges
    ORDER BY "Количество" DESC

SELECT * FROM get_all_count