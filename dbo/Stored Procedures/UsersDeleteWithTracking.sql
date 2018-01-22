
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/18/17
-- Description:	Delete a User in DOMO and track the association between DOMO and external objects
-- =============================================
create PROCEDURE UsersDeleteWithTracking
	@domain varchar(200),
	@DOMOUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	exec dbo.UsersDelete @domain, @DOMOUserId

	delete m
	from dbo.UserRecordMappings m
	where
		m.domain = @domain
		and m.DOMOUserId = @DOMOUserId
END