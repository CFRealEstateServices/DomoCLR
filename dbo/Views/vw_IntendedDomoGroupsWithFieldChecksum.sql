
create view [dbo].[vw_IntendedDomoGroupsWithFieldChecksum]
as
select
	i.*,
	CHECKSUM(isDefault,name) as FieldChecksum
from dbo.IntendedDOMOGroups i