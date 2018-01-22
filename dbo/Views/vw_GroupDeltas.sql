


CREATE view [dbo].[vw_GroupDeltas]
as
select
	t.*
from (
		select
			c.domain,
			cast(null as int) as DOMOGroupId,
			c.externalID as externalId,
			'insert' as neededAction
		from dbo.vw_IntendedDomoGroupsWithFieldChecksum c
		left join dbo.GroupRecordMappings m
			on m.domain = c.domain
			and m.externalGroupId = c.externalID
		where
			m.domain is null

		union all
		select
			c.domain,
			c.id as DOMOGroupId,
			cast(null as int) as externalId,
			'delete' as neededAction
		from dbo.vw_ActualDomoGroupsWithFieldChecksum c
		left join dbo.GroupRecordMappings m
			on m.domain = c.domain
			and m.DOMOGroupId = c.id
		where
			m.domain is null
	
		union all
		select
			c.domain,
			c.id as DOMOGroupId,
			m.externalGroupId as externalId,
			(case when c.FieldChecksum = i.FieldChecksum then 'none' else 'update' end) as neededAction
		from dbo.vw_ActualDomoGroupsWithFieldChecksum c
		join dbo.GroupRecordMappings m
			on m.domain = c.domain
			and m.DOMOGroupId = c.id
		join dbo.vw_IntendedDomoGroupsWithFieldChecksum i
			on i.domain = c.domain
			and i.externalID = m.externalGroupId
	) t
--left join dbo.ActualDOMOGroups act
--	on act.domain = t.domain
--	and act.id = t.DOMOGroupId
--left join dbo.IntendedDOMOGroups intd
--	on intd.domain = t.domain
--	and intd.externalID = t.externalId
--left join dbo.DOMOGroupDomainsToBlockFromManagementViaAPI domainBlock
--	on domainBlock.domain = t.domain
--	and isnull(act.email, intd.email) like '%' + domainBlock.emailDomain
--left join dbo.DOMOGroupsToBlockFromManagementViaAPI GroupBlock
--	on GroupBlock.domain = t.domain
--	and isnull(act.email, intd.email) = GroupBlock.email
--where
--	domainBlock.domain is null
--	and GroupBlock.domain is null
group by
	t.domain,
	t.DOMOGroupId,
	t.externalId,
	t.neededAction