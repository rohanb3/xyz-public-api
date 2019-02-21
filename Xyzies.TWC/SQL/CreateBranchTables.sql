CREATE TABLE [TWC_BranchContactType] (
    [BranchContactTypeID] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NULL,
    CONSTRAINT [PK_TWC_BranchContactType] PRIMARY KEY ([BranchContactTypeID])
);

CREATE TABLE [TWC_Branches] (
    [BranchID] int NOT NULL IDENTITY,
    [BranchName] nvarchar(250) NOT NULL,
    [Email] nvarchar(50) NOT NULL,
    [Phone] nvarchar(50) NULL,
    [Fax] nvarchar(50) NULL,
    [Address] nvarchar(50) NULL,
    [City] nvarchar(50) NULL,
    [ZipCode] nvarchar(50) NULL,
    [GeoLat] nvarchar(50) NULL,
    [GeoLon] nvarchar(50) NULL,
    [Status] int NULL,
    [State] nvarchar(50) NULL,
    [CreatedDate] datetime2 NULL,
    [ModifiedDate] datetime2 NULL,
    [CreatedBy] int NULL,
    [ModifiedBy] int NULL,
    [CompanyID] int NULL,
    CONSTRAINT [PK_TWC_Branches] PRIMARY KEY ([BranchID]),
    CONSTRAINT [FK_TWC_Branches_TWC_Companies_CompanyID] FOREIGN KEY ([CompanyID]) REFERENCES [TWC_Companies] ([CompanyID]) ON DELETE NO ACTION
);

CREATE TABLE [TWC_BranchContact] (
    [BranchContactID] int NOT NULL IDENTITY,
    [PersonName] nvarchar(50) NULL,
    [PersonLastName] nvarchar(50) NULL,
    [PersonTitle] nvarchar(100) NULL,
    [Value] nvarchar(250) NOT NULL,
    [BranchContactTypeId] int NOT NULL,
    [BranchId] int NULL,
    CONSTRAINT [PK_TWC_BranchContact] PRIMARY KEY ([BranchContactID]),
    CONSTRAINT [FK_TWC_BranchContact_TWC_BranchContactType_BranchContactTypeId] FOREIGN KEY ([BranchContactTypeId]) REFERENCES [TWC_BranchContactType] ([BranchContactTypeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_TWC_BranchContact_TWC_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [TWC_Branches] ([BranchID]) ON DELETE NO ACTION
);