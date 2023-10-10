
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/23/2017 16:59:07
-- Generated from EDMX file: C:\Users\user01\Desktop\adentispro\adentispro\adentispro\HIQ-Training\HIQTrainingSite\HIQTraining\Model\HIQTrainingDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [HIQTraining];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_Attendance_Calendar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Attendances] DROP CONSTRAINT [FK_Attendance_Calendar];
GO
IF OBJECT_ID(N'[dbo].[FK_Attendance_Inscription]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Attendances] DROP CONSTRAINT [FK_Attendance_Inscription];
GO
IF OBJECT_ID(N'[dbo].[FK_Calendar_Course]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Calendars] DROP CONSTRAINT [FK_Calendar_Course];
GO
IF OBJECT_ID(N'[dbo].[FK_Certification_CertificationType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Certifications] DROP CONSTRAINT [FK_Certification_CertificationType];
GO
IF OBJECT_ID(N'[dbo].[FK_Certification_FormativeEntity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Certifications] DROP CONSTRAINT [FK_Certification_FormativeEntity];
GO
IF OBJECT_ID(N'[dbo].[FK_Certification_Student]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Certifications] DROP CONSTRAINT [FK_Certification_Student];
GO
IF OBJECT_ID(N'[dbo].[FK_Student_Company]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_Student_Company];
GO
IF OBJECT_ID(N'[dbo].[FK_Teacher_Company]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Teachers] DROP CONSTRAINT [FK_Teacher_Company];
GO
IF OBJECT_ID(N'[dbo].[FK_Course_CourseLevel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Courses] DROP CONSTRAINT [FK_Course_CourseLevel];
GO
IF OBJECT_ID(N'[dbo].[FK_Course_CourseLocation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Courses] DROP CONSTRAINT [FK_Course_CourseLocation];
GO
IF OBJECT_ID(N'[dbo].[FK_Course_FormativeEntity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Courses] DROP CONSTRAINT [FK_Course_FormativeEntity];
GO
IF OBJECT_ID(N'[dbo].[FK_Course_Teacher]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Courses] DROP CONSTRAINT [FK_Course_Teacher];
GO
IF OBJECT_ID(N'[dbo].[FK_Inscription_Course]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Inscriptions] DROP CONSTRAINT [FK_Inscription_Course];
GO
IF OBJECT_ID(N'[dbo].[FK_Inscription_InscriptionType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Inscriptions] DROP CONSTRAINT [FK_Inscription_InscriptionType];
GO
IF OBJECT_ID(N'[dbo].[FK_Inscription_Student]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Inscriptions] DROP CONSTRAINT [FK_Inscription_Student];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRole];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUser];
GO
IF OBJECT_ID(N'[dbo].[FK_CourseTypeCourse]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Courses] DROP CONSTRAINT [FK_CourseTypeCourse];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[Attendances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Attendances];
GO
IF OBJECT_ID(N'[dbo].[Calendars]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Calendars];
GO
IF OBJECT_ID(N'[dbo].[Certifications]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Certifications];
GO
IF OBJECT_ID(N'[dbo].[CertificationTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CertificationTypes];
GO
IF OBJECT_ID(N'[dbo].[Companies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Companies];
GO
IF OBJECT_ID(N'[dbo].[Courses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Courses];
GO
IF OBJECT_ID(N'[dbo].[CourseLevels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CourseLevels];
GO
IF OBJECT_ID(N'[dbo].[CourseLocations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CourseLocations];
GO
IF OBJECT_ID(N'[dbo].[FormativeEntities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FormativeEntities];
GO
IF OBJECT_ID(N'[dbo].[Inscriptions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Inscriptions];
GO
IF OBJECT_ID(N'[dbo].[InscriptionTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InscriptionTypes];
GO
IF OBJECT_ID(N'[dbo].[Logs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Logs];
GO
IF OBJECT_ID(N'[dbo].[Students]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Students];
GO
IF OBJECT_ID(N'[dbo].[Teachers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Teachers];
GO
IF OBJECT_ID(N'[dbo].[CourseTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CourseTypes];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'Attendances'
CREATE TABLE [dbo].[Attendances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InscriptionId] int  NOT NULL,
    [CalendarId] int  NOT NULL,
    [Status] int  NOT NULL,
    [Observation] nvarchar(max)  NULL,
    [UserCreated] varchar(50)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserUpdated] varchar(50)  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'Calendars'
CREATE TABLE [dbo].[Calendars] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CourseId] int  NOT NULL,
    [CalendarDate] datetime  NOT NULL,
    [Status] int  NOT NULL,
    [Observation] nvarchar(max)  NULL,
    [UserCreated] varchar(50)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserUpdated] varchar(50)  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'Certifications'
CREATE TABLE [dbo].[Certifications] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [Classification] decimal(5,2)  NULL,
    [CertificationTypeId] int  NOT NULL,
    [Status] int  NOT NULL,
    [StudentId] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Observation] varchar(max)  NULL,
    [FormativeEntityId] int  NOT NULL,
    [UpdateDate] datetime  NULL,
    [UserCreated] varchar(50)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserUpdated] varchar(50)  NULL
);
GO

-- Creating table 'CertificationTypes'
CREATE TABLE [dbo].[CertificationTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [Code] varchar(10)  NOT NULL,
    [UserCreated] varchar(50)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserUpdated] varchar(50)  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'Companies'
CREATE TABLE [dbo].[Companies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [UserCreated] varchar(50)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserUpdated] varchar(50)  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'Courses'
CREATE TABLE [dbo].[Courses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(100)  NOT NULL,
    [LevelId] int  NOT NULL,
    [Code] varchar(10)  NOT NULL,
    [LocationId] int  NOT NULL,
    [FormativeEntityId] int  NOT NULL,
    [TeacherId] int  NOT NULL,
    [Observation] varchar(500)  NULL,
    [StartHour] time  NOT NULL,
    [Status] int  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndHour] time  NOT NULL,
    [CloseDate] datetime  NOT NULL,
    [InscriptionEmail] int  NOT NULL,
    [ConfirmationEmail] int  NOT NULL,
    [Reminder] int  NOT NULL,
    [Documentation] int  NOT NULL,
    [Intranet] int  NOT NULL,
    [Material] int  NOT NULL,
    [Dtp] int  NOT NULL,
    [Certificates] int  NOT NULL,
    [Avaliation] int  NOT NULL,
    [Confidential] int  NOT NULL,
    [UniqueReport] int  NOT NULL,
    [UserCreated] varchar(50)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserUpdated] varchar(50)  NULL,
    [UpdateDate] datetime  NULL,
    [CanceledObservation] varchar(500)  NULL,
    [Effort] nvarchar(max)  NOT NULL,
    [CourseTypeId] int  NOT NULL
);
GO

-- Creating table 'CourseLevels'
CREATE TABLE [dbo].[CourseLevels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [UpdateDate] datetime  NULL,
    [UserCreated] varchar(50)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserUpdated] varchar(50)  NULL
);
GO

-- Creating table 'CourseLocations'
CREATE TABLE [dbo].[CourseLocations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [DisplayColor] varchar(10)  NULL,
    [UserCreated] varchar(50)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserUpdated] varchar(50)  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'FormativeEntities'
CREATE TABLE [dbo].[FormativeEntities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [UserCreated] varchar(50)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserUpdated] varchar(50)  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'Inscriptions'
CREATE TABLE [dbo].[Inscriptions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentId] int  NOT NULL,
    [CourseId] int  NOT NULL,
    [TypeId] int  NOT NULL,
    [Status] int  NOT NULL,
    [Observation] varchar(500)  NULL,
    [UserCreated] varchar(50)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserUpdated] varchar(50)  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'InscriptionTypes'
CREATE TABLE [dbo].[InscriptionTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] varchar(50)  NOT NULL,
    [UserCreated] varchar(50)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserUpdated] varchar(50)  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'Logs'
CREATE TABLE [dbo].[Logs] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Type] varchar(20)  NOT NULL,
    [Priority] int  NOT NULL,
    [Category] varchar(50)  NOT NULL,
    [Title] varchar(200)  NOT NULL,
    [Message] varchar(max)  NOT NULL,
    [UserCreated] varchar(50)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserUpdated] varchar(50)  NULL,
    [UpdateDate] datetime  NULL
);
GO

-- Creating table 'Students'
CREATE TABLE [dbo].[Students] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [Email] varchar(50)  NOT NULL,
    [PhoneNumber] varchar(10)  NULL,
    [CompanyId] int  NOT NULL,
    [Status] int  NOT NULL,
    [UpdateDate] datetime  NULL,
    [UserCreated] varchar(50)  NULL,
    [CreatedDate] datetime  NULL,
    [UserUpdated] varchar(50)  NULL,
    [Observation] nvarchar(max)  NULL
);
GO

-- Creating table 'Teachers'
CREATE TABLE [dbo].[Teachers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [Email] varchar(50)  NULL,
    [PhoneNumber] varchar(10)  NULL,
    [CompanyId] int  NOT NULL,
    [Status] int  NOT NULL,
    [UserCreated] varchar(50)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UserUpdated] varchar(50)  NULL,
    [UpdateDate] datetime  NULL,
    [PayRoll] nvarchar(max)  NULL,
    [TecnicalSkills] nvarchar(max)  NULL
);
GO

-- Creating table 'CourseTypes'
CREATE TABLE [dbo].[CourseTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [UpdateDate] datetime  NOT NULL,
    [UserCreated] nvarchar(max)  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [UserUpdated] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [AspNetRoles_Id] nvarchar(128)  NOT NULL,
    [AspNetUsers_Id] nvarchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey], [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Attendances'
ALTER TABLE [dbo].[Attendances]
ADD CONSTRAINT [PK_Attendances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Calendars'
ALTER TABLE [dbo].[Calendars]
ADD CONSTRAINT [PK_Calendars]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Certifications'
ALTER TABLE [dbo].[Certifications]
ADD CONSTRAINT [PK_Certifications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CertificationTypes'
ALTER TABLE [dbo].[CertificationTypes]
ADD CONSTRAINT [PK_CertificationTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [PK_Companies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [PK_Courses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CourseLevels'
ALTER TABLE [dbo].[CourseLevels]
ADD CONSTRAINT [PK_CourseLevels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CourseLocations'
ALTER TABLE [dbo].[CourseLocations]
ADD CONSTRAINT [PK_CourseLocations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FormativeEntities'
ALTER TABLE [dbo].[FormativeEntities]
ADD CONSTRAINT [PK_FormativeEntities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Inscriptions'
ALTER TABLE [dbo].[Inscriptions]
ADD CONSTRAINT [PK_Inscriptions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InscriptionTypes'
ALTER TABLE [dbo].[InscriptionTypes]
ADD CONSTRAINT [PK_InscriptionTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Logs'
ALTER TABLE [dbo].[Logs]
ADD CONSTRAINT [PK_Logs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [PK_Students]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Teachers'
ALTER TABLE [dbo].[Teachers]
ADD CONSTRAINT [PK_Teachers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CourseTypes'
ALTER TABLE [dbo].[CourseTypes]
ADD CONSTRAINT [PK_CourseTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AspNetRoles_Id], [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([AspNetRoles_Id], [AspNetUsers_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [CalendarId] in table 'Attendances'
ALTER TABLE [dbo].[Attendances]
ADD CONSTRAINT [FK_Attendance_Calendar]
    FOREIGN KEY ([CalendarId])
    REFERENCES [dbo].[Calendars]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Attendance_Calendar'
CREATE INDEX [IX_FK_Attendance_Calendar]
ON [dbo].[Attendances]
    ([CalendarId]);
GO

-- Creating foreign key on [InscriptionId] in table 'Attendances'
ALTER TABLE [dbo].[Attendances]
ADD CONSTRAINT [FK_Attendance_Inscription]
    FOREIGN KEY ([InscriptionId])
    REFERENCES [dbo].[Inscriptions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Attendance_Inscription'
CREATE INDEX [IX_FK_Attendance_Inscription]
ON [dbo].[Attendances]
    ([InscriptionId]);
GO

-- Creating foreign key on [CourseId] in table 'Calendars'
ALTER TABLE [dbo].[Calendars]
ADD CONSTRAINT [FK_Calendar_Course]
    FOREIGN KEY ([CourseId])
    REFERENCES [dbo].[Courses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Calendar_Course'
CREATE INDEX [IX_FK_Calendar_Course]
ON [dbo].[Calendars]
    ([CourseId]);
GO

-- Creating foreign key on [CertificationTypeId] in table 'Certifications'
ALTER TABLE [dbo].[Certifications]
ADD CONSTRAINT [FK_Certification_CertificationType]
    FOREIGN KEY ([CertificationTypeId])
    REFERENCES [dbo].[CertificationTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Certification_CertificationType'
CREATE INDEX [IX_FK_Certification_CertificationType]
ON [dbo].[Certifications]
    ([CertificationTypeId]);
GO

-- Creating foreign key on [FormativeEntityId] in table 'Certifications'
ALTER TABLE [dbo].[Certifications]
ADD CONSTRAINT [FK_Certification_FormativeEntity]
    FOREIGN KEY ([FormativeEntityId])
    REFERENCES [dbo].[FormativeEntities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Certification_FormativeEntity'
CREATE INDEX [IX_FK_Certification_FormativeEntity]
ON [dbo].[Certifications]
    ([FormativeEntityId]);
GO

-- Creating foreign key on [StudentId] in table 'Certifications'
ALTER TABLE [dbo].[Certifications]
ADD CONSTRAINT [FK_Certification_Student]
    FOREIGN KEY ([StudentId])
    REFERENCES [dbo].[Students]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Certification_Student'
CREATE INDEX [IX_FK_Certification_Student]
ON [dbo].[Certifications]
    ([StudentId]);
GO

-- Creating foreign key on [CompanyId] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [FK_Student_Company]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Companies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Student_Company'
CREATE INDEX [IX_FK_Student_Company]
ON [dbo].[Students]
    ([CompanyId]);
GO

-- Creating foreign key on [CompanyId] in table 'Teachers'
ALTER TABLE [dbo].[Teachers]
ADD CONSTRAINT [FK_Teacher_Company]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Companies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Teacher_Company'
CREATE INDEX [IX_FK_Teacher_Company]
ON [dbo].[Teachers]
    ([CompanyId]);
GO

-- Creating foreign key on [LevelId] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [FK_Course_CourseLevel]
    FOREIGN KEY ([LevelId])
    REFERENCES [dbo].[CourseLevels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Course_CourseLevel'
CREATE INDEX [IX_FK_Course_CourseLevel]
ON [dbo].[Courses]
    ([LevelId]);
GO

-- Creating foreign key on [LocationId] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [FK_Course_CourseLocation]
    FOREIGN KEY ([LocationId])
    REFERENCES [dbo].[CourseLocations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Course_CourseLocation'
CREATE INDEX [IX_FK_Course_CourseLocation]
ON [dbo].[Courses]
    ([LocationId]);
GO

-- Creating foreign key on [FormativeEntityId] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [FK_Course_FormativeEntity]
    FOREIGN KEY ([FormativeEntityId])
    REFERENCES [dbo].[FormativeEntities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Course_FormativeEntity'
CREATE INDEX [IX_FK_Course_FormativeEntity]
ON [dbo].[Courses]
    ([FormativeEntityId]);
GO

-- Creating foreign key on [TeacherId] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [FK_Course_Teacher]
    FOREIGN KEY ([TeacherId])
    REFERENCES [dbo].[Teachers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Course_Teacher'
CREATE INDEX [IX_FK_Course_Teacher]
ON [dbo].[Courses]
    ([TeacherId]);
GO

-- Creating foreign key on [CourseId] in table 'Inscriptions'
ALTER TABLE [dbo].[Inscriptions]
ADD CONSTRAINT [FK_Inscription_Course]
    FOREIGN KEY ([CourseId])
    REFERENCES [dbo].[Courses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inscription_Course'
CREATE INDEX [IX_FK_Inscription_Course]
ON [dbo].[Inscriptions]
    ([CourseId]);
GO

-- Creating foreign key on [TypeId] in table 'Inscriptions'
ALTER TABLE [dbo].[Inscriptions]
ADD CONSTRAINT [FK_Inscription_InscriptionType]
    FOREIGN KEY ([TypeId])
    REFERENCES [dbo].[InscriptionTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inscription_InscriptionType'
CREATE INDEX [IX_FK_Inscription_InscriptionType]
ON [dbo].[Inscriptions]
    ([TypeId]);
GO

-- Creating foreign key on [StudentId] in table 'Inscriptions'
ALTER TABLE [dbo].[Inscriptions]
ADD CONSTRAINT [FK_Inscription_Student]
    FOREIGN KEY ([StudentId])
    REFERENCES [dbo].[Students]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Inscription_Student'
CREATE INDEX [IX_FK_Inscription_Student]
ON [dbo].[Inscriptions]
    ([StudentId]);
GO

-- Creating foreign key on [AspNetRoles_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRole]
    FOREIGN KEY ([AspNetRoles_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUser]
    FOREIGN KEY ([AspNetUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUser'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUser]
ON [dbo].[AspNetUserRoles]
    ([AspNetUsers_Id]);
GO

-- Creating foreign key on [CourseTypeId] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [FK_CourseTypeCourse]
    FOREIGN KEY ([CourseTypeId])
    REFERENCES [dbo].[CourseTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CourseTypeCourse'
CREATE INDEX [IX_FK_CourseTypeCourse]
ON [dbo].[Courses]
    ([CourseTypeId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------