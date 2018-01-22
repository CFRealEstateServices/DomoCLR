
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/18/17
-- Description:	Sync Intended Groups to DOMO
-- =============================================
create PROCEDURE IdentityManagement_SyncGroupMembershipsToDOMO
	@domain varchar(200),
	@ProcessInserts bit,
	@ProcessDeletes bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	exec dbo.TryImport_GroupMappings @domain
	exec dbo.Sync_DownloadGroupMembership @domain, 0

	select 
		*,
		ROW_NUMBER() OVER (order by deltas.domain) as RowNum
	into #orderedDeltas
	from dbo.vw_GroupMembershipDeltas deltas
	where
		deltas.domain = @domain
		and (
				(deltas.neededAction = 'insert' and @ProcessInserts = 1)
				OR (deltas.neededAction = 'delete' and @ProcessDeletes = 1)
			)

	declare @CurRowNum int = 1
	declare @MaxRowNum int = (select isnull(max(RowNum), 0) from #orderedDeltas)

	while (@CurRowNum <= @MaxRowNum)
	begin
		declare @neededAction varchar(10)
		declare @DOMOGroupId int
		declare @DOMOUserId int

		select
			@neededAction = d.neededAction,
			@DOMOGroupId = d.DOMOGroupId,
			@DOMOUserId = d.DOMOUserId
		from #orderedDeltas d
		where
			d.RowNum = @CurRowNum

		if (@neededAction = 'insert')
		begin
			print 'creating Group membership with DOMO Group id: ' + cast(@DOMOGroupId as varchar(20)) + ' and DOMO User ID: ' + cast(@DOMOUserId as varchar(20))
			exec dbo.GroupsAddMember @domain, @DOMOGroupId, @DOMOUserId
		end
		else if (@neededAction = 'delete')
		begin
			print 'deleting Group membership with DOMO Group id: ' + cast(@DOMOGroupId as varchar(20)) + ' and DOMO User ID: ' + cast(@DOMOUserId as varchar(20))
			exec dbo.GroupsRemoveMember @domain, @DOMOGroupId, @DOMOUserId
		end

		set @CurRowNum = @CurRowNum + 1
	end

	drop table #orderedDeltas
	exec dbo.Sync_DownloadGroupMembership @domain, 0
END