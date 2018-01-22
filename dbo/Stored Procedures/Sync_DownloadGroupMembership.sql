
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/16/17
-- Description:	Download a copy of the current DOMO group membership mappings
-- =============================================
CREATE PROCEDURE Sync_DownloadGroupMembership
	@domain varchar(200) = '' -- set a value if you don't want to use the default from the _DOMODefaultDomain table
	,@displayResult bit = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if (@domain = '')
		set @domain = dbo.fn_GetDefaultDOMODomain()

	CREATE TABLE #ret (
		[groupId] [int] NOT NULL,
		[userId] [int] NOT NULL,
	)

	declare @curGroupId int
	DECLARE group_cursor CURSOR FOR   
    SELECT id  
    FROM ActualDOMOGroups g  
    WHERE g.domain = @domain
  
    OPEN group_cursor  
    FETCH NEXT FROM group_cursor INTO @curGroupId       
  
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
		insert into #ret
		exec dbo.GroupsListMembers @domain, @curGroupId, 9999,0
  
		FETCH NEXT FROM group_cursor INTO @curGroupId  
    END  
  
    CLOSE group_cursor  
    DEALLOCATE group_cursor 

	delete t
	from dbo.ActualDOMOGroupMembership t
	where
		t.domain = @domain

	insert into dbo.ActualDOMOGroupMembership
	select
		@domain,
		t.*
	from #ret t
	
	if (@displayResult = 1)
	begin
		select *
		from #ret
	end
	
	drop table #ret
END