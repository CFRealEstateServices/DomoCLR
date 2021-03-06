DOMO CLR
================================
This is a library to call DOMO API methods directly via MS SQL Server.  It enables you to:

- Download data from DOMO into MS SQL Server
- Download users, groups, and user membership in groups from DOMO
- Sync a model of local users, groups, and user membership in groups from your MS SQL Server into DOMO (inserts, updates, and deletes are all supported)

System Requirements
----------------
- Microsoft SQL Server (tested on SQL 2016 Enterprise and SQL 2017 Enterprise, but would likely work on earlier versions as well)

Prerequisites
----------------

1. Create a SQL DB dedicated to DOMO CLR
2. Make the DB Trustworthy (`alter [db_name] set trustworthy on`).  NOTE: This has security implications.  Familiarize yourself with them before making this change or deploying assemblies as directed below.
3. Create default schema (under the `/Default Scripts/Tables` directory)
4. Copy `/External Libraries/JSONDotNet/Newtonsoft.Json.dll` to your DB server.  It's under `C:\Deployments\SQL CLR Assemblies\Newtonsoft.Json.dll` for me
5. Install the .NET 4.x redistributable on your SQL server if it's not already installed (https://www.microsoft.com/en-us/download/details.aspx?id=17718)
6. Enable CLR assemblies on your DOMO DB using: `\Default Scripts\Database Settings\01 - Enable CLR.sql`
7. Deploy the assemblies for Newtonsoft.Json.dll, System.Runtime.Serialization, and System.Web to your DOMO CLR DB using the scripts under `\Default Scripts\Assemblies`.  Change the path for any files as neccessary

Deployment
----------------
1. Build this solution in Visual Studio
2. Right click the `DOMO CLR` project and click Publish.
3. Pick your target database
4. Click 'Publish'


Usage
----------------
DomoCLR accesses DOMO APIs through access tokens and API clients.  To start out, you'll need to generate one.
1. Generate an access token (per https://knowledge.domo.com/Administer/Specifying_Security_Options/04Managing_Access_Tokens)
2. In SQL Server, insert your generated access token into DOMOCLR.dbo.\_DOMOAccessTokens
3. In SQL Server, insert your company's DOMO domain into DOMOCLR.dbo.\_DOMODefaultDomain (for example, contoso.domo.com)_
4. Generate an API client at https://developer.domo.com.  If you want to be able to download datasets, then make sure it has an Application Scope including "Data".  If you want to be able to manage users/groups/group membership, then make sure the API has Application Scope to "User".
5. In SQL Server, insert your API client ID, client secret, scope, and domo domain to DomoCLR.dbo.\_DOMOSettings 
   1. For example, if your client ID is XYZ, your client secret is ABC, you've scoped to both user and data, and your domain is contoso.domo.com, then you'd insert two rows into DOMOCLR.dbo._DOMOSettings:
   2. ['XYZ', 'ABC', 'data', 'contoso.domo.com'], ['XYZ', 'ABC', 'user', 'contoso.domo.com']

   
Downloading Data
----------------
The core sproc to download data is:  dbo.DownloadFromDOMO.  That said, to do anything with the data, you need to insert it into a database table (or temp table) that is prepopulated with the appropriate columns and data types to match your data set.  There is an example at dbo.Download_Sample_Data_Set.


Managing Users, Groups, and Group Membership
----------------
There are separate sprocs to manager users, groups, and user membership within groups.  Our use case for this was to be able to manage users automatically.  Meaning, we wanted to have a model of intended users, groups, and group members in our internal database and be able to sync that to DOMO directly (and automatically via SSIS or any other scheduler.)

- User Management sprocs: dbo.Users*
- Group Management sprocs: dbo.Groups*
- Group Membership Management sprocs: dbo.GroupsAddmember / dbo.GroupsRemoveMember

If you want to be able to automatically manage users, groups, and group membership, then look at the dbo.IdentityManagement_Sync* sprocs.  You can configure whether you'd like to process inserts, updates, and/or deletes for each sproc to sync users, groups, and group membership.  For example, you might want to add and update users via API but not delete them._

That said, as a prereq to using the identity management sprocs, you need to point the DOMOCLR database (or prepopulate it) with your intended, model users, groups, and group membership.  You need to load that data into:

- dbo.IntendedDOMOUsers
- dbo.IntendedDOMOGroups
- dbo.IntendedDOMOGroupMembership