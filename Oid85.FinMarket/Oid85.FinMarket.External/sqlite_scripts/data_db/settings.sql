DROP TABLE IF EXISTS settings;

CREATE TABLE settings (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,
    key TEXT,
    value TEXT,
    description TEXT
);

INSERT INTO settings (key, value, description)
VALUES ('Postgres:ConnectionString', 'User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=finmarket;', 'Строка связи к БД со свечами');

INSERT INTO settings (key, value, description)
VALUES ('Tinkoff:Token', 't.szzlPYKzuUTNxiVrNJPRlueboUd1eQm1MceHyb6LB-yDZ7DrHV4gN-NWkDvPvFIHAArGZHXrcDzUCJJSkqtBog', 'Токен доступа к Tinkoff API');

INSERT INTO settings (key, value, description)
VALUES ('ApplicationSettings:Buffer', '300', 'Минимум свечей за один запрос');

INSERT INTO settings (key, value, description)
VALUES ('Quartz:DowloadDaily:Cron', '0 5 * * * ? *', 'Cron-строка для скачивания дневных свечей');

INSERT INTO settings (key, value, description)
VALUES ('Quartz:DowloadDaily:Enable', 'false', 'Включено скачивание дневных свечей');

INSERT INTO settings (key, value, description)
VALUES ('Quartz:DowloadHourly:Cron', '0 1,6,11,16,21,26,31,36,41,46,51,56 * * * ? *', 'Cron-строка для скачивания часовых свечей');

INSERT INTO settings (key, value, description)
VALUES ('Quartz:DowloadHourly:Enable', 'false', 'Включено скачивание часовых свечей');
