create view vw_IntendedDomoUsersWithFieldChecksum
as
select
	i.*,
	CHECKSUM(email,role,name) as FieldChecksum
from dbo.IntendedDOMOUsers i