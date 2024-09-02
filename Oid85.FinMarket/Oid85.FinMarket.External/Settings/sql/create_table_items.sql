DROP TABLE items;

CREATE TABLE items (
    id          INTEGER PRIMARY KEY
                        UNIQUE
                        NOT NULL,
    key          TEXT,
    value        TEXT,
    description  TEXT
);

INSERT INTO items (key, value, description)
VALUES ('Postgres:ConnectionString', 'User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=finmarket;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;', 'Строка связи к БД со свечами');

INSERT INTO items (key, value, description)
VALUES ('SQLite:Catalogs:ConnectionString', 'Data Source=c:\finmarket\catalogs.db', 'Строка связи к БД со справочниками');

INSERT INTO items (key, value, description)
VALUES ('Tinkoff:Token', 't.szzlPYKzuUTNxiVrNJPRlueboUd1eQm1MceHyb6LB-yDZ7DrHV4gN-NWkDvPvFIHAArGZHXrcDzUCJJSkqtBog', 'Токен доступа к Tinkoff API');