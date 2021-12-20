
CREATE PROC tblToDoAddOrEdit

@ID int,
@Task varchar(50),
@DueDate Date,
@TStatus varchar(50)

	AS
	IF(@ID = 0)
		INSERT INTO tblToDo(Task,DueDate,TStatus) VALUES (@Task,@DueDate,@TStatus)

	ELSE

		Update tblToDo

		SET
				
		Task=@Task,
		DueDate=@DueDate,
		TStatus=@TStatus

		WHERE ID=@ID
