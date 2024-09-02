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
VALUES ('LQDT', '', '����', 1662341);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('��� 26225', '', '���������', 160);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('��� 26238', '', '���������', 200);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('��� 26207', '', '���������', 154);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('��� 26219', '', '���������', 164);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('��� 26243', '', '���������', 140);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('��� 26226', '', '���������', 143);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('NG', '������� �� ��������� ���', '�������', 0);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('RI', '������� �� ������ ���', '�������', 0);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('GAZP', '��� �������', '�����', 800);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('BSPBP', '��� ��� �.�.', '�����', 2000);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('LKOH', '��� ������', '�����', 25);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('NVTK', '��� �������', '�����', 100);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('SNGSP', '��� �������������� �.�.', '�����', 2000);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('SBER', '��� �������� �.�.', '�����', 450);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('MOEX', '��� ���������� �����', '�����', 500);

INSERT INTO portfolio_main (ticker, description, asset_type, position)
VALUES ('RUB', '�����', 'RUB', 137175);
