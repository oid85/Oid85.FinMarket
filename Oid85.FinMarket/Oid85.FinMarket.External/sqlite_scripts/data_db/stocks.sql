DROP TABLE IF EXISTS stocks;

CREATE TABLE stocks (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL, 
    ticker TEXT, 
    figi TEXT, 
    isin TEXT, 
    description TEXT, 
    sector TEXT,     
    record_date TEXT,
    declared_date TEXT,
    dividend REAL, 
    dividend_prc REAL,
    is_active INTEGER
);
