
CREATE PROC tblToDoDeleteByID

@ID int

AS

SELECT * FROM tblToDo where ID=@id