DROP TABLE IF EXISTS watch_list_stocks;

CREATE TABLE watch_list_stocks (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,      
    ticker TEXT, 
    sector TEXT,
    price REAL);

INSERT INTO watch_list_stocks (ticker, sector, price) VALUES ('SNGSP', '', 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price) VALUES ('BSPBP', '', 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price) VALUES ('LKOH',  '', 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price) VALUES ('FLOT',  '', 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price) VALUES ('TATNP', '', 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price) VALUES ('SBER',  '', 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price) VALUES ('MOEX',  '', 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price) VALUES ('NVTK',  '', 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price) VALUES ('NLMK',  '', 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price) VALUES ('GAZP',  '', 0.0);
