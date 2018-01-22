
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/18/17
-- Description:	Try to import any obvious matches between existing and intended DOMO groups
-- =============================================
CREATE PROCEDURE [dbo].[TryImport_GroupMappings]
	@domain varchar(200)
AS
BEGIN
	exec dbo.Sync_DownloadGroups @domain, 0

	insert into dbo.GroupRecordMappings
	select
		@domain,
		a.id,
		i.externalID
	from dbo.ActualDOMOGroups a
	join dbo.IntendedDOMOGroups i
		on i.domain = @domain
		and i.[name] = a.[name]
	left join dbo.GroupRecordMappings m
		on m.domain = @domain
		and m.DOMOGroupId = a.id
		and m.externalGroupId = i.externalID
	where
		a.domain = @domain
		and m.domain is null
END