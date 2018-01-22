create view [dbo].[vw_GroupMembershipDeltas]
as
select
	act.domain,
	act.groupId as DOMOGroupId,
	act.userId as DOMOUserId,
	(case when g.domain is not null then 'none' else 'delete' end) as neededAction
from dbo.ActualDOMOGroupMembership act
left join dbo.GroupRecordMappings gm
	on gm.domain = act.domain
	and gm.DOMOGroupId = act.groupId
left join dbo.UserRecordMappings um
	on um.domain = act.domain
	and um.DOMOUserId = act.userId
left join dbo.IntendedDOMOGroupMembership g
	on g.domain = act.domain
	and g.externalGroupId = gm.externalGroupId
	and g.externalUserId = um.ExternalUserId

union all
select
	g.domain,
	gm.DOMOGroupId as DOMOGroupId,
	um.DOMOUserId as DOMOUserId,
	'insert' as neededAction
from dbo.IntendedDOMOGroupMembership g
left join dbo.GroupRecordMappings gm
	on gm.domain = g.domain
	and gm.externalGroupId = g.externalGroupId
left join dbo.UserRecordMappings um
	on um.domain = g.domain
	and um.ExternalUserId = g.externalUserId
left join dbo.ActualDOMOGroupMembership act
	on act.domain = act.domain
	and act.groupId = gm.DOMOGroupId
	and act.userId = um.DOMOUserId
where
	act.domain is null


