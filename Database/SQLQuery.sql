
--CREATE DATABASE TicketManagementSystem;
use TicketManagementSystem;

-- 1. ROLES TABLE

CREATE TABLE Roles (
    id		INT PRIMARY KEY IDENTITY(1,1), -- IDENTITY(1,1) is correct for SQL Server
    name	VARCHAR(50) NOT NULL UNIQUE
			CONSTRAINT CHK_RoleName CHECK (name IN ('MANAGER', 'SUPPORT', 'USER'))
);

INSERT INTO Roles VALUES ('MANAGER'),('SUPPORT'),('USER');

select * from Roles;

--1. 'MANAGER'
--2. 'SUPPORT'
--3. 'USER'

-- 2. USERS TABLE

CREATE TABLE Users (
    id			INT IDENTITY(1,1) PRIMARY KEY,
    name		VARCHAR(255) NOT NULL,
    email		VARCHAR(255) NOT NULL UNIQUE,
    password	VARCHAR(255) NOT NULL, -- Store bcrypt hash password
    role_id		INT NOT NULL,
    created_at	DATETIME DEFAULT GETDATE(),

	-- Foreign Key Constraints
	FOREIGN KEY (role_id) REFERENCES roles(id)
);

INSERT INTO users (name, email, password, role_id)
VALUES 
('Manager', 'manager@gmail.com', 'password', 1),
('Support', 'support@gmail.com', 'password', 2),
('User', 'user@gmail.com', 'password', 3);

select * from Users;

-- 3. Tickets TABLE

CREATE TABLE Tickets (
    id				INT IDENTITY(1,1) PRIMARY KEY,
    title			VARCHAR(255) NOT NULL,
    description		TEXT NOT NULL,
    status			VARCHAR(20) NOT NULL CONSTRAINT CHK_TicketStatus CHECK (status IN ('OPEN', 'IN_PROGRESS', 'RESOLVED', 'CLOSED')) DEFAULT 'OPEN',
    priority		VARCHAR(20) NOT NULL CONSTRAINT CHK_TicketPriority CHECK (priority IN ('LOW', 'MEDIUM', 'HIGH')) DEFAULT 'MEDIUM',
    created_by		INT NOT NULL,
    assigned_to		INT NULL,
    created_at		DATETIME DEFAULT GETDATE(),

    -- Foreign Key Constraints
    CONSTRAINT FK_created_by FOREIGN KEY (created_by) REFERENCES users(id),
    CONSTRAINT FK_assigned_to FOREIGN KEY (assigned_to) REFERENCES users(id)
);

INSERT INTO tickets (title, description, status, priority, created_by, assigned_to)
VALUES 
('Fix login bug', 'Users unable to log in due to a server error.', 'OPEN', 'HIGH', 1, NULL),
('Update styling on homepage', 'The CSS on the homepage is outdated and needs a refresh.', 'IN_PROGRESS', 'MEDIUM', 2, 3),
('New feature request', 'Add an option to export data to Excel format.', 'RESOLVED', 'LOW', 3, 2),
('Database optimization', 'Review slow queries and optimize database performance.', 'CLOSED', 'HIGH', 1, 1);

-- 4. ticket_comments TABLE
CREATE TABLE ticket_comments (
    id					INT IDENTITY(1,1) PRIMARY KEY,
    ticket_id			INT NOT NULL,
    user_id				INT NOT NULL,
    comment				TEXT NOT NULL,
    created_at			DATETIME DEFAULT GETDATE(),

	-- Foreign Key Constraints
    CONSTRAINT fk_ticket_comments_ticket_id FOREIGN KEY (ticket_id) REFERENCES tickets(id) ON DELETE CASCADE,
    CONSTRAINT fk_ticket_comments_user_id FOREIGN KEY (user_id) REFERENCES users(id)
);

INSERT INTO ticket_comments (ticket_id, user_id, comment)
VALUES 
(1, 1, 'A follow-up comment providing more details.'),
(1, 1, 'Another update on the ticket progress.'),
(1, 1, 'Final resolution details added to the comments.');

-- 5. ticket_status_logs TABLE

CREATE TABLE ticket_status_logs (
    id				INT IDENTITY(1,1) PRIMARY KEY,
    ticket_id		INT NOT NULL,
    old_status		VARCHAR(20) NOT NULL,
    new_status		VARCHAR(20) NOT NULL,
    changed_by		INT,
    changed_at		DATETIME DEFAULT GETDATE(),

    -- Constraints
    CONSTRAINT FK_TicketStatusLogs_Tickets 
        FOREIGN KEY (ticket_id) REFERENCES tickets(id) 
        ON DELETE CASCADE,                
    CONSTRAINT FK_TicketStatusLogs_Users 
        FOREIGN KEY (changed_by) REFERENCES users(id),                
    CONSTRAINT CHK_TicketStatusLogs_OldStatus 
        CHECK (old_status IN ('OPEN', 'IN_PROGRESS', 'RESOLVED', 'CLOSED')),
    CONSTRAINT CHK_TicketStatusLogs_NewStatus 
        CHECK (new_status IN ('OPEN', 'IN_PROGRESS', 'RESOLVED', 'CLOSED'))
);

INSERT INTO ticket_status_logs (ticket_id, old_status, new_status, changed_by, changed_at)
VALUES 
(1, 'OPEN', 'IN_PROGRESS', 2, '2023-10-25 09:00:00'),
(1, 'IN_PROGRESS', 'RESOLVED', 2, '2023-10-25 14:00:00'),
(1, 'RESOLVED', 'CLOSED', 1, '2023-10-26 10:00:00'),

(2, 'OPEN', 'IN_PROGRESS', 1, '2023-10-25 10:00:00'),
(2, 'IN_PROGRESS', 'OPEN', 1, '2023-10-25 16:00:00'),

(3, 'OPEN', 'IN_PROGRESS', 2, DEFAULT);

Select * from Roles;
Select * from Users;
Select * from Tickets;
Select * from ticket_comments;
select * from ticket_status_logs;