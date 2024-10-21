DROP TABLE IF EXISTS stocks;

CREATE TABLE stocks (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL, 
    ticker TEXT, 
    figi TEXT, 
    isin TEXT, 
    description TEXT, 
    sector TEXT, 
    is_active INTEGER);
