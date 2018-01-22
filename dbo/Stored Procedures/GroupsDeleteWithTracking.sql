
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/18/17
-- Description:	Delete a Group in DOMO and track the association between DOMO and external objects
-- =============================================
create PROCEDURE GroupsDeleteWithTracking
	@domain varchar(200),
	@DOMOGroupId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- remove any existing members from the group (since we can't delete the group til it has no members)
	begin
		select
			m.*,
			ROW_NUMBER() OVER (order by m.domain) as RowNum
		into #MembersToBeRemoved
		from dbo.ActualDOMOGroupMembership m
		where
			m.domain = @domain
			and m.groupId = @DOMOGroupId
			
		declare @CurRowNum int = 1
		declare @MaxRowNum int = (select isnull(max(RowNum), 0) from #MembersToBeRemoved)
		
		while (@CurRowNum <= @MaxRowNum)
		begin
			declare @UserIdToRemove int = (select m.userId from #MembersToBeRemoved m where m.RowNum = @CurRowNum)
			exec GroupsRemoveMember @domain, @DOMOGroupId, @UserIdToRemove
			
			set @CurRowNum = @CurRowNum + 1
		end

		drop table #MembersToBeRemoved
	end

	exec dbo.GroupsDelete @domain, @DOMOGroupId

	delete m
	from dbo.GroupRecordMappings m
	where
		m.domain = @domain
		and m.DOMOGroupId = @DOMOGroupId
END