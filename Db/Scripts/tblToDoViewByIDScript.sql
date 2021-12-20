CREATE PROC tblToDoViewByID

@ID int

AS

SELECT * FROM tblToDo where ID=@id