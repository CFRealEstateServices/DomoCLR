
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/16/17
-- Description:	Download a copy of the current DOMO group list
-- =============================================
CREATE PROCEDURE Sync_DownloadGroups
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
		[creatorId] [int] NOT NULL,
		[id] [int] NOT NULL,
		[active] [bit] NOT NULL,
		[isDefault] [bit] NOT NULL,
		[memberCount] [int] NOT NULL,
		[name] [varchar](200) NULL
	)

    insert into #ret
	exec dbo.GroupsList @domain, 9999,0

	delete t
	from dbo.ActualDOMOGroups t
	where
		t.domain = @domain

	insert into dbo.ActualDOMOGroups
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