
CREATE TABLE Rol (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    TypeRol VARCHAR(50),
    Code VARCHAR(50),
    Name VARCHAR(100),
    Active TINYINT(1),
    DeleteAt DATETIME,
    CreateAt DATETIME
);

CREATE TABLE Permission (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100)
);

CREATE TABLE RolePermission (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    RolId INT,
    PermissionId INT,
    FOREIGN KEY (RolId) REFERENCES Rol(Id),
    FOREIGN KEY (PermissionId) REFERENCES Permission(Id)
);

CREATE TABLE Person (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100),
    Phone VARCHAR(20),
    Active TINYINT(1),
    DeleteAt DATETIME,
    CreateAt DATETIME
);

CREATE TABLE Institution (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100),
    Address TEXT,
    Phone VARCHAR(20),
    Commune VARCHAR(100),
    Department VARCHAR(100)
);

CREATE TABLE `User` (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100),
    Email VARCHAR(100),
    Password TEXT,
    Active TINYINT(1),
    DeleteAt DATETIME,
    CreateAt DATETIME,
    InstitutionId INT,
    PersonId INT,
    FOREIGN KEY (InstitutionId) REFERENCES Institution(Id),
    FOREIGN KEY (PersonId) REFERENCES Person(Id)
);

CREATE TABLE UserRol (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT,
    RolId INT,
    DeleteAt DATETIME,
    CreateAt DATETIME,
    FOREIGN KEY (UserId) REFERENCES User(Id),
    FOREIGN KEY (RolId) REFERENCES Rol(Id)
);

CREATE TABLE Experience (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    NameExperiences VARCHAR(150),
    Summary TEXT,
    Methodologies TEXT,
    Transfer TEXT,
    DateRegistration DATE,
    UserId INT,
    InstitutionId INT,
    Code VARCHAR(50),
    Active TINYINT(1),
    DeleteAt DATETIME,
    CreateAt DATETIME,
    FOREIGN KEY (UserId) REFERENCES User(Id),
    FOREIGN KEY (InstitutionId) REFERENCES Institution(Id)
);

CREATE TABLE Document (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Url TEXT,
    Name VARCHAR(100),
    ExperienceId INT,
    FOREIGN KEY (ExperienceId) REFERENCES Experience(Id)
);

CREATE TABLE Objective (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ObjectiveDescription TEXT,
    Innovation TINYINT(1),
    Results TEXT,
    Sustainability TEXT,
    ExperienceId INT,
    FOREIGN KEY (ExperienceId) REFERENCES Experience(Id)
);

CREATE TABLE Evaluation (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    TypeEvaluation VARCHAR(50),
    Comments TEXT,
    DateTime DATETIME,
    UserId INT,
    StateId INT,
    ExperienceId INT,
    FOREIGN KEY (UserId) REFERENCES User(Id),
    FOREIGN KEY (ExperienceId) REFERENCES Experience(Id)
);

CREATE TABLE EvaluationCriteria (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Score INT,
    EvaluationId INT,
    CriteriaId INT,
    FOREIGN KEY (EvaluationId) REFERENCES Evaluation(Id)
);

CREATE TABLE Criteria (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100)
);

CREATE TABLE State (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50)
);

CREATE TABLE HistoryExperience (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Action TEXT,
    DateTime DATETIME,
    UserId INT,
    TableName VARCHAR(100),
    AffectedId INT,
    Observation TEXT,
    Active TINYINT(1),
    StateId INT,
    FOREIGN KEY (UserId) REFERENCES User(Id),
    FOREIGN KEY (StateId) REFERENCES State(Id)
);

CREATE TABLE Grade (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100)
);

CREATE TABLE ExperienceGrade (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ExperienceId INT,
    GradeId INT,
    FOREIGN KEY (ExperienceId) REFERENCES Experience(Id),
    FOREIGN KEY (GradeId) REFERENCES Grade(Id)
);

CREATE TABLE PopulationGrade (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100)
);

CREATE TABLE ExperiencePopulation (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100),
    ExperienceId INT,
    PopulationGradeId INT,
    FOREIGN KEY (ExperienceId) REFERENCES Experience(Id),
    FOREIGN KEY (PopulationGradeId) REFERENCES PopulationGrade(Id)
);

CREATE TABLE LineThematic (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100),
    Code VARCHAR(50),
    Active TINYINT(1),
    DeleteAt DATETIME,
    CreateAt DATETIME
);

CREATE TABLE ExperienceLine (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    LineThematicId INT,
    ExperienceId INT,
    DeleteAt DATETIME,
    CreateAt DATETIME,
    FOREIGN KEY (LineThematicId) REFERENCES LineThematic(Id),
    FOREIGN KEY (ExperienceId) REFERENCES Experience(Id)
);
