CREATE TABLE [TWC_BranchContactTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NULL,
    CONSTRAINT [BranchContactTypeID] PRIMARY KEY ([Id])
);

CREATE TABLE [TWC_Branches] (
    [Id] int NOT NULL IDENTITY,
    [BranchName] nvarchar(250) NOT NULL,
    [Email] nvarchar(50) NULL,
    [Phone] nvarchar(50) NULL,
    [Fax] nvarchar(50) NULL,
    [Address] nvarchar(50) NULL,
    [City] nvarchar(50) NULL,
    [ZipCode] nvarchar(50) NULL,
    [GeoLat] nvarchar(50) NULL,
    [GeoLon] nvarchar(50) NULL,
    [Status] int NULL,
    [State] nvarchar(50) NULL,
    [CreatedDate] AS GETUTCDATE(),
    [ModifiedDate] AS GETUTCDATE(),
    [CreatedBy] int NULL,
    [ModifiedBy] int NULL,
    [ParentCompanyId] int NOT NULL,
    CONSTRAINT [BranchID] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TWC_Branches_TWC_Companies_ParentCompanyId] FOREIGN KEY ([ParentCompanyId]) REFERENCES [TWC_Companies] ([CompanyID]) ON DELETE CASCADE
);

CREATE TABLE [TWC_BranchContacts] (
    [Id] int NOT NULL IDENTITY,
    [PersonName] nvarchar(50) NULL,
    [PersonLastName] nvarchar(50) NULL,
    [PersonTitle] nvarchar(100) NULL,
    [Value] nvarchar(100) NOT NULL,
    [CreatedDate] AS GETUTCDATE(),
    [ModifiedDate] AS GETUTCDATE(),
    [BranchContactTypeId] int NOT NULL,
    [BranchPrimaryContactId] int NOT NULL,
    CONSTRAINT [BranchContactID] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TWC_BranchContacts_TWC_BranchContactTypes_BranchContactTypeId] FOREIGN KEY ([BranchContactTypeId]) REFERENCES [TWC_BranchContactTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TWC_BranchContacts_TWC_Branches_BranchPrimaryContactId] FOREIGN KEY ([BranchPrimaryContactId]) REFERENCES [TWC_Branches] ([Id]) ON DELETE CASCADE
);