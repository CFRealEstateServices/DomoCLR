create view vw_ActualDomoUsersWithFieldChecksum
as
select
	i.*,
	CHECKSUM(email,role,name) as FieldChecksum
from dbo.ActualDOMOUsers i