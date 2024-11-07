using Microsoft.AspNetCore.Mvc; 

   using Microsoft.AspNetCore.Mvc.RazorPages; 

   using Microsoft.AspNetCore.Mvc.Rendering; 

   using Microsoft.Extensions.Configuration; 

   using System.Data.SqlClient; 

  

   namespace Module10Lab.Pages 

   { 

       public class EmployeeManagementModel : PageModel 

       { 

           private readonly string _connectionString; 
// connection string for database connection
  

           public EmployeeManagementModel(IConfiguration configuration) 

           { 

               _connectionString = configuration.GetConnectionString("NorthwindConnection"); 

           } 

  
//binding the form fields to the data we retreive
           [BindProperty] 

           public int? SelectedEmployeeId { get; set; } 

  

           public List<SelectListItem> EmployeeList { get; set; } 

           public dynamic SelectedEmployee { get; set; } 

  
        //Populate employee list when the page loads
        //get = HTTP request
           public void OnGet() 

           { 

               LoadEmployeeList(); 

           } 

  
//called when user submits the form
           public IActionResult OnPost() 

           { 
                //update the employee list
               LoadEmployeeList(); 

               if (SelectedEmployeeId.HasValue) 

               { //gets employee recrod form the database based on the
                //Employee id that the user selected in the dropdown
                   SelectedEmployee = GetEmployeeById(SelectedEmployeeId.Value); 

               } 
            //Returns the page
               return Page(); 

           } 

  
        //this method is called when the user has changed he employee infromation

           public IActionResult OnPostUpdate(int EmployeeID, string Title, string City) 

           { 
            //valls update employee to actually update the rerod in the database
               UpdateEmployee(EmployeeID, Title, City); 
            //rer=driect back to the same web page
               return RedirectToPage(); 
           } 

  
        //called when the user
           public IActionResult OnPostDelete(int EmployeeID) 

           { //deletes the employee record form the datbase based on the
            //employee id that the user selected
               DeleteEmployee(EmployeeID); 

               return RedirectToPage(); 

           } 

  
        //method that is called when the user clicks
           public IActionResult OnPostAdd(string NewFirstName, string NewLastName, string NewTitle) 

           { 
            //tjis method will insert the new em,,ployee record into the database
               AddEmployee(NewFirstName, NewLastName, NewTitle); 

               return RedirectToPage(); 

           } 

  
        //gets the list of employees from the employees table in the northwind databasse
           private void LoadEmployeeList() 

           { 
            //create a new list
               EmployeeList = new List<SelectListItem>(); 
            //create a new database connections
               using (SqlConnection connection = new SqlConnection(_connectionString)) 

               { 
                //open the connection to the database - to the sql server and the
                //northwind database that is part of our sql server
                   connection.Open(); 
                //sql command to run
                   string sql = "SELECT EmployeeID, FirstName, LastName FROM Employees"; 

                   using (SqlCommand command = new SqlCommand(sql, connection)) 

                   { 

                       using (SqlDataReader reader = command.ExecuteReader()) 

                       { //execute the sql data reader and loop through the collections
                        //of records that is retruned from the database
                           while (reader.Read()) 

                           { 
                            //add the employee to the list
                            //create a new selectListItem and add that object to 
                            //the dropdown list of employees
                               EmployeeList.Add(new SelectListItem 

                               { //value and the text
                               //the value is employee id a(employee id is a primary key)
                               //text that is actually displayed in the list
                               //is the employee firsname and lastname

                                   Value = reader["EmployeeID"].ToString(), 

                                   Text = $"{reader["FirstName"]} {reader["LastName"]}" 

                               }); 

                           } 

                       } 

                   } 

               } 

           } 

  
            //create a connectio to the datbase and retrive an employee record
            //based on the employee id
           private dynamic GetEmployeeById(int id) 

           { 
                
            using (SqlConnection connection = new SqlConnection(_connectionString)) 
               { 

                   connection.Open(); 

                   string sql = "SELECT EmployeeID, FirstName, LastName, Title, City FROM Employees WHERE EmployeeID = @Id"; 

                   using (SqlCommand command = new SqlCommand(sql, connection)) 
                   { 
                       command.Parameters.AddWithValue("@Id", id); 
                       using (SqlDataReader reader = command.ExecuteReader()) 
                       { 
                           if (reader.Read()) 
                           { 
                               return new 
                               { 
                                   EmployeeID = (int)reader["EmployeeID"], 

                                   FirstName = reader["FirstName"].ToString(), 

                                   LastName = reader["LastName"].ToString(), 

                                   Title = reader["Title"].ToString(), 

                                   City = reader["City"].ToString() 
                               }; 
                           } 
                       } 
                   } 
               } 
               return null; 

           } 

  

           private void UpdateEmployee(int employeeId, string title, string city) 

           { 

               using (SqlConnection connection = new SqlConnection(_connectionString)) 

               { 

                   connection.Open(); 

                   string sql = "UPDATE Employees SET Title = @Title, City = @City WHERE EmployeeID = @EmployeeID"; 

                   using (SqlCommand command = new SqlCommand(sql, connection)) 

                   { 
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@EmployeeID", employeeId); 
                    command.Parameters.AddWithValue("@Title", title); 
                    command.Parameters.AddWithValue("@City", city); 
                    command.ExecuteNonQuery(); 
                   } 
               } 
           } 
           private void DeleteEmployee(int id) 
           { 
               using (SqlConnection connection = new SqlConnection(_connectionString)) 
               { 
                   connection.Open(); 
                   string sql = "DELETE FROM Employees WHERE EmployeeID = @Id"; 
                   using (SqlCommand command = new SqlCommand(sql, connection)) 
                   { 
                       command.Parameters.AddWithValue("@Id", id); 

                       command.ExecuteNonQuery(); 
                   } 
               } 
           } 
           private void AddEmployee(string firstName, string lastName, string title) 
           { 
               using (SqlConnection connection = new SqlConnection(_connectionString)) 
               { 
                   connection.Open(); 
                   //sql statment to execute
                   string sql = @"INSERT INTO Employees (FirstName, LastName, Title, BirthDate, HireDate, Address, City, Country) 
                                  VALUES (@FirstName, @LastName, @Title, '1980-01-01', GETDATE(), '123 Main St', 'Anytown', 'USA')"; 
                   using (SqlCommand command = new SqlCommand(sql, connection)) 
                   { 
                    //exeecute the sql command and adding the values from the webpage as paratmeters
                       command.Parameters.AddWithValue("@FirstName", firstName); 
                       command.Parameters.AddWithValue("@LastName", lastName); 
                       command.Parameters.AddWithValue("@Title", title); 
                       command.ExecuteNonQuery(); 
                   } 
               } 
           } 
       } 
   } 

 