
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/18/17
-- Description:	Create a user in DOMO and track the association between DOMO and external objects
-- =============================================
CREATE PROCEDURE UsersCreateWithTracking
	@domain varchar(200),
	@externalUserId int,
	@sendInvite bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @Email varchar(200)
	declare @Role varchar(20)
	declare @Name varchar(200)

	select
		@Email = u.email,
		@Role = u.role,
		@Name = u.name
	from dbo.IntendedDOMOUsers u
	where
		u.domain = @domain
		and u.externalID = @externalUserId

		
	CREATE TABLE #ret (
		[id] [int] NOT NULL,
		[title] [varchar](200) NULL,
		[email] [varchar](200) NOT NULL,
		[role] [varchar](20) NOT NULL,
		[name] [varchar](200) NOT NULL,
		[createdAt] [datetime] NOT NULL,
		[UpdatedAt] [datetime] NOT NULL,
		[image] [varchar](300) NOT NULL
	)

	insert into #ret
	exec dbo.UsersCreate @domain, @Email, @Role, @Name, @sendInvite

	declare @DomoUserId int
	select
		@DomoUserId = r.id
	from #ret r

	insert into dbo.UserRecordMappings
	select
		@domain,
		@DomoUserId,
		@externalUserId

	drop table #ret
END