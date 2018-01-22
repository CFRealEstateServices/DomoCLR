
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/16/17
-- Description:	Download a copy of the current DOMO user list
-- =============================================
CREATE PROCEDURE [dbo].[Sync_DownloadUsers]
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
	exec dbo.UsersList @domain, 9999,0

	delete t
	from dbo.ActualDOMOUsers t
	where
		t.domain = @domain

	insert into dbo.ActualDOMOUsers
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