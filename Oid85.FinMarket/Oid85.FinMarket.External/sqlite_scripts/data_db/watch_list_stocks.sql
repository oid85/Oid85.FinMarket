DROP TABLE IF EXISTS watch_list_stocks;

CREATE TABLE watch_list_stocks (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,      
    ticker TEXT, 
    price REAL);

INSERT INTO watch_list_stocks (ticker, price) VALUES ('SNGSP', 0.0);
INSERT INTO watch_list_stocks (ticker, price) VALUES ('BSPBP', 0.0);
INSERT INTO watch_list_stocks (ticker, price) VALUES ('LKOH',  0.0);
INSERT INTO watch_list_stocks (ticker, price) VALUES ('FLOT',  0.0);
INSERT INTO watch_list_stocks (ticker, price) VALUES ('TATNP', 0.0);
INSERT INTO watch_list_stocks (ticker, price) VALUES ('SBER',  0.0);
INSERT INTO watch_list_stocks (ticker, price) VALUES ('MOEX',  0.0);
INSERT INTO watch_list_stocks (ticker, price) VALUES ('NVTK',  0.0);
INSERT INTO watch_list_stocks (ticker, price) VALUES ('NLMK',  0.0);
