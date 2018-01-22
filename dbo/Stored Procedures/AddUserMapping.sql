
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/18/17
-- Description:	Add user mapping
-- =============================================
CREATE PROCEDURE AddUserMapping
	-- Add the parameters for the stored procedure here
	@domain varchar(200),
	@DOMOUserId int,
	@ExternalUserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    insert into dbo.UserRecordMappings
	select
		@domain,
		@DOMOUserId,
		@ExternalUserID
END