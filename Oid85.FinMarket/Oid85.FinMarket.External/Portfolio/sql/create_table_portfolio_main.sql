DROP TABLE portfolio_main;

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
    percent_portfolio   REAL,
    cupon               REAL,
    cupon_date          TEXT
);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('LQDT', '', 'Фонд', 1662341);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('ОФЗ 26225', '', 'Облигация', 160);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('ОФЗ 26238', '', 'Облигация', 200);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('ОФЗ 26207', '', 'Облигация', 154);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('ОФЗ 26219', '', 'Облигация', 164);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('ОФЗ 26243', '', 'Облигация', 140);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('ОФЗ 26226', '', 'Облигация', 143);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('NG', 'Фьючерс на природный газ', 'Фьючерс', 0);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('RI', 'Фьючерс на индекс РТС', 'Фьючерс', 0);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('GAZP', 'ПАО Газпром', 'Акция', 800);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('BSPBP', 'ПАО БСП а.п.', 'Акция', 2000);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('LKOH', 'ПАО Лукойл', 'Акция', 25);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('NVTK', 'ПАО Новатек', 'Акция', 100);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('SNGSP', 'ПАО Сургутнефтегаз а.п.', 'Акция', 2000);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('SBER', 'ПАО Сбербанк а.о.', 'Акция', 450);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('MOEX', 'ПАО Московская биржа', 'Акция', 500);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('RUB', 'Рубль', 'RUB', 137175);
