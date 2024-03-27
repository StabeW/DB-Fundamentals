USE Gringotts

---Problem 1. Records’ Count

SELECT COUNT(*) AS [Count]
	FROM WizzardDeposits

---Problem 2. Longest Magic Wand

SELECT MAX(MagicWandSize) AS [LongestMagicWand]
	FROM WizzardDeposits

---Problem 3. Longest Magic Wand per Deposit Groups

SELECT DepositGroup, MAX(MagicWandSize) AS [LongestMagicWand]
	FROM WizzardDeposits
GROUP BY DepositGroup

---Problem 4. * Smallest Deposit Group per Magic Wand Size

SELECT TOP(2) DepositGroup
	FROM WizzardDeposits
GROUP BY DepositGroup
ORDER BY AVG(MagicWandSize)

---Problem 5. Deposits Sum

SELECT DepositGroup, SUM(DepositAmount) AS [TotalSum]
	FROM WizzardDeposits
GROUP BY DepositGroup

---Problem 6. Deposits Sum for Ollivander Family

SELECT  DepositGroup, SUM(DepositAmount) AS [TotalSum]
	FROM WizzardDeposits
	WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup

---Problem 7. Deposits Filter

SELECT  DepositGroup, SUM(DepositAmount) AS [TotalSum]
	FROM WizzardDeposits
	WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
ORDER BY TotalSum DESC

---Problem 8. Deposit Charge

SELECT DepositGroup, MagicWandCreator, MIN(DepositCharge)  AS [MinDepositCharge]
	FROM WizzardDeposits
GROUP BY MagicWandCreator ,DepositGroup
ORDER BY MagicWandCreator, DepositGroup

---Problem 9. Age Groups

SELECT A.AgeGroup, COUNT(*) AS [WizardCount]
FROM
	(SELECT 
		CASE 
			WHEN Age BETWEEN 0 AND 10 THEN '[0-10]'
			WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
			WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
			WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
			WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
			WHEN Age BETWEEN 51 AND 60 THEN '[41-60]'
			ELSE '[61+]'
			END AS [AgeGroup]
		FROM WizzardDeposits) AS [a]
GROUP BY a.AgeGroup

---Problem 10. First Letter

SELECT LEFT(FirstName, 1) AS [FirstLetter]
	FROM WizzardDeposits
	WHERE DepositGroup = 'Troll Chest' 
	GROUP BY LEFT(FirstName, 1)

---Problem 11. Average Interest

SELECT DepositGroup, IsDepositExpired, AVG(DepositInterest) AS [AverageInterest]
	FROM WizzardDeposits
	WHERE DepositStartDate > '01/01/1985'
	GROUP BY DepositGroup, IsDepositExpired
	ORDER BY DepositGroup DESC, IsDepositExpired

---Problem 12. * Rich Wizard, Poor Wizard
SELECT SUM(Sum.SumDifference) AS [SumDifference]
	FROM
	(SELECT DepositAmount - LEAD(DepositAmount) 
		OVER (ORDER BY Id) AS [SumDifference]
	FROM WizzardDeposits) AS Sum

USE SoftUni

---Problem 13. Departments Total Salaries

SELECT DepartmentID, SUM(Salary) AS [TotalSalary]
	FROM Employees
	GROUP BY DepartmentID

---Problem 14. Employees Minimum Salaries

SELECT DepartmentID, MIN(Salary) AS [MinimumSalary]
	FROM Employees
	WHERE DepartmentID IN (2, 5, 7) 
	AND HireDate > '01/01/2000'
	GROUP BY DepartmentID

---Problem 15. Employees Average Salaries

SELECT * INTO NewTable
	FROM Employees	
	WHERE Salary > 30000

DELETE FROM NewTable
WHERE ManagerID = 42

UPDATE NewTable
SET Salary += 5000
WHERE DepartmentID = 1

SELECT DepartmentID, AVG(Salary) AS [AverageSalary]
	FROM NewTable
	GROUP BY DepartmentID

---Problem 16. Employees Maximum Salaries

SELECT DepartmentID, MAX(Salary) AS [MaxSalary]
	FROM Employees
	GROUP BY DepartmentID
	HAVING MAX(Salary) NOT BETWEEN 30000 AND 70000

SELECT COUNT(Salary) AS [Count]
	FROM Employees
	GROUP BY ManagerID
	HAVING ManagerID IS NULL

---Problem 18. *3rd Highest Salary

SELECT DISTINCT rs.DepartmentID, rs.Salary
	FROM
		(SELECT DepartmentID, Salary,
			DENSE_RANK() OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS RankSalary
		FROM Employees) AS [rs]
		WHERE RankSalary = 3

---Problem 19. **Salary Challenge

SELECT TOP(10) FirstName, LastName, DepartmentID
	FROM Employees AS [e]
	WHERE Salary > (SELECT AVG(Salary) FROM Employees AS em WHERE e.DepartmentID = em.DepartmentID)
	ORDER BY DepartmentID






	