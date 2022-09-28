CREATE TABLE [dbo].[tblToDo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Task] [varchar](50) NULL,
	[DueDate] [datetime] NULL,
	[TStatus] [varchar](50) NULL,
	[AssignedTo] [varchar](50) NULL,
	[Description] [varchar](500) NULL,
	[Name] [varchar](500) NULL,
	[TeamName] [varchar](500) NULL,
	[StoryPoints] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
