DROP TABLE IF EXISTS bonds;

CREATE TABLE bonds (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,
    ticker TEXT,
    figi TEXT,
    isin TEXT, 
    description TEXT,
    sector TEXT, 
    is_active INTEGER
);
