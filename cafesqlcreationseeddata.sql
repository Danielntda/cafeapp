CREATE DATABASE cafedatabase;
USE cafedatabase;

CREATE TABLE cafe (
    id CHAR(36) NOT NULL PRIMARY KEY,        
    name VARCHAR(255) NOT NULL,              
    description TEXT NOT NULL,               
    logo VARCHAR(500) NULL,                  
    location VARCHAR(255) NOT NULL           
);

CREATE TABLE employee (
    id VARCHAR(10) NOT NULL PRIMARY KEY,     
    name VARCHAR(100) NOT NULL,
    email_address VARCHAR(255) NOT NULL,
    phone_number CHAR(8) NOT NULL,
    gender ENUM('Male', 'Female') NOT NULL,
    cafe_id CHAR(36) NULL,
    start_date DATE NULL,
    FOREIGN KEY (cafe_id) REFERENCES cafe(id) ON DELETE CASCADE
);

-- seed data
USE cafedatabase;

INSERT INTO cafe (id, name, description, logo, location) VALUES
('1a2b3c4d-0000-0000-0000-000000000001', 'Cafe Test One', 'Cafe for testing purpose 1.', 'https://placehold.co/600x400', '123 Test Road'),
('1a2b3c4d-0000-0000-0000-000000000002', 'Test Two Cafe', 'Second Cafe.', 'https://placehold.co/600x400', '456 Street');

INSERT INTO employee (id, name, email_address, phone_number, gender, cafe_id, start_date) VALUES
('UI0000001', 'Ali', 'ali@test.com', '91111111', 'Female', '1a2b3c4d-0000-0000-0000-000000000001','2024-04-26'),
('UI0000002', 'Muthu Samy', 'muthu@test.com', '92222222', 'Male', '1a2b3c4d-0000-0000-0000-000000000001', '2024-12-01'),
('UI0000003', 'Xiao Ming', 'xm@test.com', '93333333', 'Male', '1a2b3c4d-0000-0000-0000-000000000002', '2025-07-10'),
('UI0000004', 'Daniel Ng', 'daniel@test.com', '94444444', 'Male', '1a2b3c4d-0000-0000-0000-000000000002', '2025-06-15');
