CREATE TABLE IF NOT EXISTS Products (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL CHECK (length(Name) BETWEEN 2 AND 100),
    Brand TEXT NOT NULL CHECK (length(Brand) BETWEEN 2 AND 60),
    Price REAL NOT NULL CHECK (Price > 0 AND Price <= 1000000),
    Stock INTEGER NOT NULL CHECK (Stock BETWEEN 0 AND 1000000),
    Description TEXT NOT NULL DEFAULT '' CHECK (length(Description) <= 500)
);

INSERT INTO Products (Name, Brand, Price, Stock, Description)
SELECT 'Seamaster Diver 300M', 'Omega', 5400.00, 4, 'A versatile automatic dive watch.'
WHERE NOT EXISTS (SELECT 1 FROM Products);

INSERT INTO Products (Name, Brand, Price, Stock, Description)
SELECT 'Datejust 36', 'Rolex', 8200.00, 2, 'A classic everyday watch with a date display.'
WHERE (SELECT COUNT(*) FROM Products) = 1;

INSERT INTO Products (Name, Brand, Price, Stock, Description)
SELECT 'Presage Cocktail Time', 'Seiko', 450.00, 7, 'An automatic dress watch with a patterned dial.'
WHERE (SELECT COUNT(*) FROM Products) = 2;
