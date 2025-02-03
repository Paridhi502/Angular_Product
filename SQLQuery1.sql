select * from dbo.Products;
alter table dbo.Products drop column category;
select * from dbo.Products;

ALTER TABLE dbo.Products 
ADD ReleaseDate DATE NOT NULL;

INSERT INTO Products (Name, Description, Price, ReleaseDate)
VALUES
('iPhone 15 Pro', 'Latest iPhone model with A17 chip and improved camera system', 149999, '2023-09-22'),
('Samsung Galaxy S23 Ultra', 'Flagship phone with 200MP camera and Snapdragon 8 Gen 2', 124999, '2023-02-17'),
('OnePlus 11 5G', '5G-enabled phone with Snapdragon 8 Gen 2 and 120Hz AMOLED display', 54999, '2023-02-07'),
('Xiaomi 13 Pro', 'Premium smartphone with Leica optics and 50MP camera', 74999, '2023-02-26'),
('Realme GT 2 Pro', 'High-performance phone with Snapdragon 8 Gen 1 and 120Hz AMOLED', 36999, '2022-03-15'),
('Oppo Reno 8 Pro 5G', 'Mid-range phone with 50MP camera and AMOLED display', 37999, '2022-07-18'),
('Vivo X90 Pro 5G', 'Flagship phone with Zeiss optics and excellent display quality', 82999, '2022-12-15'),
('Google Pixel 8 Pro', 'Google’s latest phone with Tensor G3 chip and exceptional camera quality', 89999, '2023-10-04'),
('Motorola Edge 30 Pro', '5G phone with Snapdragon 8 Gen 1 and OLED display', 49999, '2022-02-24'),
('Samsung Galaxy Z Fold 5', 'Foldable phone with large screen and multitasking features', 179999, '2023-08-11');

CREATE PROCEDURE AddProducts 
    @Name NVARCHAR(150),
    @Description NVARCHAR(MAX),
    @Price DECIMAL(18,2),
    @ReleaseDate DATE
AS
BEGIN
    INSERT INTO Products (Name, Description, Price, ReleaseDate)
    VALUES (@Name, @Description, @Price, @ReleaseDate);
END

CREATE PROCEDURE GetAllProducts
AS
BEGIN
    SELECT ProductId, Name, Description, Price, ReleaseDate FROM Products;
END

CREATE PROCEDURE GetProductById
    @id INT 
AS
BEGIN
    SELECT ProductId, Name, Description, Price, ReleaseDate 
    FROM Products 
    WHERE ProductId = @id;
END


CREATE PROCEDURE UpdateProduct 
    @ProductId INT,
    @Name NVARCHAR(150),
    @Description NVARCHAR(MAX),
    @Price DECIMAL(18,2),
    @ReleaseDate DATE
AS
BEGIN
    UPDATE Products
    SET Name = @Name, Description = @Description, Price = @Price, ReleaseDate = @ReleaseDate
    WHERE ProductId = @ProductId;
END

CREATE PROCEDURE DeleteProduct 
    @ProductId INT
AS
BEGIN
    DELETE FROM Products WHERE ProductId = @ProductId;
END


SELECT * FROM sys.procedures;

SELECT @@SERVERNAME
EXEC GetProductById @id = 1;
