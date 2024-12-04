-- Create database

CREATE DATABASE MembershipSystem
GO
USE MembershipSystem
GO

-- Employees Table
CREATE TABLE EMPLOYEES (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(100) NOT NULL,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    active BIT DEFAULT 1,
    last_login DATETIME
)

-- Users Table
CREATE TABLE USERS (
    id INT IDENTITY(1,1) PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(20),
    address VARCHAR(200),
    registration_date DATETIME DEFAULT GETDATE(),
    active BIT DEFAULT 1
)

-- Memberships Table
CREATE TABLE MEMBERSHIPS (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(200),
    duration_months INT NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    active bIT DEFAULT 1,
    price DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES USERS(id)
)

SELECT COUNT(*) FROM Memberships;
SELECT COUNT(*) FROM Users;
SELECT m.*, CONCAT(u.first_name, ' ', u.last_name) as UserFullName 
FROM Memberships m 
INNER JOIN Users u ON m.user_id = u.id;


-- Payments Table
CREATE TABLE PAYMENTS (
    id INT IDENTITY(1,1) PRIMARY KEY,
    membership_id INT NOT NULL,
    amount DECIMAL(10,2) NOT NULL,
    payment_date DATETIME DEFAULT GETDATE(),
    status VARCHAR(20) NOT NULL,
    reference VARCHAR(100),
    FOREIGN KEY (membership_id) REFERENCES MEMBERSHIPS(id)
)

-- DATA
l
-- Insertar Usuarios
INSERT INTO USERS (first_name, last_name, email, phone, address, active) VALUES
('Carlos', 'López', 'carlos.lopez@email.com', '555-0101', 'Av. Principal 123', 1),
('Ana', 'Martínez', 'ana.martinez@email.com', '555-0102', 'Calle Central 456', 1),
('Roberto', 'Díaz', 'roberto.diaz@email.com', '555-0103', 'Plaza Mayor 789', 1),
('Laura', 'Sánchez', 'laura.sanchez@email.com', '555-0104', 'Av. Libertad 321', 1);

-- Insertar Membresías
INSERT INTO MEMBERSHIPS (user_id, name, description, duration_months, start_date, end_date, status, price) VALUES
(1, 'Plan Mensual', 'Acceso completo por un mes', 1, '2024-01-01', '2024-02-01', 'ACTIVE', 49.99),
(2, 'Plan Trimestral', 'Acceso completo por tres meses', 3, '2024-01-01', '2024-04-01', 'ACTIVE', 129.99),
(3, 'Plan Semestral', 'Acceso completo por seis meses', 6, '2024-01-01', '2024-07-01', 'ACTIVE', 249.99),
(4, 'Plan Anual', 'Acceso completo por un año', 12, '2024-01-01', '2025-01-01', 'ACTIVE', 449.99);

-- Insertar Pagos
INSERT INTO PAYMENTS (membership_id, amount, payment_date, status, reference) VALUES
(1, 49.99, '2024-01-01',  'COMPLETED', 'REF-001-2024'),
(2, 129.99, '2024-01-01',  'COMPLETED', 'REF-002-2024'),
(3, 249.99, '2024-01-01',  'COMPLETED', 'REF-003-2024'),
(4, 449.99, '2024-01-01',  'COMPLETED', 'REF-004-2024');




-- Insert an employee for testing
INSERT INTO EMPLOYEES (
    username, 
    password_hash, 
    first_name, 
    last_name, 
    email, 
    active
) VALUES (
    'admin',
    'admin123',  
    'System',
    'Administrator',
    'admin@system.com',
    1
);

-- Create the updated stored procedure
CREATE PROCEDURE sp_GetUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        id AS UserId,
        username AS Username,
        first_name AS FirstName,
        last_name AS LastName,
        email AS Email,
        active AS IsActive
    FROM 
        EMPLOYEES
    WHERE 
        username = @Username 
        AND password_hash = @Password
        AND active = 1;
END
GO
