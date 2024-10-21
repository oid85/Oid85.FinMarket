DROP TABLE IF EXISTS dividends;

CREATE TABLE dividends (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,
    ticker TEXT,
    dividend_date TEXT,
    dividend REAL, 
    dividend_prc REAL
);
