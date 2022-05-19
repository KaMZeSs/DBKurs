CREATE OR REPLACE FUNCTION CountCascadeAlbums(ids INT[]) RETURNS INT[] AS $$
DECLARE
    result INT[2];
    temp INT;
BEGIN
    result[0] := array_length(ids, 1);
    SELECT count(productRange_id) INTO temp FROM ProductRanges 
    WHERE productranges.album_id = ANY(ids::INT[]);
    result[1] := temp;
    RETURN result;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION CountCascadeShops(ids INT[]) RETURNS INT AS $$
DECLARE
    result INT[2];
    temp INT;
BEGIN
    result[0] := array_length(ids, 1);
    SELECT count(productRange_id) INTO temp FROM ProductRanges 
    WHERE productranges.shop_id = ANY(ids::INT[]);
    result[1] := temp;
    RETURN result;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION CountCascadeDistricts(ids INT[]) RETURNS INT[] AS $$
DECLARE
    result INT[3];
    temp INT;
BEGIN
    result[0] := array_length(ids, 1);

    SELECT count(*) INTO temp FROM shops
    WHERE district_id = ANY(ids::INT[]);
    result[1] := temp;

    SELECT count(*) INTO temp FROM productranges 
    WHERE shop_id = ANY(SELECT shop_id FROM shops 
    WHERE district_id = ANY(ids::INT[]));
    result[2] := temp;

    RETURN result;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION CountCascadePropertyTypes(ids INT[]) RETURNS INT[] AS $$
DECLARE
    result INT[3];
    temp INT;
BEGIN
    result[0] := array_length(ids, 1);

    SELECT count(*) INTO temp FROM shops
    WHERE propertytype_id = ANY(ids::INT[]);
    result[1] := temp;

    SELECT count(*) INTO temp FROM productranges 
    WHERE shop_id = ANY(SELECT shop_id FROM shops 
    WHERE propertytype_id = ANY(ids::INT[]));
    result[2] := temp;

    RETURN result;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION CountCascadeOwners(ids INT[]) RETURNS INT[] AS $$
DECLARE
    result INT[3];
    temp INT;
BEGIN
    result[0] := array_length(ids, 1);

    SELECT count(*) INTO temp FROM shops
    WHERE owner_id = ANY(ids::INT[]);
    result[1] := temp;

    SELECT count(*) INTO temp FROM productranges 
    WHERE shop_id = ANY(SELECT shop_id FROM shops 
    WHERE owner_id = ANY(ids::INT[]));
    result[2] := temp;

    RETURN result;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION CountCascadeRecordTypes(ids INT[]) RETURNS INT[] AS $$
DECLARE
    result INT[3];
    temp INT;
BEGIN
    result[0] := array_length(ids, 1);

    SELECT count(*) INTO temp FROM albums
    WHERE recordtype_id = ANY(ids::INT[]);
    result[1] := temp;

    SELECT count(*) INTO temp FROM productranges 
    WHERE album_id = ANY(SELECT album_id FROM albums 
    WHERE recordtype_id = ANY(ids::INT[]));
    result[2] := temp;

    RETURN result;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION CountCascadeLanguages(ids INT[]) RETURNS INT[] AS $$
DECLARE
    result INT[3];
    temp INT;
BEGIN
    result[0] := array_length(ids, 1);

    SELECT count(*) INTO temp FROM albums
    WHERE language_id = ANY(ids::INT[]);
    result[1] := temp;

    SELECT count(*) INTO temp FROM productranges 
    WHERE album_id = ANY(SELECT album_id FROM albums 
    WHERE language_id = ANY(ids::INT[]));
    result[2] := temp;

    RETURN result;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION CountCascadeExecutors(ids INT[]) RETURNS INT[] AS $$
DECLARE
    result INT[3];
    temp INT;
    executor_names TEXT[] := ARRAY(
        SELECT '%' || executor_name || '%'
        FROM executors
        Where executor_id = ANY(ids::INT [])
    );
BEGIN
    result[0] := array_length(ids, 1);

    SELECT count(*) INTO temp FROM albums
    WHERE executor_id = ANY(ids::INT[]) 
    OR albumInfo LIKE ANY(executor_names::TEXT[]);
    result[1] := temp;

    SELECT count(*) INTO temp FROM productranges 
    WHERE album_id = ANY(SELECT album_id FROM albums 
    WHERE executor_id = ANY(ids::INT[]) 
    OR albumInfo LIKE ANY(executor_names::TEXT[]));
    result[2] := temp;

    RETURN result;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION CountCascadeGenres(ids INT[]) RETURNS INT[] AS $$
DECLARE
    result INT[3];
    temp INT;
BEGIN
    result[0] := array_length(ids, 1);

    SELECT count(*) INTO temp FROM albums
    WHERE genre_id = ANY(ids::INT[]);
    result[1] := temp;

    SELECT count(*) INTO temp FROM productranges 
    WHERE album_id = ANY(SELECT album_id FROM albums 
    WHERE genre_id = ANY(ids::INT[]));
    result[2] := temp;

    RETURN result;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION CountCascadeRecordFirm(ids INT[]) RETURNS INT[] AS $$
DECLARE
    result INT[3];
    temp INT;
BEGIN
    result[0] := array_length(ids, 1);

    SELECT count(*) INTO temp FROM albums
    WHERE recordfirm_id = ANY(ids::INT[]);
    result[1] := temp;

    SELECT count(*) INTO temp FROM productranges 
    WHERE album_id = ANY(SELECT album_id FROM albums 
    WHERE recordfirm_id = ANY(ids::INT[]));
    result[2] := temp;

    RETURN result;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION CountCascadeCities(ids INT[]) RETURNS INT[] AS $$
DECLARE
    result INT[4];
    temp INT;
BEGIN
    result[0] := array_length(ids, 1);

    SELECT count(*) INTO temp FROM recordfirms
    WHERE city_id = ANY(ids::INT[]);
    result[1] := temp;

    SELECT count(*) INTO temp FROM albums
    WHERE recordfirm_id = ANY(SELECT recordfirm_id FROM recordfirms
    WHERE city_id = ANY(ids::INT[]));
    result[2] := temp;

    SELECT count(*) INTO temp FROM productranges 
    WHERE album_id = ANY(SELECT album_id FROM albums 
    WHERE recordfirm_id = ANY(SELECT recordfirm_id FROM recordfirms
    WHERE city_id = ANY(ids::INT[])));
    result[3] := temp;

    RETURN result;
END; $$ LANGUAGE 'plpgsql';

CREATE OR REPLACE FUNCTION CountCascadeCountries(ids INT[]) RETURNS INT[] AS $$
DECLARE
    result INT[5];
    temp INT;
BEGIN
    result[0] := array_length(ids, 1);

    SELECT count(*) INTO temp FROM cities
    WHERE country_id = ANY(ids::INT[]);
    result[1] := temp;

    SELECT count(*) INTO temp FROM recordfirms
    WHERE city_id = ANY(SELECT city_id FROM cities
    WHERE country_id = ANY(ids::INT[]));
    result[2] := temp;

    SELECT count(*) INTO temp FROM albums 
    WHERE recordfirm_id = ANY(SELECT recordfirm_id FROM recordfirms 
    WHERE city_id = ANY(SELECT city_id FROM cities
    WHERE country_id = ANY(ids::INT[])));
    result[3] := temp;
    
    SELECT count(*) INTO temp FROM productranges 
    WHERE album_id = ANY(SELECT album_id FROM albums 
    WHERE recordfirm_id = ANY(SELECT recordfirm_id FROM recordfirms
    WHERE city_id = ANY(SELECT city_id FROM cities
    WHERE country_id = ANY(ids::INT[]))));
    result[4] := temp;

    RETURN result;
END; $$ LANGUAGE 'plpgsql';