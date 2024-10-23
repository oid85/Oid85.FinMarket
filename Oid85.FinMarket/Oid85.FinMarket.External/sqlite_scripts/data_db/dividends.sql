DROP TABLE IF EXISTS dividends;

CREATE TABLE dividends (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,
    ticker TEXT,
    record_date TEXT,
    declared_date TEXT,
    dividend REAL, 
    dividend_prc REAL
);
