using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace EmployeePayrollSystemThreading
{
    public class EmployeePayrollOperations
    {

        public static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=payroll;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);
        
        public List<EmployeeDetails> employeePayrollDetailList = new List<EmployeeDetails>();
        /// <summary>
        /// Adds the employee to payroll. UC1
        /// </summary>
        /// <param name="employeePayrollDataList">The employee payroll data list.</param>
        public void addEmployeeToPayroll(List<EmployeeDetails> employeePayrollDataList)
        {
            employeePayrollDataList.ForEach(employeeData =>
            {
                Console.WriteLine("Employee Being added" + employeeData.EmployeeName);
                this.addEmployeePayroll(employeeData);
                Console.WriteLine("Employee added:" + employeeData.EmployeeName);
            });
            Console.WriteLine(this.employeePayrollDetailList.ToString());
        }
        public void addEmployeeToPayrollWithThread(List<EmployeeDetails> employeePayrollDataList)
        {
            employeePayrollDataList.ForEach(employeeData =>
            {
                Task thread = new Task(() =>
                {
                    Console.WriteLine("Employee Being added" + employeeData.EmployeeName);
                    this.addEmployeePayroll(employeeData);
                    Console.WriteLine("Employee added:" + employeeData.EmployeeName);
                });
                thread.Start();
            });
        }
        public void addEmployeePayroll(EmployeeDetails emp)
        {
            employeePayrollDetailList.Add(emp);
        }
        public void addEmployeeToPayrollDataBase(List<EmployeeDetails> employeePayrollDataList)
        {
            employeePayrollDataList.ForEach(employeeData =>
            {
                Console.WriteLine("Employee being added" + employeeData.EmployeeName);
                this.addEmployeePayrollDatabase(employeeData);
                Console.WriteLine("Employee added" + employeeData.EmployeeName);
            });
        }
        public void addEmployeePayrollDatabase(EmployeeDetails employeeDetails)
        {
            SqlCommand command = new SqlCommand("spInsertData", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EmployeeId", employeeDetails.EmployeeID);
            command.Parameters.AddWithValue("@EmployeeName", employeeDetails.EmployeeName);
            command.Parameters.AddWithValue("@phoneNumber", employeeDetails.PhoneNumber);
            command.Parameters.AddWithValue("@Address", employeeDetails.Address);
            command.Parameters.AddWithValue("@Department", employeeDetails.Department);
            command.Parameters.AddWithValue("@gender", employeeDetails.Gender);
            command.Parameters.AddWithValue("@basicpay", employeeDetails.BasicPay);
            command.Parameters.AddWithValue("@deductions", employeeDetails.Deductions);
            command.Parameters.AddWithValue("@taxablepay", employeeDetails.TaxablePay);
            command.Parameters.AddWithValue("@tax", employeeDetails.Tax);
            command.Parameters.AddWithValue("@netpay", employeeDetails.NetPay);
            command.Parameters.AddWithValue("@city", employeeDetails.City);
            command.Parameters.AddWithValue("@country", employeeDetails.Country);
            connection.Open();
            //adding data into database - using disconnected architecture(as connected architecture only reads the data)
            var result = command.ExecuteNonQuery();
            //closing connection
            Console.WriteLine(result);
            connection.Close();




        }

    }
}
