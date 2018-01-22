-- =============================================
-- Author:		Ryan Nigro
-- Create date: 3/16/17
-- Description:	Get default DOMO domain
-- =============================================
CREATE FUNCTION fn_GetDefaultDOMODomain
(
	
)
RETURNS varchar(200)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ret varchar(200)

	-- Add the T-SQL statements to compute the return value here
	SELECT @ret = DefaultDOMODomain from _DOMODefaultDomain

	-- Return the result of the function
	RETURN isnull(@ret, '')
END