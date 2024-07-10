

-- GetAllDataProducts.sql

USE [tt]
GO

CREATE PROCEDURE GetAllDataProducts
AS
BEGIN
    SELECT 
        p.Id AS ProductId,
        p.Name AS ProductName,
        b.Name AS BrandName,
        pr.Name AS PropertyName,
        pp.Value AS PropertyValue
    FROM 
        Products p
    INNER JOIN 
        Brand b ON p.BrandId = b.Id
    INNER JOIN 
        ProductProperties pp ON p.Id = pp.ProductId
    INNER JOIN 
        Properties pr ON pp.PropertyId = pr.Id
END
GO


 
 CREATE VIEW [dbo].[AllProperties_View]
AS
SELECT 
    pr.Id AS PropertyId,
    pr.Name AS PropertyName,
    pp.Value AS PropertyValue
FROM 
    Properties pr
INNER JOIN 
    ProductProperties pp ON pr.Id = pp.PropertyId;