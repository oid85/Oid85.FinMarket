DROP TABLE IF EXISTS currencies;

CREATE TABLE currencies (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,
    ticker TEXT,
    figi TEXT,
    description TEXT,
    sector TEXT, 
    is_active INTEGER
);
