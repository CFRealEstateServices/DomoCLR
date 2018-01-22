
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/18/17
-- Description:	Create a Group in DOMO and track the association between DOMO and external objects
-- =============================================
create PROCEDURE GroupsCreateWithTracking
	@domain varchar(200),
	@externalGroupId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @IsDefault bit
	declare @Name varchar(200)

	select
		@IsDefault = u.isDefault,
		@Name = u.name
	from dbo.IntendedDOMOGroups u
	where
		u.domain = @domain
		and u.externalID = @externalGroupId

		
	CREATE TABLE #ret (
		[creatorId] [int] NOT NULL,
		[id] [int] NOT NULL,
		[active] [bit] NOT NULL,
		[isDefault] [bit] NOT NULL,
		[memberCount] [int] NOT NULL,
		[name] [varchar](200) NULL
	)

	insert into #ret
	exec dbo.GroupsCreate @domain, @IsDefault, @Name

	declare @DomoGroupId int
	select
		@DomoGroupId = r.id
	from #ret r

	insert into dbo.GroupRecordMappings
	select
		@domain,
		@DomoGroupId,
		@externalGroupId

	drop table #ret
END