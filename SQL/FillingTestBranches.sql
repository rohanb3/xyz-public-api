insert into [TWC_Branch] ([Id],[BranchName],[Email],[Phone],[Fax],[State],[City],[ZipCode],[AddressLine1],[AddressLine2],[GeoLat],[GeoLng],[IsEnabled],[CreatedDate],[ModifiedDate],[CreatedBy],[ModifiedBy],[CompanyId])
SELECT newid(), [CompanyName] + ' #1',[Email],[Phone],[Fax],[State],[City],[ZipCode],[Address],[Address],[GeoLat],[GeoLon],[IsEnabled],'2019-04-17 23:44:34.370' as [CreatedDate],null as [ModifiedDate],[CreatedBy],[ModifiedBy],[CompanyID]
FROM [TWC04052019_TEMP_DUMP].[dbo].[TWC_Companies]
