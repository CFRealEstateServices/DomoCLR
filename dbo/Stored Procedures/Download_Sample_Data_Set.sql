
-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/15/17
-- Description:	Download a sample dataset from DOMO
-- =============================================
CREATE PROCEDURE [dbo].[Download_Sample_Data_Set] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- wrapper table definition so that we could export the data somewhere.  For this one, we just threw the results in a temp table
	create table #t (
		firstname varchar(100),
		lastname varchar(200),
		username varchar(200),
		usertype varchar(50),
		zerotothirtylogins int,
		thirtytosixtylogins int,
		sixtytoninetylogins int,
		totallogins int,
	)

	insert into #t
	exec DownloadFromDOMO 'CLIENT_SUBDOMAIN_HERE.domo.com', 'ddd2aa71-a9b4-45d3-a6a1-3675289462fb' -- this is the dataset ID in DOMO. https://CLIENT_SUBDOMAIN_HERE.domo.com/datasources/ddd2aa71-a9b4-45d3-a6a1-3675289462fb/details/overview

	select *
	from #t

	drop table #t
END