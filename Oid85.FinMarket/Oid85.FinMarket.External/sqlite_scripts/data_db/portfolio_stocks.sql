DROP TABLE IF EXISTS portfolio_stocks;

CREATE TABLE portfolio_stocks (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,      
    ticker TEXT,
    sector TEXT,
    number_shares INTEGER, 
    price REAL, 
    prc REAL,
    record_date TEXT,
    declared_date TEXT,
    dividend REAL, 
    dividend_prc REAL
);

INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc, record_date, declared_date, dividend, dividend_prc) VALUES ('SNGSP', '', 5200, 0.0, 0.0, '', '', 0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc, record_date, declared_date, dividend, dividend_prc) VALUES ('BSPBP', '', 4000, 0.0, 0.0, '', '', 0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc, record_date, declared_date, dividend, dividend_prc) VALUES ('LKOH',  '', 35,   0.0, 0.0, '', '', 0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc, record_date, declared_date, dividend, dividend_prc) VALUES ('FLOT',  '', 1620, 0.0, 0.0, '', '', 0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc, record_date, declared_date, dividend, dividend_prc) VALUES ('TATNP', '', 350,  0.0, 0.0, '', '', 0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc, record_date, declared_date, dividend, dividend_prc) VALUES ('SBER',  '', 1000, 0.0, 0.0, '', '', 0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc, record_date, declared_date, dividend, dividend_prc) VALUES ('MOEX',  '', 950,  0.0, 0.0, '', '', 0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc, record_date, declared_date, dividend, dividend_prc) VALUES ('NVTK',  '', 125,  0.0, 0.0, '', '', 0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc, record_date, declared_date, dividend, dividend_prc) VALUES ('NLMK',  '', 1000, 0.0, 0.0, '', '', 0.0, 0.0);
