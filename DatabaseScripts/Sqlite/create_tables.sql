CREATE TABLE DepartmentMaster (
    Department TEXT NOT NULL PRIMARY KEY
);

CREATE TABLE Employees (
    EmployeeID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    DOJ TEXT NOT NULL,
    Department TEXT NOT NULL,
    FOREIGN KEY (Department) REFERENCES DepartmentMaster(Department)
);
