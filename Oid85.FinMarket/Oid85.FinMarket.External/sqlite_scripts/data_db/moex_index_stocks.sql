DROP TABLE IF EXISTS moex_index_stocks;

CREATE TABLE moex_index_stocks (
    id INTEGER PRIMARY KEY UNIQUE NOT NULL,      
    ticker TEXT, 
    sector TEXT,
    number_shares INTEGER, 
    price REAL, 
    prc REAL,
    dividend_date TEXT,
    dividend REAL, 
    dividend_prc REAL
);

INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('LKOH',  '', 692865762,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('SBER',  '', 21586948000,   0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('SBERP', '', 1000000000,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('GAZP',  '', 23673512900,   0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('TATN',  '', 2178690700,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('YDEX',  '', 379453795,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('GMKN',  '', 15286339700,   0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('TCSG',  '', 199305492,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('NVTK',  '', 3036306000,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('SNGS',  '', 35725994705,   0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('PLZL',  '', 136069400,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('SNGSP', '', 7701998235,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('ROSN',  '', 10598177817,   0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('CHMF',  '', 837718660,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('PIKK',  '', 660497344,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('IRAO',  '', 104400000000,  0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('NLMK',  '', 5993227240,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('OZON',  '', 216413733,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('FIVE',  '', 271572872,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('TATNP', '', 147508500,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('MOEX',  '', 2276401458,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('RUAL',  '', 15193014862,   0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('MAGN',  '', 11174330000,   0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('MGNT',  '', 101911355,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('ALRS',  '', 7364965630,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('RTKM',  '', 3282997929,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('MTSS',  '', 1998381575,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('VTBR',  '', 5369933893,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('AGRO',  '', 136666665,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('AFLT',  '', 3975771215,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('POSI',  '', 66000000,      0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('PHOR',  '', 129500000,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('CBOM',  '', 33429709866,   0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('AFKS',  '', 9650000000,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('VKCO',  '', 227874940,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('TRNFP', '', 155487500,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('BSPB',  '', 457544031,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('FLOT',  '', 2374993901,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('FEES',  '', 2113460101477, 0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('ENPG',  '', 638848896,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('ASTR',  '', 210000000,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('MSNG',  '', 39749359700,   0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('GLTR',  '', 178318259,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('UPRO',  '', 63048706145,   0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('SELG',  '', 1030000000,    0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('MTLR',  '', 416270745,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('HYDR',  '', 444793377038,  0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('LEAS',  '', 120000000,     0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('SMLT',  '', 61579358,      0.0, 0.0, '', 0.0, 0.0);
INSERT INTO moex_index_stocks (ticker, sector, number_shares, price, prc, dividend_date, dividend, dividend_prc) VALUES ('MTLRP', '', 138756915,     0.0, 0.0, '', 0.0, 0.0);
