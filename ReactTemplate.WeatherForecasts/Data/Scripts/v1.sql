CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20250728130539_Initial') THEN
        IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'weather_forecasts') THEN
            CREATE SCHEMA weather_forecasts;
        END IF;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20250728130539_Initial') THEN
    CREATE TABLE weather_forecasts.weather_forecasts (
        id uuid NOT NULL,
        date date NOT NULL,
        temperature_c integer NOT NULL,
        summary text,
        CONSTRAINT pk_weather_forecasts PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20250728130539_Initial') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20250728130539_Initial', '9.0.7');
    END IF;
END $EF$;
COMMIT;

