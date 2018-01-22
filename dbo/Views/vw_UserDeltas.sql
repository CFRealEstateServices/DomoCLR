


CREATE view [dbo].[vw_UserDeltas]
as
select
	t.*
from (
		select
			c.domain,
			cast(null as int) as DOMOUserId,
			c.externalID as externalId,
			'insert' as neededAction
		from dbo.vw_IntendedDomoUsersWithFieldChecksum c
		left join dbo.UserRecordMappings m
			on m.domain = c.domain
			and m.ExternalUserId = c.externalID
		where
			m.domain is null

		union all
		select
			c.domain,
			c.id as DOMOUserId,
			cast(null as int) as externalId,
			'delete' as neededAction
		from dbo.vw_ActualDomoUsersWithFieldChecksum c
		left join dbo.UserRecordMappings m
			on m.domain = c.domain
			and m.DOMOUserId = c.id
		where
			m.domain is null
	
		union all
		select
			c.domain,
			c.id as DOMOUserId,
			m.ExternalUserId as externalId,
			(case when c.FieldChecksum = i.FieldChecksum then 'none' else 'update' end) as neededAction
		from dbo.vw_ActualDomoUsersWithFieldChecksum c
		join dbo.UserRecordMappings m
			on m.domain = c.domain
			and m.DOMOUserId = c.id
		join dbo.vw_IntendedDomoUsersWithFieldChecksum i
			on i.domain = c.domain
			and i.externalID = m.ExternalUserId
	) t
left join dbo.ActualDOMOUsers act
	on act.domain = t.domain
	and act.id = t.DOMOUserId
left join dbo.IntendedDOMOUsers intd
	on intd.domain = t.domain
	and intd.externalID = t.externalId
left join dbo.DOMOUserDomainsToBlockFromManagementViaAPI domainBlock
	on domainBlock.domain = t.domain
	and isnull(act.email, intd.email) like '%' + domainBlock.emailDomain
left join dbo.DOMOUsersToBlockFromManagementViaAPI userBlock
	on userBlock.domain = t.domain
	and isnull(act.email, intd.email) = userBlock.email
where
	domainBlock.domain is null
	and userBlock.domain is null
group by
	t.domain,
	t.DOMOUserId,
	t.externalId,
	t.neededAction