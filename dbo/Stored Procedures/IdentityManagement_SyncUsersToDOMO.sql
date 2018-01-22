
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/18/17
-- Description:	Sync Intended Users to DOMO
-- =============================================
CREATE PROCEDURE [dbo].[IdentityManagement_SyncUsersToDOMO]
	@domain varchar(200),
	@ProcessInserts bit,
	@ProcessUpdates bit,
	@ProcessDeletes bit,
	@sendInviteToNewUsers bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	exec dbo.TryImport_UserMappings @domain

	select 
		*,
		ROW_NUMBER() OVER (order by deltas.domain) as RowNum
	into #orderedDeltas
	from dbo.vw_UserDeltas deltas
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
		declare @DOMOUserId int
		declare @ExternalUserID int
		declare @Email varchar(200)
		declare @Role varchar(10)
		declare @Name varchar(200)

		select
			@neededAction = d.neededAction,
			@DOMOUserId = d.DOMOUserId,
			@ExternalUserID = d.externalId
		from #orderedDeltas d
		where
			d.RowNum = @CurRowNum

		if (@neededAction = 'insert')
		begin
			select
				@Email = i.email,
				@Role = i.[role],
				@Name = i.[name]
			from dbo.IntendedDOMOUsers i
			where
				i.domain = @domain
				and i.externalID = @ExternalUserID

			print 'creating user with external user id: ' + cast(@ExternalUserId as varchar(20)) + ' and email: ' + @Email
			exec dbo.UsersCreateWithTracking @domain, @ExternalUserId, @sendInviteToNewUsers
		end
		else if (@neededAction = 'update')
		begin
			select
				@Email = i.email,
				@Role = i.[role],
				@Name = i.[name]
			from dbo.IntendedDOMOUsers i
			where
				i.domain = @domain
				and i.externalID = @ExternalUserID
				
			print 'updating user with external user id: ' + cast(@ExternalUserId as varchar(20)) + ' and email: ' + @Email
			exec dbo.UsersUpdate @domain, @DOMOUserId, @Email, @Role, @Name
		end
		else if (@neededAction = 'delete')
		begin
			select
				@Email = i.email,
				@Role = i.[role],
				@Name = i.[name]
			from dbo.ActualDOMOUsers i
			where
				i.domain = @domain
				and i.id = @DOMOUserId

			print 'deleting user with DOMO user id: ' + cast(@DOMOUserId as varchar(20)) + ' and email: ' + @Email
			exec dbo.UsersDeleteWithTracking @domain, @DOMOUserId
		end

		set @CurRowNum = @CurRowNum + 1
	end

	drop table #orderedDeltas
	exec dbo.Sync_DownloadUsers @domain, 0
END