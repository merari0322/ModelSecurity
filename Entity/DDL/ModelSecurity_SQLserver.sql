-- =============================
-- Creacion de todas las tablas 
-- =============================

CREATE TABLE Rol (
	Id INT IDENTITY (1,1) PRIMARY KEY,
	TypeRol VARCHAR (50) NOT NULL,
	Code VARCHAR (50) NOT NULL,
	"Name" VARCHAR (50) NOT NULL,
	Active BIT NOT NULL,
	DeleteAt DATETIME NOT NULL,
	CreateAt DATETIME NOT NULL,

)

CREATE TABLE Permission (
    Id INT IDENTITY (1,1) PRIMARY KEY,
    permissiontype VARCHAR(50) NOT NULL,
    IdRolPermission INT,
    FOREIGN KEY (IdRolPermission) REFERENCES Rol(Id)  
);

CREATE TABLE RolPermission(
	Id INT IDENTITY (1,1) PRIMARY KEY ,
	RolId INT NOT NULL,
	PermissionId INT NOT NULL,
)

CREATE TABLE Document (
	 Id INT IDENTITY (1,1) PRIMARY KEY,
	 "Url" VARCHAR (255) NOT NULL,
	 "Name" VARCHAR (50) NOT NULL,
)

CREATE TABLE Verification (
	Id INT IDENTITY (1,1) PRIMARY KEY,
	"Name" VARCHAR (50) NOT NULL,
	"Description" VARCHAR (50) NOT NULL,
)

CREATE TABLE Grade (
	Id INT IDENTITY (1,1) PRIMARY KEY,
	"Name" VARCHAR (50) NOT NULL,
)

CREATE TABLE ExperienceGrade (
	Id INT IDENTITY (1,1) PRIMARY KEY,
	GradeId INT NOT NULL, 

)

CREATE TABLE Experience (
	Id INT IDENTITY(1,1) PRIMARY KEY,    
    NameExperiences VARCHAR(255) NOT NULL, 
    "DateTime" DATETIME NOT NULL,          
    DurationDays INT NULL,                
    DurationMonths INT NULL,          
    DurationYears INT NULL,              
    Summary VARCHAR(50) NULL,                     
    Methodologies VARCHAR(50) NULL,         
    "Transfer" VARCHAR(50) NULL,                  
    DateRegistration DATETIME DEFAULT SYSDATETIME(),
    UserId1 INT NOT NULL,                  
    InstitutionId1 INT NOT NULL,         
    Code VARCHAR(50) UNIQUE NOT NULL,    
    Active BIT NOT NULL,         
    DeleteAt DATETIME NULL,               
    CreateAt DATETIME DEFAULT SYSDATETIME(), 
)


CREATE TABLE Objective (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ObjectiveDescription TEXT NOT NULL,
    Innovation VARCHAR(255) NOT NULL,
    Results VARCHAR(255) NOT NULL,
    Sustainability VARCHAR(255) NOT NULL,
    ExperienceId1 INT NOT NULL,

)

CREATE TABLE Person (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Phone VARCHAR(20) NULL,
    Active BIT NOT NULL DEFAULT 1,
    DeleteAt DATETIME NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETDATE(),
    UserId INT NOT NULL,

)

CREATE TABLE [User] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Active BIT NOT NULL DEFAULT 1,
    DeleteAt DATETIME NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETDATE()
)


CREATE TABLE UserRol (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId1 INT NOT NULL,
    RolId1 INT NOT NULL,
    DeleteAt DATETIME NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETDATE(),

)


CREATE TABLE ExperiencePopulation (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    ExperienceId INT NOT NULL,
    PopulationGradeId INT NOT NULL,

)


CREATE TABLE PopulationGrade (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(255) NOT NULL
);


CREATE TABLE ExperienceLineThematic (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    LineThematicId1 INT NOT NULL,
    DeleteAt DATETIME NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETDATE(),

)


CREATE TABLE LineThematic (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Code VARCHAR(50) NOT NULL,
    Active BIT NOT NULL DEFAULT 1,
    DeleteAt DATETIME NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETDATE()
);


CREATE TABLE Evaluation (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TypeEvaluation VARCHAR(100) NOT NULL,
    Comments TEXT NULL,
    DateTime DATETIME NOT NULL DEFAULT GETDATE(),
    UserId1 INT NOT NULL,
    ExperienceId1 INT NOT NULL,

)


CREATE TABLE State (
    Id INT PRIMARY KEY,   -- Id como clave primaria
    Name NVARCHAR(100)    -- Nombre del estado
);


CREATE TABLE EvaluationCriteria (
    Id INT PRIMARY KEY,
    Score DECIMAL(5,2), 
    EvaluationId INT,
);

CREATE TABLE Criteria (
    Id INT PRIMARY KEY,
    Name NVARCHAR(100)
);
	

CREATE TABLE HistoryExperience (
    Id INT PRIMARY KEY,
    Action NVARCHAR(100),
    DateTime DATETIME,
    UserId1 INT,
    TableName NVARCHAR(100),
    Observation NVARCHAR(MAX),
    AfectedId INT,
    Active BIT,
    StateId INT,
);



-- ======================
-- Relaciones de User
-- ======================
ALTER TABLE [User] ADD CONSTRAINT FK_User_Institution FOREIGN KEY (InstitutionId) REFERENCES Institution(Id);
ALTER TABLE [User] ADD CONSTRAINT FK_User_Person FOREIGN KEY (PersonId) REFERENCES Person(Id);
ALTER TABLE [User] ADD CONSTRAINT FK_User_HistoryExperience FOREIGN KEY (HistoryExperienceId) REFERENCES HistoryExperience(Id);

-- ======================
-- Relaciones de HistoryExperience
-- ======================
ALTER TABLE HistoryExperience ADD CONSTRAINT FK_HistoryExperience_User FOREIGN KEY (UserId) REFERENCES [User](Id);
ALTER TABLE HistoryExperience ADD CONSTRAINT FK_HistoryExperience_State FOREIGN KEY (IdState) REFERENCES State(Id);

-- ======================
-- Relaciones de Experience
-- ======================
ALTER TABLE Experience ADD CONSTRAINT FK_Experience_User FOREIGN KEY (UserId) REFERENCES [User](Id);
ALTER TABLE Experience ADD CONSTRAINT FK_Experience_Institution FOREIGN KEY (InstitutionId) REFERENCES Institution(Id);

-- ======================
-- Relaciones de Evaluation
-- ======================
ALTER TABLE Evaluation ADD CONSTRAINT FK_Evaluation_User FOREIGN KEY (UserId) REFERENCES [User](Id);
ALTER TABLE Evaluation ADD CONSTRAINT FK_Evaluation_State FOREIGN KEY (IdState) REFERENCES State(Id);
ALTER TABLE Evaluation ADD CONSTRAINT FK_Evaluation_Experience FOREIGN KEY (IdExperience) REFERENCES Experience(Id);

-- ======================
-- Relaciones de EvaluationCriteria
-- ======================
ALTER TABLE EvaluationCriteria ADD CONSTRAINT FK_EvaluationCriteria_Evaluation FOREIGN KEY (EvaluationId) REFERENCES Evaluation(Id);
ALTER TABLE EvaluationCriteria ADD CONSTRAINT FK_EvaluationCriteria_Criteria FOREIGN KEY (IdCriteria) REFERENCES Criteria(Id);

-- ======================
-- Relaciones de ExperiencePopulation
-- ======================
ALTER TABLE ExperiencePopulation ADD CONSTRAINT FK_ExperiencePopulation_Experience FOREIGN KEY (ExperienceId) REFERENCES Experience(Id);
ALTER TABLE ExperiencePopulation ADD CONSTRAINT FK_ExperiencePopulation_PopulationGrade FOREIGN KEY (PopulationGradeId) REFERENCES PopulationGrade(Id);

-- ======================
-- Relaciones de ExperienceGrade
-- ======================
ALTER TABLE ExperienceGrade ADD CONSTRAINT FK_ExperienceGrade_Experience FOREIGN KEY (ExperienceId) REFERENCES Experience(Id);
ALTER TABLE ExperienceGrade ADD CONSTRAINT FK_ExperienceGrade_Grade FOREIGN KEY (GradeId) REFERENCES Grade(Id);

-- ======================
-- Relaciones de ExperienceLineThematic
-- ======================
ALTER TABLE ExperienceLineThematic ADD CONSTRAINT FK_ExperienceLineThematic_Experience FOREIGN KEY (ExperienceId) REFERENCES Experience(Id);
ALTER TABLE ExperienceLineThematic ADD CONSTRAINT FK_ExperienceLineThematic_LineThematic FOREIGN KEY (LineThematicId) REFERENCES LineThematic(Id);

-- ======================
-- Relaciones de RolePermission
-- ======================
ALTER TABLE RolePermission ADD CONSTRAINT FK_RolePermission_Rol FOREIGN KEY (RolId) REFERENCES Rol(Id);
ALTER TABLE RolePermission ADD CONSTRAINT FK_RolePermission_Permission FOREIGN KEY (PermissionId) REFERENCES Permission(Id);

-- ======================
-- Relaciones de UserRol
-- ======================
ALTER TABLE UserRol ADD CONSTRAINT FK_UserRol_User FOREIGN KEY (UserId) REFERENCES [User](Id);
ALTER TABLE UserRol ADD CONSTRAINT FK_UserRol_Rol FOREIGN KEY (RolId) REFERENCES Rol(Id);

-- ======================
-- Relaciones de Objective
-- ======================
ALTER TABLE Objective ADD CONSTRAINT FK_Objective_Experience FOREIGN KEY (ExperienceId1) REFERENCES Experience(Id);

-- ======================
-- Relaciones de Document
-- ======================
ALTER TABLE Document ADD CONSTRAINT FK_Document_Experience FOREIGN KEY (ExperienceId) REFERENCES Experience(Id);

-- ======================
-- Relaciones de Verification
-- ======================
ALTER TABLE Verification ADD CONSTRAINT FK_Verification_Experience FOREIGN KEY (ExperienceId) REFERENCES Experience(Id);


