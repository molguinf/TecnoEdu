IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [FullName] nvarchar(150) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [Categories] (
        [CategoryId] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(300) NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [Institutes] (
        [InstituteId] int NOT NULL IDENTITY,
        [Name] nvarchar(180) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Address] nvarchar(250) NOT NULL,
        [City] nvarchar(100) NOT NULL,
        [Phone] nvarchar(30) NOT NULL,
        [ContactEmail] nvarchar(150) NOT NULL,
        [Website] nvarchar(250) NULL,
        [LogoUrl] nvarchar(500) NULL,
        [VerificationStatus] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_Institutes] PRIMARY KEY ([InstituteId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [Courses] (
        [CourseId] int NOT NULL IDENTITY,
        [InstituteId] int NOT NULL,
        [CategoryId] int NOT NULL,
        [Title] nvarchar(180) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Requirements] nvarchar(max) NULL,
        [DurationHours] int NULL,
        [Modality] nvarchar(30) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Courses] PRIMARY KEY ([CourseId]),
        CONSTRAINT [FK_Courses_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([CategoryId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Courses_Institutes_InstituteId] FOREIGN KEY ([InstituteId]) REFERENCES [Institutes] ([InstituteId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [InstituteUsers] (
        [InstituteUserId] int NOT NULL IDENTITY,
        [InstituteId] int NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [InternalRole] nvarchar(40) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_InstituteUsers] PRIMARY KEY ([InstituteUserId]),
        CONSTRAINT [FK_InstituteUsers_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_InstituteUsers_Institutes_InstituteId] FOREIGN KEY ([InstituteId]) REFERENCES [Institutes] ([InstituteId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [CourseOfferings] (
        [OfferingId] int NOT NULL IDENTITY,
        [CourseId] int NOT NULL,
        [StartDate] date NOT NULL,
        [EndDate] date NULL,
        [ScheduleDescription] nvarchar(200) NOT NULL,
        [Price] decimal(12,2) NOT NULL,
        [Currency] nvarchar(3) NOT NULL,
        [TotalCapacity] int NOT NULL,
        [Status] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_CourseOfferings] PRIMARY KEY ([OfferingId]),
        CONSTRAINT [CK_CourseOffering_Capacity] CHECK ([TotalCapacity] > 0),
        CONSTRAINT [CK_CourseOffering_Dates] CHECK ([EndDate] IS NULL OR [EndDate] >= [StartDate]),
        CONSTRAINT [CK_CourseOffering_Price] CHECK ([Price] >= 0),
        CONSTRAINT [FK_CourseOfferings_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [Inquiries] (
        [InquiryId] int NOT NULL IDENTITY,
        [InstituteId] int NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [CourseId] int NULL,
        [Subject] nvarchar(200) NOT NULL,
        [Status] nvarchar(20) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Inquiries] PRIMARY KEY ([InquiryId]),
        CONSTRAINT [FK_Inquiries_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Inquiries_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Inquiries_Institutes_InstituteId] FOREIGN KEY ([InstituteId]) REFERENCES [Institutes] ([InstituteId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [Reservations] (
        [ReservationId] int NOT NULL IDENTITY,
        [OfferingId] int NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [RequestedAt] datetime2 NOT NULL,
        [Status] nvarchar(20) NOT NULL,
        [Notes] nvarchar(500) NULL,
        CONSTRAINT [PK_Reservations] PRIMARY KEY ([ReservationId]),
        CONSTRAINT [FK_Reservations_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Reservations_CourseOfferings_OfferingId] FOREIGN KEY ([OfferingId]) REFERENCES [CourseOfferings] ([OfferingId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE TABLE [InquiryMessages] (
        [MessageId] int NOT NULL IDENTITY,
        [InquiryId] int NOT NULL,
        [SenderUserId] nvarchar(450) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [SentAt] datetime2 NOT NULL,
        CONSTRAINT [PK_InquiryMessages] PRIMARY KEY ([MessageId]),
        CONSTRAINT [FK_InquiryMessages_AspNetUsers_SenderUserId] FOREIGN KEY ([SenderUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_InquiryMessages_Inquiries_InquiryId] FOREIGN KEY ([InquiryId]) REFERENCES [Inquiries] ([InquiryId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_Categories_Name] ON [Categories] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_CourseOfferings_CourseId] ON [CourseOfferings] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_Courses_CategoryId] ON [Courses] ([CategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_Courses_InstituteId] ON [Courses] ([InstituteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_Inquiries_CourseId] ON [Inquiries] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_Inquiries_InstituteId] ON [Inquiries] ([InstituteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_Inquiries_UserId] ON [Inquiries] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_InquiryMessages_InquiryId_SentAt] ON [InquiryMessages] ([InquiryId], [SentAt]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_InquiryMessages_SenderUserId] ON [InquiryMessages] ([SenderUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_Institutes_Name] ON [Institutes] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_InstituteUsers_InstituteId_UserId] ON [InstituteUsers] ([InstituteId], [UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_InstituteUsers_UserId] ON [InstituteUsers] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Reservations_OfferingId_UserId] ON [Reservations] ([OfferingId], [UserId]) WHERE [Status] = N''Confirmed''');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    CREATE INDEX [IX_Reservations_UserId] ON [Reservations] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261126035459_InitialEnglishSchema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261126035459_InitialEnglishSchema', N'10.0.0');
END;

COMMIT;
GO
