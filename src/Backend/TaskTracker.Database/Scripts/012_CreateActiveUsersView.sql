CREATE VIEW ActiveUsers AS
SELECT *
FROM Users
WHERE IsDeleted = 0;