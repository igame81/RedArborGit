using log4net;
using RedArbor.Dao.Interface;
using RedArbor.Models;
using RedArbor.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RedArbor.Dao
{

    public class EmployeeDao : IEmployeeDao
    {
        private string ConnectionString = string.Empty;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public EmployeeDao()
        {
            this.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public Employee FindById(int id)
        {
            Employee result = null;

            string sqlQuery = "select * from Employees where Id=@Id";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@Id", id);

                Log.DebugFormat("FindById: {0} ID: {1}", command.CommandText, id);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        result = new Employee();

                        while (dataReader.Read())
                        {
                            result = this.GetEmployeeFromDataReader(dataReader);
                        }
                    }
                }
                
            }

            return result;
        }

        public Employee[] FindAll()
        {
            List<Employee> result = new List<Employee>();
            
            string sqlQuery = String.Format("select * from Employees");
            Employee employee = null;
            
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlQuery, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {

                            employee = this.GetEmployeeFromDataReader(dataReader);
                            result.Add(employee);
                        }
                    }
                }

                command.Dispose();
            }

            return result.ToArray<Employee>();
        }

        public Employee SaveOrUpdate(Employee employee)
        {
            
            string createQuery = "Insert into Employees (CompanyId, CreatedOn, DeletedOn, UpdatedOn, Email, Fax, Name, Password, PortalId, RoleId, StatusId, Telephone, Username, Lastlogin) "
                + " Values(@CompanyId, @CreatedOn, @DeletedOn, @UpdatedOn, @Email, @Fax, @Name, @Password, @PortalId, @RoleId, @StatusId, @Telephone, @Username, @Lastlogin );"
                + "Select @@Identity";

            string updateQuery = "Update Employees SET "
                + " CompanyId = @CompanyId, CreatedOn = @CreatedOn, DeletedOn = @DeletedOn, UpdatedOn = @UpdatedOn, Email = @Email, Fax = @Fax, Name = @Name, Password = @Password, PortalId = @PortalId, RoleId = @RoleId, StatusId = @StatusId, Telephone = @Telephone, Username = @Username, Lastlogin = @Lastlogin "
                + " Where Id = @Id";

            int savedEmployeeId = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand command = null;

                if (employee.Id != 0)
                {
                    command = new SqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@Id", employee.Id);
                }
                else
                {
                    command = new SqlCommand(createQuery, connection);
                }

                command.Parameters.AddWithValue("@CompanyId", employee.CompanyId);
                command.Parameters.AddWithValue("@CreatedOn", this.ConvertNullValue(employee.CreatedOn));
                command.Parameters.AddWithValue("@DeletedOn", this.ConvertNullValue(employee.DeletedOn));
                command.Parameters.AddWithValue("@UpdatedOn", this.ConvertNullValue(employee.UpdatedOn));
                command.Parameters.AddWithValue("@Lastlogin", this.ConvertNullValue(employee.Lastlogin));
                command.Parameters.AddWithValue("@Email", employee.Email);
                command.Parameters.AddWithValue("@Fax", this.ConvertNullValue(employee.Fax));
                command.Parameters.AddWithValue("@Name", this.ConvertNullValue(employee.Name));
                command.Parameters.AddWithValue("@Password", EncryptionUtils.Encrypt(employee.Password));
                command.Parameters.AddWithValue("@PortalId", employee.PortalId);
                command.Parameters.AddWithValue("@RoleId", employee.RoleId);
                command.Parameters.AddWithValue("@StatusId", employee.StatusId);
                command.Parameters.AddWithValue("@Username", employee.Username);
                command.Parameters.AddWithValue("@Telephone", this.ConvertNullValue(employee.Telephone));


                try
                {
                    var commandResult = command.ExecuteScalar();
                    
                    if (commandResult != null)
                    {
                        savedEmployeeId = Convert.ToInt32(commandResult);
                    }
                    else
                    {
                        savedEmployeeId = employee.Id;
                    }

                    command.Dispose();
                    
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat("Error on SaveOrUpdate {0}", ex.ToString());
                    throw ex;
                }

            }

            if (employee.Id == 0)
            {
                employee.Id = savedEmployeeId;
            }

            return employee;
        }

        public bool Delete(int id)
        {
            bool result = false;

            string sqlQuery = "delete from Employees where Id = @Id";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@Id", id);

                int rowsDeletedCount = command.ExecuteNonQuery();
                if (rowsDeletedCount != 0)
                {
                    result = true;
                }

                command.Dispose();
            }
            

            return result;
        }

        #region Private
        private Employee GetEmployeeFromDataReader(SqlDataReader dataReader)
        {
            Employee employee = new Employee();

            employee.Id = Convert.ToInt32(dataReader["Id"]);
            employee.CompanyId = Convert.ToInt32(dataReader["CompanyId"]);
            if (dataReader["CreatedOn"] != DBNull.Value)
            {
                employee.CreatedOn = Convert.ToDateTime(dataReader["CreatedOn"]);
            }
            if (dataReader["DeletedOn"] != DBNull.Value)
            {
                employee.DeletedOn = Convert.ToDateTime(dataReader["DeletedOn"]);
            }

            if (dataReader["Lastlogin"] != DBNull.Value)
            {
                employee.Lastlogin = Convert.ToDateTime(dataReader["Lastlogin"]);
            }

            if (dataReader["UpdatedOn"] != DBNull.Value)
            {
                employee.UpdatedOn = Convert.ToDateTime(dataReader["UpdatedOn"]);
            }

            employee.Email = dataReader["Email"].ToString();
            if (dataReader["Fax"] != DBNull.Value)
            {
                employee.Fax = dataReader["Fax"].ToString();
            }
            if (dataReader["Name"] != DBNull.Value)
            {
                employee.Name = dataReader["Name"].ToString();
            }
            employee.Password = EncryptionUtils.Decrypt(dataReader["Password"].ToString());
            employee.PortalId = Convert.ToInt32(dataReader["PortalId"]);
            employee.RoleId = Convert.ToInt32(dataReader["RoleId"]);
            employee.StatusId = Convert.ToInt32(dataReader["StatusId"]);
            if (dataReader["Telephone"] != DBNull.Value)
            {
                employee.Telephone = dataReader["Telephone"].ToString();
            }
            employee.Username = dataReader["Username"].ToString();

            return employee;

        }

        private object ConvertNullValue(object propertyValue)
        {
            if (propertyValue != null)
            {
                return propertyValue;
            }
            else
            {
                return DBNull.Value;
            }
        }
        #endregion
    }
}