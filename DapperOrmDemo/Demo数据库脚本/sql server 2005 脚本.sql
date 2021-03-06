SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Department](
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentName] [nchar](20) NOT NULL,
	[Description] [nchar](255) NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Employee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[Gender] [bit] NOT NULL,
	[Birthday] [datetime] NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [View_EmpDepName]
AS
SELECT     TOP (100) PERCENT dbo.Department.DepartmentName AS Expr1, dbo.Department.Description, dbo.Employee.*
FROM         dbo.Department INNER JOIN
                      dbo.Employee ON dbo.Department.DepartmentId = dbo.Employee.DepartmentId

GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Department] FOREIGN KEY([DepartmentId])
REFERENCES [Department] ([DepartmentId])
