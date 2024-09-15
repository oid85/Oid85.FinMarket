---------------------------------------------------------------------------

DROP TABLE IF EXISTS stocks;

CREATE TABLE stocks (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL, 
    ticker TEXT, 
    figi TEXT, 
    description TEXT, 
    sector TEXT, 
    is_active INTEGER);

---------------------------------------------------------------------------

DROP TABLE IF EXISTS bonds;

CREATE TABLE bonds (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,
    ticker TEXT,
    figi TEXT,
    description TEXT,
    sector TEXT, 
    is_active INTEGER
);

---------------------------------------------------------------------------

DROP TABLE IF EXISTS futures;

CREATE TABLE futures (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,
    ticker TEXT,
    figi TEXT,
    description TEXT,
    sector TEXT, 
    is_active INTEGER
);

---------------------------------------------------------------------------

DROP TABLE IF EXISTS currencies;

CREATE TABLE currencies (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,
    ticker TEXT,
    figi TEXT,
    description TEXT,
    sector TEXT, 
    is_active INTEGER
);

---------------------------------------------------------------------------

DROP TABLE IF EXISTS settings;

CREATE TABLE settings (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,
    key TEXT,
    value TEXT,
    description TEXT
);

INSERT INTO settings (key, value, description)
VALUES ('Postgres:ConnectionString', 'User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=finmarket;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;', '������ ����� � �� �� �������');

INSERT INTO settings (key, value, description)
VALUES ('Tinkoff:Token', 't.szzlPYKzuUTNxiVrNJPRlueboUd1eQm1MceHyb6LB-yDZ7DrHV4gN-NWkDvPvFIHAArGZHXrcDzUCJJSkqtBog', '����� ������� � Tinkoff API');

INSERT INTO settings (key, value, description)
VALUES ('ApplicationSettings:Buffer', '300', '������� ������ �� ���� ������');

INSERT INTO settings (key, value, description)
VALUES ('Quartz:DowloadDaily:Cron', '0 5 * * * ? *', 'Cron-������ ��� ���������� ������� ������');

INSERT INTO settings (key, value, description)
VALUES ('Quartz:DowloadDaily:Enable', 'false', '�������� ���������� ������� ������');

INSERT INTO settings (key, value, description)
VALUES ('Quartz:DowloadHourly:Cron', '0 1,6,11,16,21,26,31,36,41,46,51,56 * * * ? *', 'Cron-������ ��� ���������� ������� ������');

INSERT INTO settings (key, value, description)
VALUES ('Quartz:DowloadHourly:Enable', 'false', '�������� ���������� ������� ������');