
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/18/17
-- Description:	Sync Intended Groups to DOMO
-- =============================================
CREATE PROCEDURE IdentityManagement_SyncGroupsToDOMO
	@domain varchar(200),
	@ProcessInserts bit,
	@ProcessUpdates bit,
	@ProcessDeletes bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	exec dbo.TryImport_GroupMappings @domain
	exec dbo.Sync_DownloadGroupMembership @domain, 0

	select 
		deltas.*,
		ROW_NUMBER() OVER (order by deltas.domain, g.isDefault) as RowNum
	into #orderedDeltas
	from dbo.vw_GroupDeltas deltas
	left join dbo.IntendedDOMOGroups g
		on g.externalID = deltas.externalId
	where
		deltas.domain = @domain
		and (
				(deltas.neededAction = 'insert' and @ProcessInserts = 1)
				OR (deltas.neededAction = 'update' and @ProcessUpdates = 1)
				OR (deltas.neededAction = 'delete' and @ProcessDeletes = 1)
			)

	declare @CurRowNum int = 1
	declare @MaxRowNum int = (select isnull(max(RowNum), 0) from #orderedDeltas)

	while (@CurRowNum <= @MaxRowNum)
	begin
		declare @neededAction varchar(10)
		declare @DOMOGroupId int
		declare @ExternalGroupID int
		declare @IsDefault bit
		declare @Name varchar(200)
		declare @IsActive bit = 1

		select
			@neededAction = d.neededAction,
			@DOMOGroupId = d.DOMOGroupId,
			@ExternalGroupID = d.externalId
		from #orderedDeltas d
		where
			d.RowNum = @CurRowNum

		if (@neededAction = 'insert')
		begin
			select
				@IsDefault = i.isDefault,
				@Name = i.[name]
			from dbo.IntendedDOMOGroups i
			where
				i.domain = @domain
				and i.externalID = @ExternalGroupID

			print 'creating Group with external Group id: ' + cast(@ExternalGroupId as varchar(20)) + ' and name: ' + @Name
			exec dbo.GroupsCreateWithTracking @domain, @ExternalGroupId
		end
		else if (@neededAction = 'update')
		begin
			select
				@IsDefault = i.isDefault,
				@Name = i.[name]
			from dbo.IntendedDOMOGroups i
			where
				i.domain = @domain
				and i.externalID = @ExternalGroupID
				
			print 'updating Group with external Group id: ' + cast(@ExternalGroupId as varchar(20)) + ' and name: ' + @Name
			exec dbo.GroupsUpdate @domain, @DOMOGroupId, @IsActive, @IsDefault, @Name
		end
		else if (@neededAction = 'delete')
		begin
			select
				@IsDefault = i.isDefault,
				@Name = i.[name]
			from dbo.ActualDOMOGroups i
			where
				i.domain = @domain
				and i.id = @DOMOGroupId

			print 'deleting Group with DOMO Group id: ' + cast(@DOMOGroupId as varchar(20)) + ' and name: ' + @Name
			exec dbo.GroupsDeleteWithTracking @domain, @DOMOGroupId
		end

		set @CurRowNum = @CurRowNum + 1
	end

	drop table #orderedDeltas
	exec dbo.Sync_DownloadGroups @domain, 0
	exec dbo.Sync_DownloadGroupMembership @domain, 0
END