DROP TABLE stocks;

CREATE TABLE stocks (
    id          INTEGER PRIMARY KEY
                        UNIQUE
                        NOT NULL,
    ticker      TEXT,
    figi        TEXT,
    description TEXT,
    is_active   INTEGER
);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('SBER', 'BBG004730N97', 'ПАО Сбербанк а.о.', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('SBERP', 'BBG0047315Z6', 'ПАО Сбербанк а.п.', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('GAZP', 'BBG004730RQ9', 'ПАО Газпром', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('LKOH', 'BBG004731041', 'ПАО Лукойл', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('MOEX', 'BBG004730JK3', 'ПАО Московская биржа', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('NVTK', 'BBG00475KKZ7', 'ПАО Новатэк', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('BSPB', 'BBG000QJW1G4', 'ПАО БСП а.о.', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('BSPBP', 'BBG00Z197566', 'ПАО БСП а.п.', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('SNGS', 'BBG0047315F8', 'ПАО Сургутнефтегаз а.о.', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('SNGSP', 'BBG004S681N1', 'ПАО Сургутнефтегаз а.п.', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('ROSN', 'BBG004731363', 'ПАО Роснефть', 1);
