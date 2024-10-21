DROP TABLE IF EXISTS futures;

CREATE TABLE futures (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,
    ticker TEXT,
    figi TEXT,
    isin TEXT, 
    description TEXT,
    sector TEXT, 
    is_active INTEGER
);
