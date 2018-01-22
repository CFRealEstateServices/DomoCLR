
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/18/17
-- Description:	Try to import any obvious matches between existing and intended DOMO users
-- =============================================
CREATE PROCEDURE [dbo].[TryImport_UserMappings]
	@domain varchar(200)
AS
BEGIN
	exec dbo.Sync_DownloadUsers @domain, 0

	insert into dbo.UserRecordMappings
	select
		@domain,
		a.id,
		i.externalID
	from dbo.ActualDOMOUsers a
	join dbo.IntendedDOMOUsers i
		on i.domain = @domain
		and i.email = a.email
	left join dbo.UserRecordMappings m
		on m.domain = @domain
		and m.DOMOUserId = a.id
		and m.ExternalUserId = i.externalID
	where
		a.domain = @domain
		and m.domain is null
END