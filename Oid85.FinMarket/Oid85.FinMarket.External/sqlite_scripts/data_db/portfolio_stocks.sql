DROP TABLE IF EXISTS portfolio_stocks;

CREATE TABLE portfolio_stocks (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,      
    ticker TEXT,
    sector TEXT,
    number_shares INTEGER, 
    price REAL, 
    prc REAL);

INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc) VALUES ('SNGSP', '', 5200, 0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc) VALUES ('BSPBP', '', 4000, 0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc) VALUES ('LKOH',  '', 35,   0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc) VALUES ('FLOT',  '', 1620, 0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc) VALUES ('TATNP', '', 350,  0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc) VALUES ('SBER',  '', 1000, 0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc) VALUES ('MOEX',  '', 950,  0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc) VALUES ('NVTK',  '', 125,  0.0, 0.0);
INSERT INTO portfolio_stocks (ticker, sector, number_shares, price, prc) VALUES ('NLMK',  '', 1000, 0.0, 0.0);
