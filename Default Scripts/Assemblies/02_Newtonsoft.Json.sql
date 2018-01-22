/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if ((select count(0)
from sys.assembly_files f
where
	f.name like '%Newtonsoft.Json.dll') = 0)
begin
	CREATE ASSEMBLY [Newtonsoft_Json]
	FROM 'C:\Deployments\SQL CLR Assemblies\Newtonsoft.Json.dll'
	WITH PERMISSION_SET = UNSAFE;
end
else
begin
	begin try
		alter ASSEMBLY [Newtonsoft_Json]
		FROM 'C:\Deployments\SQL CLR Assemblies\Newtonsoft.Json.dll';
	end try
	begin catch
	end catch
end