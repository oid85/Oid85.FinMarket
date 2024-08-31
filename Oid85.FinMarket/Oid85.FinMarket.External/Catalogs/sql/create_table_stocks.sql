CREATE TABLE stocks (
    id          INTEGER PRIMARY KEY
                        UNIQUE
                        NOT NULL,
    ticker      TEXT,
    figi        TEXT,
    description TEXT,
    is_active   INTEGER
)