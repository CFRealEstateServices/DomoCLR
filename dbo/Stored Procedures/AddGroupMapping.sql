
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/18/17
-- Description:	Add Group mapping
-- =============================================
CREATE PROCEDURE AddGroupMapping
	-- Add the parameters for the stored procedure here
	@domain varchar(200),
	@DOMOGroupId int,
	@ExternalGroupID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    insert into dbo.GroupRecordMappings
	select
		@domain,
		@DOMOGroupId,
		@ExternalGroupID
END