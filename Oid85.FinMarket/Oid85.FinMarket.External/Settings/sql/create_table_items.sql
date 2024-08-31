CREATE TABLE items (
    id          INTEGER PRIMARY KEY
                        UNIQUE
                        NOT NULL,
    key          TEXT,
    value        TEXT,
    description  TEXT
)