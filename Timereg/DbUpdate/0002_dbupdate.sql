CREATE TABLE IF NOT EXISTS public.test
(
    id serial PRIMARY KEY,
    name character varying(500) COLLATE pg_catalog."default",
    email character varying(200) COLLATE pg_catalog."default",
    salary numeric(10,2)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.test
    OWNER to app_user;