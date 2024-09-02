CREATE TABLE portfolio_main (
    id          INTEGER PRIMARY KEY
                        UNIQUE
                        NOT NULL,
    ticker              TEXT,
    description         TEXT,
    asset_type          TEXT,
    position            INTEGER,
    price               REAL,
    cost                REAL,
    percent_portfolio   REAL
);

CREATE TABLE aggregate_portfolio_main (
    id          INTEGER PRIMARY KEY
                        UNIQUE
                        NOT NULL,
    asset_type          TEXT,
    cost                REAL,
    percent_portfolio   REAL
);

INSERT INTO aggregate_portfolio_main (asset_type, cost, percent_portfolio)
VALUES ('Акция', 0.0, 0.0);

INSERT INTO aggregate_portfolio_main (asset_type, cost, percent_portfolio)
VALUES ('Облигация', 0.0, 0.0);

INSERT INTO aggregate_portfolio_main (asset_type, cost, percent_portfolio)
VALUES ('Фонд', 0.0, 0.0);

INSERT INTO aggregate_portfolio_main (asset_type, cost, percent_portfolio)
VALUES ('RUB', 0.0, 0.0);