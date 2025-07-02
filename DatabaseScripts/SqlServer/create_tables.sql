CREATE TABLE DepartmentMaster (
    Department NVARCHAR(255) NOT NULL PRIMARY KEY
);

CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    DOJ DATETIME NOT NULL,
    Department NVARCHAR(255) NOT NULL,
    CONSTRAINT FK_Employees_DepartmentMaster_Department FOREIGN KEY (Department)
        REFERENCES DepartmentMaster(Department)
);
