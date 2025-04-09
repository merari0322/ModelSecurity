CREATE TABLE "Permission" (
    "Id"SERIAL PRIMARY KEY,
    PermissionType VARCHAR(100)
);

CREATE TABLE Rol (
    "Id" SERIAL PRIMARY KEY,
    TypeRol VARCHAR(50),
    Code VARCHAR(50),
    "Name" VARCHAR(100),
    Active BOOLEAN,
    DeleteAt TIMESTAMP,
    CreateAt TIMESTAMP
);

CREATE TABLE RolePermission (
    "Id" SERIAL PRIMARY KEY,
    RolId INT REFERENCES Rol("Id"),
    PermissionId INT REFERENCES "Permission"("Id")
);

CREATE TABLE Person (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100),
    Phone VARCHAR(20),
    Active BOOLEAN,
    DeleteAt TIMESTAMP,
    CreateAt TIMESTAMP
);

CREATE TABLE Institution (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100),
    Address TEXT,
    Phone VARCHAR(20),
    Commune VARCHAR(100),
    Department VARCHAR(100)
);

CREATE TABLE "User" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100),
    Email VARCHAR(100),
    "Password" TEXT,
    Active BOOLEAN,
    DeleteAt TIMESTAMP,
    CreateAt TIMESTAMP,
    InstitutionId INT REFERENCES Institution("Id"),
    PersonId INT REFERENCES Person("Id")
);

CREATE TABLE UserRol (
    "Id" SERIAL PRIMARY KEY,
    UserId INT REFERENCES "User"("Id"),
    RolId INT REFERENCES Rol("Id"),
    DeleteAt TIMESTAMP,
    CreateAt TIMESTAMP
);

CREATE TABLE Experience (
    "Id" SERIAL PRIMARY KEY,
    NameExperiences VARCHAR(150),
    Summary TEXT,
    Methodologies TEXT,
    Transfer TEXT,
    DateRegistration DATE,
    UserId INT REFERENCES "User"("Id"),
    InstitutionId INT REFERENCES Institution("Id"),
    Code VARCHAR(50),
    Active BOOLEAN,
    DeleteAt TIMESTAMP,
    CreateAt TIMESTAMP
);

CREATE TABLE "Document" (
    "Id" SERIAL PRIMARY KEY,
    Url TEXT,
    "Name" VARCHAR(100),
    ExperienceId INT REFERENCES Experience("Id")
);

CREATE TABLE Objective (
    "Id" SERIAL PRIMARY KEY,
    ObjectiveDescription TEXT,
    Innovation BOOLEAN,
    Results TEXT,
    Sustainability TEXT,
    ExperienceId INT REFERENCES Experience("Id")
);

CREATE TABLE Evaluation (
    "Id" SERIAL PRIMARY KEY,
    TypeEvaluation VARCHAR(50),
    "Comments" TEXT,
    DateTime TIMESTAMP,
    UserId INT REFERENCES "User"("Id"),
    StateId INT,
    ExperienceId INT REFERENCES Experience("Id")
);

CREATE TABLE EvaluationCriteria (
    "Id" SERIAL PRIMARY KEY,
    Score INT,
    EvaluationId INT REFERENCES Evaluation("Id"),
    CriteriaId INT
);

CREATE TABLE Criteria (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100)
);

CREATE TABLE State (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(50)
);

CREATE TABLE HistoryExperience (
    "Id" SERIAL PRIMARY KEY,
    "Action" TEXT,
    DateTime TIMESTAMP,
    UserId INT REFERENCES "User"(Id),
    TableName VARCHAR(100),
    AffectedId INT,
    Observation TEXT,
    Active BOOLEAN,
    StateId INT REFERENCES "State"("Id")
);

CREATE TABLE Grade (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100)
);

CREATE TABLE ExperienceGrade (
    "Id" SERIAL PRIMARY KEY,
    ExperienceId INT REFERENCES Experience("Id"),
    GradeId INT REFERENCES Grade("Id")
);

CREATE TABLE PopulationGrade (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100)
);

CREATE TABLE ExperiencePopulation (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100),
    ExperienceId INT REFERENCES Experience("Id"),
    PopulationGradeId INT REFERENCES PopulationGrade("Id")
);

CREATE TABLE LineThematic (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100),
    Code VARCHAR(50),
    Active BOOLEAN,
    DeleteAt TIMESTAMP,
    CreateAt TIMESTAMP
);

CREATE TABLE ExperienceLine (
    "Id" SERIAL PRIMARY KEY,
    LineThematicId INT REFERENCES LineThematic("Id"),
    ExperienceId INT REFERENCES Experience("Id"),
    DeleteAt TIMESTAMP,
    CreateAt TIMESTAMP
);
