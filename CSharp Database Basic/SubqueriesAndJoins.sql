---Problem 1. Employee Address

USE SoftUni

SELECT TOP(5) EmployeeID, JobTitle, e.AddressID, AddressText 
	FROM Employees AS e
JOIN Addresses AS a ON e.AddressID = a.AddressID
ORDER BY AddressID

---Problem 2. Addresses with Towns

SELECT TOP(50) FirstName, LastName, t.[Name], AddressText 
	FROM Employees AS e
JOIN Addresses AS a ON e.AddressID = a.AddressID
JOIN Towns AS t ON a.TownID = t.TownID
ORDER BY FirstName, LastName

---Problem 3. Sales Employee

SELECT EmployeeID, FirstName, LastName, d.[Name] 
	FROM Employees AS e
JOIN Departments AS d ON e.DepartmentID = d.DepartmentID 
AND d.[Name] = 'Sales'
ORDER BY EmployeeID

---Problem 4. Employee Departments
 SELECT TOP(5) EmployeeID, FirstName, Salary, d.[Name]
	FROM Employees AS e
JOIN Departments AS d ON e.DepartmentID = d.DepartmentID 
AND Salary > 15000
ORDER BY d.DepartmentID

---Problem 5. Employees Without Project

SELECT TOP(3) e.EmployeeID, FirstName 
	FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID 
WHERE ProjectID IS NULL
ORDER BY e.EmployeeID

---Problem 6. Employees Hired After

SELECT FirstName, LastName, HireDate, d.[Name] 
	FROM Employees AS e
JOIN Departments AS d ON e.DepartmentID = d.DepartmentID 
AND HireDate > '1999-01-01' 
AND d.[Name] IN ('Sales', 'Finance')
ORDER BY HireDate

---Problem 7. Employees with Project

SELECT TOP(5) e.EmployeeID, FirstName, p.[Name]
	FROM Employees AS e
JOIN EmployeesProjects AS ep ON ep.EmployeeID = e.EmployeeID
	JOIN Projects AS p ON p.ProjectID = ep.ProjectID
WHERE P.EndDate IS NULL
ORDER BY EmployeeID
	
---Problem 8. Employee 24

SELECT TOP(5) e.EmployeeID, FirstName, 
	CASE
	    WHEN p.StartDate > '01-01-2005' 
			THEN NULL
		WHEN p.StartDate < '01-01-2005' 
			THEN p.[Name]
		END AS ProjectName
	FROM Employees AS e
JOIN EmployeesProjects AS ep ON ep.EmployeeID = e.EmployeeID
	JOIN Projects AS p ON p.ProjectID = ep.ProjectID
WHERE e.EmployeeID = 24

---Problem 9. Employee Manager

SELECT e.EmployeeID, e.FirstName, m.EmployeeID, m.FirstName
	FROM Employees AS e
	JOIN Employees AS m ON m.EmployeeID = e.ManagerID
	WHERE e.ManagerID IN (3, 7)
	ORDER BY e.EmployeeID

---Problem 10. Employee Summary

SELECT TOP(50)
	e.EmployeeID,
	CONCAT_WS(' ', e.FirstName, e.LastName) AS [EmployeeName], 
	CONCAT_WS(' ', m.FirstName, m.LastName) AS [ManagerName],
	d.[Name] AS [DepartmentName]
	FROM Employees AS e
	JOIN Employees AS m ON m.EmployeeID = e.ManagerID
	JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
	ORDER BY e.EmployeeID

---Problem 11. Min Average Salary

SELECT TOP(1) AVG(Salary) AS [MinAverageSalary]
	FROM Employees
	GROUP BY DepartmentID
	ORDER BY AVG(Salary)

---Problem 12. Highest Peaks in Bulgaria

USE Geography

SELECT c.CountryCode, m.MountainRange, PeakName, Elevation
	FROM Countries AS c
JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
JOIN Mountains AS m ON m.Id = mc.MountainId
JOIN Peaks AS p ON m.Id = p.MountainId
WHERE Elevation > 2835
AND c.CountryCode = 'BG'
ORDER BY Elevation DESC

---Problem 13. Count Mountain Ranges

SELECT c.CountryCode, COUNT(m.MountainRange) AS [MountainRange]
	FROM Countries AS c
JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
JOIN Mountains AS m ON m.Id = mc.MountainId
WHERE c.CountryCode IN ('BG', 'RU', 'US')
GROUP BY c.CountryCode

---Problem 14. Countries with Rivers

SELECT TOP(5) c.CountryName, r.RiverName 
	FROM Countries AS c
LEFT JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
WHERE c.ContinentCode = 'AF'
ORDER BY c.CountryName 

---Problem 15. *Continents and Currencies

SELECT * FROM Continents
SELECT * FROM Currencies

---Problem 16. Countries without any Mountains

SELECT COUNT(c.CountryCode) AS [CountryCode] 
	FROM countries AS c
LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
WHERE mc.MountainId IS NULL

---Problem 17. Highest Peak and Longest River by Country

SELECT c.CountryName, MAX(p.Elevation) AS [HighestPeakElevation], MAX(r.[Length]) AS [LongestRiverLength]
	FROM Countries AS c
JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
JOIN Rivers AS r ON r.Id = cr.RiverId
JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
JOIN Mountains AS m ON m.Id = mc.MountainId
JOIN Peaks AS p ON p.MountainId = m.Id
GROUP BY c.CountryName
ORDER BY HighestPeakElevation DESC, LongestRiverLength DESC

---Problem 18. * Highest Peak Name and Elevation by Country

SELECT TOP(5) c.CountryName AS [Country],
	   ISNULL(p.PeakName, '(no highest peak)') AS [Highest Peak Name],
	   ISNULL(MAX(p.Elevation), 0) AS [Highest Peak Elevation],
	   ISNULL(m.MountainRange, '(no mountain)') AS [Mountain]
	FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
LEFT JOIN Mountains AS m ON m.Id = mc.MountainId
LEFT JOIN Peaks AS p ON p.MountainId = m.Id
	GROUP BY c.CountryName, p.PeakName, m.MountainRange
	ORDER BY c.CountryName, p.PeakName