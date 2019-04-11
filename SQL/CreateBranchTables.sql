CREATE TABLE [TWC_BranchContactType] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(128) NULL,
    CONSTRAINT [PK_TWC_BranchContactType] PRIMARY KEY ([Id])
);

CREATE TABLE [TWC_Branch] (
    [Id] uniqueidentifier NOT NULL,
    [BranchName] nvarchar(256) NOT NULL,
    [Email] nvarchar(128) NULL,
    [Phone] nvarchar(16) NULL,
    [Fax] nvarchar(16) NULL,
    [State] nvarchar(64) NULL,
    [City] nvarchar(64) NULL,
    [ZipCode] nvarchar(16) NULL,
    [AddressLine1] nvarchar(max) NULL,
    [AddressLine2] nvarchar(max) NULL,
    [GeoLat] nvarchar(32) NULL,
    [GeoLng] nvarchar(32) NULL,
    [IsEnabled] bit NOT NULL DEFAULT 1,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedDate] datetime2 NULL,
    [CreatedBy] int NULL,
    [ModifiedBy] int NULL,
    [CompanyId] int NOT NULL,
    CONSTRAINT [PK_TWC_Branch] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TWC_Branch_TWC_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [TWC_Companies] ([CompanyID]) ON DELETE CASCADE
);

CREATE TABLE [TWC_BranchContact] (
    [Id] uniqueidentifier NOT NULL,
    [PersonName] nvarchar(50) NULL,
    [PersonLastName] nvarchar(50) NULL,
    [PersonTitle] nvarchar(100) NULL,
    [Value] nvarchar(100) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [ModifiedDate] datetime2 NULL,
    [BranchContactTypeId] uniqueidentifier NOT NULL,
    [BranchId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_TWC_BranchContact] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TWC_BranchContact_TWC_BranchContactType_BranchContactTypeId] FOREIGN KEY ([BranchContactTypeId]) REFERENCES [TWC_BranchContactType] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TWC_BranchContact_TWC_Branch_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [TWC_Branch] ([Id]) ON DELETE CASCADE
);
