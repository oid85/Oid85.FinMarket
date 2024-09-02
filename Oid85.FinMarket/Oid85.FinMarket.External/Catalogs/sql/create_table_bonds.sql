DROP TABLE bonds;

CREATE TABLE bonds (
    id          INTEGER PRIMARY KEY
                        UNIQUE
                        NOT NULL,
    ticker      TEXT,
    figi        TEXT,
    description TEXT,
    is_active   INTEGER
);

INSERT INTO bonds (ticker, figi, description, is_active)
VALUES ('Œ‘« 26225', 'BBG00K53FX22', '', 1);

INSERT INTO bonds (ticker, figi, description, is_active)
VALUES ('Œ‘« 26238', 'BBG011FJ4HS6', '', 1);

INSERT INTO bonds (ticker, figi, description, is_active)
VALUES ('Œ‘« 26207', 'BBG002PD3452', '', 1);

INSERT INTO bonds (ticker, figi, description, is_active)
VALUES ('Œ‘« 26219', 'BBG00D6Q7LY6', '', 1);

INSERT INTO bonds (ticker, figi, description, is_active)
VALUES ('Œ‘« 26243', 'BBG01H3W2J21', '', 1);

INSERT INTO bonds (ticker, figi, description, is_active)
VALUES ('Œ‘« 26226', 'BBG00N8J90K7', '', 1);