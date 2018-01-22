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


:r .\Database_Settings\01_Enable_CLR.sql

:r .\Assemblies\01_System.Runtime.Serialization.sql
:r .\Assemblies\02_Newtonsoft.Json.sql
:r .\Assemblies\03_System.Web.sql