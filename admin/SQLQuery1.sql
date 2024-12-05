truncate table Designation_Salary_tbl

INSERT INTO [dbo].[Designation_Salary_tbl] 
([Designation_Name], [BaseSalary], [HRA], [DA], [TA], [OtherAllowances], [Deductions]) 
VALUES 
('Manager', 50000.00, 10000.00, 8000.00, 5000.00, 2000.00, 3000.00),
('Team Lead', 40000.00, 8000.00, 6000.00, 4000.00, 1500.00, 2500.00),
('Software Engineer', 30000.00, 6000.00, 5000.00, 3000.00, 1000.00, 2000.00),
('Intern', 15000.00, 3000.00, 2000.00, 1000.00, 500.00, 1000.00),
('HR', 35000.00, 7000.00, 5000.00, 2000.00, 1000.00, 1500.00);