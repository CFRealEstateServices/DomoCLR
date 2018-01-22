
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/18/17
-- Description:	Add User mapping
-- =============================================
CREATE PROCEDURE RemoveUserMapping
	-- Add the parameters for the stored procedure here
	@domain varchar(200),
	@DOMOUserId int,
	@ExternalUserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    delete m
	from dbo.UserRecordMappings m
	where
		m.domain = @domain
		and m.DOMOUserId = @DOMOUserId
		and m.ExternalUserId = @ExternalUserID
END