

create view [dbo].[vw_ActualDomoGroupsWithFieldChecksum]
as
select
	i.*,
	CHECKSUM(isDefault,name) as FieldChecksum
from dbo.ActualDOMOGroups i