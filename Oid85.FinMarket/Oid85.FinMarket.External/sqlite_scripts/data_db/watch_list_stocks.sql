DROP TABLE IF EXISTS watch_list_stocks;

CREATE TABLE watch_list_stocks (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,      
    ticker TEXT, 
    sector TEXT,
    price REAL,
    record_date TEXT,
    declared_date TEXT,
    dividend REAL, 
    dividend_prc REAL
);

INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('SNGSP', '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('BSPB',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('LKOH',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('FLOT',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('TATNP', '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('SBER',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('MOEX',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('NVTK',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('NLMK',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('GAZP',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('IRKT',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('SGZH',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('AQUA',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('LSNG',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('GCHE',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('CHMF',  '', 0.0, '', '', 0.0, 0.0);
INSERT INTO watch_list_stocks (ticker, sector, price, record_date, declared_date, dividend, dividend_prc) VALUES ('AFLT',  '', 0.0, '', '', 0.0, 0.0);
