truncate table Designation_Salary_tbl

INSERT INTO [dbo].[Designation_Salary_tbl] 
    ([Designation_Name], [BaseSalary], [HRA], [DA], [TA], [OtherAllowances], [Deductions], [Hourly_Rate])
VALUES 
    ('Manager', 50000.00, 10000.00, 5000.00, 3000.00, 2000.00, 1500.00, 250.00),
    ('Developer', 40000.00, 8000.00, 4000.00, 2500.00, 1500.00, 1200.00, 220.00),
    ('Tester', 35000.00, 7000.00, 3500.00, 2000.00, 1000.00, 1100.00, 210.00),
    ('HR', 45000.00, 9000.00, 4500.00, 2500.00, 1800.00, 1300.00, 240.00);
