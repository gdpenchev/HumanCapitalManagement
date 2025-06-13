Human Capital Management System

The solution contains 3 primary .Net projects (.Net 8)

- AuthenticationAPI: handles user authentication and issuance JWT tokens
- EmployeeAPI: provides employee management (CRUD) operations per roles.
- PeopleManagementUI: basic UI interface for intaracting with authentication and employee APIs.

 AuthenticationAPI: ASP.NET Core
 - Issues JWT token based on provided username/password
 - UserService provides validation of users and roles. Currently for the demo this is limited to only 3 already populaed users with roles:
   Username Password Role
   Maria     12345   hradmin
   Pesho     12345   manager
   Ivan      12345   employee
   
  hradmin - can create employees, edit details + salaries, delete employees, view employees,
  manager - can edit details (without salaries), view employees,
  employee - can view employees
  
 - endpoint: https://localhost:7143/api/auth/login
 
 EmployeeAPI: ASP.NET Core
 - Manages employee records with secure,role based access.
 - EmployeeService has already populated 3 employees for demonstration
 - endpoints:
	https://localhost:7195/api/employee/create
	https://localhost:7195/api/employee/getall
	https://localhost:7195/api/employee/getById/{id}
	https://localhost:7195/api/employee/delete/{id}
	https://localhost:7195/api/employee/update/{id}
	
 PeopleManagementUI: ASP.NET Core MVC/Razor
 - Consumes AuthenticationAPI and EmployeeAPI
 - Login view: Enter credentials of a specific role (see aboove roles and credentials)
 - One logged in you will see list of employees with limitation of the view depending on the logged user
 - You have Login button option to navigate from the employee list section to the login view.