using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using EmployeeRegistration.Models;

namespace EmployeeRegistration.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IConfiguration configuration;
        public EmployeeController(IConfiguration config)
        {
            this.configuration = config;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(EmployeeModel employee)
        {
            string conStr = configuration.GetConnectionString("DefaultConnectionString");
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                string query = "INSERT INTO EmployeeList(Name, Age, Email, PhoneNumber) VALUES (@Name, @Age, @Email, @PhoneNumber)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Age", employee.Age);
                cmd.Parameters.AddWithValue("@Email", employee.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            return Json(true);
        }

        [HttpGet]
        public JsonResult GetEmployeeList()
        {
            List<EmployeeModel> list = new List<EmployeeModel>();
            string conStr = configuration.GetConnectionString("DefaultConnectionString");
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                string query = "SELECT * FROM EmployeeList";

                var _dataTable = new DataTable();

                using SqlDataAdapter _adapter = new SqlDataAdapter(query, con);

                _adapter.Fill(_dataTable);

                list = _dataTable.AsEnumerable().
                    Select(x => new EmployeeModel
                    {
                        Age = Convert.ToInt32(x["Age"]),
                        Email = x["Email"].ToString(),
                        EmployeeId = Convert.ToInt32(x["EmployeeId"]),
                        Name = x["Name"].ToString(),
                        PhoneNumber = x["PhoneNumber"].ToString(),
                    }).
                    ToList();
            }

            return Json(list);
        }
        
        [Route("employee/delete/{_id}")]
        public JsonResult Delete(long _id)
        {
            string conStr = configuration.GetConnectionString("DefaultConnectionString");
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = "DELETE FROM EmployeeList WHERE EmployeeId=@id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = _id;
                    cmd.ExecuteNonQuery();
                }
            }
            return Json(true);
        }

        [Route("employee/edit")]
        [HttpPost]
        public JsonResult Edit(EmployeeModel emp)
        {

            string conStr = configuration.GetConnectionString("DefaultConnectionString");
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = "UPDATE EmployeeList SET Name=@Name, Age=@Age, Email=@Email, PhoneNumber=@PhoneNumber WHERE EmployeeId=@_id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@_id", SqlDbType.Int).Value = emp.EmployeeId;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = emp.Name;
                    cmd.Parameters.Add("@Age", SqlDbType.Int).Value = emp.Age;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = emp.Email;
                    cmd.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = emp.PhoneNumber;
                    cmd.ExecuteNonQuery();
                }
            }
            return Json(true);
        }
    }
}
