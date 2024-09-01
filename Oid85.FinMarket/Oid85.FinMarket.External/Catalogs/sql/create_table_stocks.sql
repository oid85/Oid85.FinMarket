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
VALUES ('SBER', 'BBG004730N97', '��� �������� �.�.', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('SBERP', 'BBG0047315Z6', '��� �������� �.�.', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('GAZP', 'BBG004730RQ9', '��� �������', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('LKOH', 'BBG004731041', '��� ������', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('MOEX', 'BBG004730JK3', '��� ���������� �����', 1);

INSERT INTO stocks (ticker, figi, description, is_active)
VALUES ('NVTK', 'BBG00475KKZ7', '��� �������', 1);