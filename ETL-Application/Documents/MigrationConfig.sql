USE [HR]
GO
/****** Object:  Table [dbo].[MigrationConfig]    Script Date: 9/8/2022 10:00:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MigrationConfig](
	[Migration_ID] [int] IDENTITY(1,1) NOT NULL,
	[Source_Server] [nvarchar](100) NOT NULL,
	[Source_DB_Name] [nvarchar](50) NULL,
	[Source_Auth_Mode] [nvarchar](50) NOT NULL,
	[Source_DB_Username] [nvarchar](50) NULL,
	[Source_DB_Password] [nvarchar](50) NULL,
	[Source_Select_Query] [nvarchar](max) NULL,
	[Destination_Server] [nvarchar](50) NULL,
	[Destination_DB_Name] [nvarchar](50) NULL,
	[Destination_Auth_Mode] [nvarchar](50) NULL,
	[Destination_DB_Username] [nvarchar](50) NULL,
	[Destination_DB_Password] [nvarchar](50) NULL,
	[Destination_Table_Name] [nvarchar](max) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[MigrationConfig] ON 

INSERT [dbo].[MigrationConfig] ([Migration_ID], [Source_Server], [Source_DB_Name], [Source_Auth_Mode], [Source_DB_Username], [Source_DB_Password], [Source_Select_Query], [Destination_Server], [Destination_DB_Name], [Destination_Auth_Mode], [Destination_DB_Username], [Destination_DB_Password], [Destination_Table_Name], [IsActive]) VALUES (1, N'YOGESH-PC', N'HR', N'', N'test', N'test', N'select  ISNULL(emp.employee_id,'''') "employee_id",  ISNULL(emp.first_name,'''') "first_name",  ISNULL(emp.last_name,'''') "last_name",  ISNULL(emp.email,'''') "email",  ISNULL(emp.phone_number,'''') "phone_number",  ISNULL(emp.hire_date,'''') "hire_date",  ISNULL(job.job_title,'''') "job_title",  ISNULL(job.min_salary,0) "min_salary",  ISNULL(job.max_salary,0) "max_salary",  ISNULL(loc.street_address+'' ''+loc.city+'' ''+loc.state_province+'' ''+cou.country_name+'' PIN-''+loc.postal_code + '' [''+reg.region_name+'']'','''') "Address",  ISNULL(emp.salary,0) "Emp_Salary",  ISNULL(manager.first_name+'' ''+ manager.last_name,'''') "Manager_Name",  ISNULL(dep.department_name,'''') "department_name"  from [dbo].[employees] emp  join [dbo].[departments] dep on dep.department_id=emp.department_id  join [dbo].[jobs] job on job.job_id=emp.job_id  join [dbo].[locations] loc on loc.location_id=dep.location_id  join [dbo].[countries] cou on cou.country_id = loc.country_id  join [dbo].[regions] reg on reg.region_id=cou.region_id  left join [dbo].[employees] manager on emp.manager_id = manager.employee_id', N'YOGESH-PC', N'Archive', N'', N'test', N'test', N'[dbo].[Emp_Archive]', 1)
INSERT [dbo].[MigrationConfig] ([Migration_ID], [Source_Server], [Source_DB_Name], [Source_Auth_Mode], [Source_DB_Username], [Source_DB_Password], [Source_Select_Query], [Destination_Server], [Destination_DB_Name], [Destination_Auth_Mode], [Destination_DB_Username], [Destination_DB_Password], [Destination_Table_Name], [IsActive]) VALUES (2, N'YOGESH-PC', N'HR', N'Windows Auth', N'', N'', N'select  ISNULL(emp.employee_id,'''') "employee_id",  ISNULL(emp.first_name,'''') "first_name",  ISNULL(emp.last_name,'''') "last_name",  ISNULL(emp.email,'''') "email",  ISNULL(emp.phone_number,'''') "phone_number",  ISNULL(emp.hire_date,'''') "hire_date",  ISNULL(job.job_title,'''') "job_title",  ISNULL(job.min_salary,0) "min_salary",  ISNULL(job.max_salary,0) "max_salary",  ISNULL(loc.street_address+'' ''+loc.city+'' ''+loc.state_province+'' ''+cou.country_name+'' PIN-''+loc.postal_code + '' [''+reg.region_name+'']'','''') "Address",  ISNULL(emp.salary,0) "Emp_Salary",  ISNULL(manager.first_name+'' ''+ manager.last_name,'''') "Manager_Name",  ISNULL(dep.department_name,'''') "department_name"  from [dbo].[employees] emp  join [dbo].[departments] dep on dep.department_id=emp.department_id  join [dbo].[jobs] job on job.job_id=emp.job_id  join [dbo].[locations] loc on loc.location_id=dep.location_id  join [dbo].[countries] cou on cou.country_id = loc.country_id  join [dbo].[regions] reg on reg.region_id=cou.region_id  left join [dbo].[employees] manager on emp.manager_id = manager.employee_id', N'YOGESH-PC', N'Archive', N'Windows Auth', N'', N'', N'[dbo].[Emp_Archive]', 1)
SET IDENTITY_INSERT [dbo].[MigrationConfig] OFF
