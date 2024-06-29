Create Database TaskManagerDB;
Use TaskManagerDB;

CREATE TABLE Users (
   UserID INT PRIMARY KEY IDENTITY,
   UserName NVARCHAR(20) NOT NULL,
   FullName NVARCHAR(60) NOT NULL,
   PasswordHash NVARCHAR(255) NOT NULL,
   Email NVARCHAR(75) NOT NULL
);

CREATE TABLE TaskStatus (
   StatusID INT PRIMARY KEY IDENTITY,
   StatusName NVARCHAR(15) NOT NULL
);

Create Table Priority(
   PriorityID INT PRIMARY KEY IDENTITY,
   PriorityStatus NVarchar(15) NOT NULL
);

Create Table Tasks(
   TaskID INT PRIMARY KEY IDENTITY,
   Title NVARCHAR(30) NOT NULL,
   Description NVARCHAR(100),
   DueDate DATE NOT NULL,
   StatusID INT NOT NULL,
   UserID INT NOT NULL,
   PriorityID Int NOT NULL,
   CONSTRAINT fk_PriorityID FOREIGN KEY (PriorityID) REFERENCES Priority(PriorityID),
   CONSTRAINT FK_StatusID FOREIGN KEY (StatusID) REFERENCES TaskStatus(StatusID),
   CONSTRAINT FK_UserID FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

 Insert Into Priority (PriorityStatus) Values ('Low'), ('Medium'), ('High'); /* not modify */

 Insert Into TaskStatus (StatusName) Values ('Pending'), ('In Progress'), ('Completed'); /* not modify */

 Insert Into Users (UserName, FullName, PasswordHash, Email) Values ('Admin', 'Admin', 'Admin', 'TaskManager@Example.Com');

