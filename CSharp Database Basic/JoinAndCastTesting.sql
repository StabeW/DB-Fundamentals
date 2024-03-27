SELECT *
	FROM
	(SELECT 
		CONCAT_WS(' ', LastName, FirstName) AS [Full Name],
		Departments.[Name] AS [Departments Name],
		HireDate,
		AVG(Salary) AS [Average Salary],
		Projects.[Name] AS [Project Name],
		Projects.[StartDate] AS [Project StartDate],
		ISNULL(CONVERT(NVARCHAR, Projects.[EndDate] , 120), 'Progressing') AS [Project EndDate],
		ISNULL(CAST(DATEDIFF(MONTH, Projects.StartDate, Projects.[EndDate]) AS NVARCHAR), 'Progressing') AS [Complate Project Time]
		FROM Employees
	JOIN Departments
	ON Employees.DepartmentID = Departments.DepartmentID
	JOIN Projects
	ON Employees.EmployeeID = Projects.ProjectID
	GROUP BY 
		 [LastName], 
		 [FirstName], 
		 Departments.[Name], 
		 [HireDate], 
		 [Salary], 
		 Projects.[Name], 
		 Projects.[StartDate],
		 Projects.[EndDate]) AS [EMP]



