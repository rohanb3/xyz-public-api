CREATE TABLE [TWC_BranchContactTypes] (
    [BranchContactTypeID] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NULL,
    CONSTRAINT [BranchContactTypeID] PRIMARY KEY ([BranchContactTypeID])
);

CREATE TABLE [TWC_Branches] (
    [BranchID] int NOT NULL IDENTITY,
    [BranchName] nvarchar(250) NOT NULL,
    [Email] nvarchar(50) NULL,
    [Phone] nvarchar(50) NULL,
    [Fax] nvarchar(50) NULL,
    [State] nvarchar(50) NULL,
    [City] nvarchar(50) NULL,
    [ZipCode] nvarchar(50) NULL,
    [AddressLine1] nvarchar(50) NULL,
    [AddressLine2] nvarchar(50) NULL,
    [GeoLat] nvarchar(50) NULL,
    [GeoLng] nvarchar(50) NULL,
    [IsEnabled] bit NULL DEFAULT 1,
    [Status] int NULL,
    [CreatedDate] datetime2 NULL,
    [ModifiedDate] datetime2 NULL,
    [CreatedBy] int NULL,
    [ModifiedBy] int NULL,
    [CompanyId] int NOT NULL,
    CONSTRAINT [BranchID] PRIMARY KEY ([BranchID]),
    CONSTRAINT [FK_TWC_Branches_TWC_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [TWC_Companies] ([CompanyID]) ON DELETE CASCADE
);

CREATE TABLE [TWC_BranchContacts] (
    [BranchContactID] int NOT NULL IDENTITY,
    [PersonName] nvarchar(50) NULL,
    [PersonLastName] nvarchar(50) NULL,
    [PersonTitle] nvarchar(100) NULL,
    [Value] nvarchar(100) NOT NULL,
    [CreatedDate] datetime2 NULL,
    [ModifiedDate] datetime2 NULL,
    [BranchContactTypeId] int NOT NULL,
    [BranchPrimaryContactId] int NOT NULL,
    CONSTRAINT [BranchContactID] PRIMARY KEY ([BranchContactID]),
    CONSTRAINT [FK_TWC_BranchContacts_TWC_BranchContactTypes_BranchContactTypeId] FOREIGN KEY ([BranchContactTypeId]) REFERENCES [TWC_BranchContactTypes] ([BranchContactTypeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_TWC_BranchContacts_TWC_Branches_BranchPrimaryContactId] FOREIGN KEY ([BranchPrimaryContactId]) REFERENCES [TWC_Branches] ([BranchID]) ON DELETE CASCADE
);
