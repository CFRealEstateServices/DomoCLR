
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/18/17
-- Description:	Process a sync of intended users, groups, and group memberships to DOMO
-- =============================================
CREATE PROCEDURE IdentityManagement_SyncAll
	@domain varchar(200),
	@ProcessInserts bit,
	@ProcessUpdates bit,
	@ProcessDeletes bit,
	@SendInvitesToNewUsers bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    exec dbo.IdentityManagement_SyncUsersToDOMO @domain, @ProcessInserts, @ProcessUpdates, @ProcessDeletes, @SendInvitesToNewUsers
	exec dbo.IdentityManagement_SyncGroupsToDOMO @domain, @ProcessInserts, @ProcessUpdates, @ProcessDeletes
	exec dbo.IdentityManagement_SyncGroupMembershipsToDOMO @domain, @ProcessInserts, @ProcessDeletes
END