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
	f.name like '%System.Web.dll') = 0)
begin
	CREATE ASSEMBLY [System_Web]
	FROM 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Web.dll'
	WITH PERMISSION_SET = UNSAFE;
end
else
begin
	begin try
		alter ASSEMBLY [System_Web]
		FROM 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Web.dll';
	end try
	begin catch
	end catch
end