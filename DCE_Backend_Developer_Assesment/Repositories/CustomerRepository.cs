using DCE_Backend_Developer_Assesment.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DCE_Backend_Developer_Assesment.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            // Use IConfiguration to retrieve the connection string from appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

   

        public IEnumerable<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM Customer", connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(MapCustomerFromReader(reader));
                    }
                }
            }

            return customers;
        }

        public bool IsEmailInUse(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Customer WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    int count = (int)command.ExecuteScalar();
                    return count > 0; // Return true if email is already in use
                }
            }
        }

        public Customer AddCustomer(Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(
                    "INSERT INTO Customer (Username, Email, FirstName, LastName) " +
                    "OUTPUT INSERTED.UserId, INSERTED.IsActive, INSERTED.CreatedOn " +
                    "VALUES (@Username, @Email, @FirstName, @LastName);",
                    connection))
                {
                    command.Parameters.AddWithValue("@Username", customer.Username);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                  

                    // Execute the INSERT command and capture the output values
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Update the customer object with the returned values
                            customer.UserId = (Guid)reader["UserId"];
                            customer.IsActive = (bool)reader["IsActive"];
                            customer.CreatedOn= (DateTime)reader["CreatedOn"];
                        }
                        else
                        {
                            return null; // Insertion failed
                        }
                    }

                    return customer; // Registration was successful
                }
            }
        }




        public Customer GetCustomerById(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM Customer WHERE UserId = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapCustomerFromReader(reader);
                        }
                        else
                        {
                            return null; // Customer not found
                        }
                    }
                }
            }
        }
        public bool UpdateCustomer(Guid id, string? username, string? email, string? firstName, string? lastName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Check if the new email already exists for another customer
                if (email != null && IsEmailInUse(email))
                {
                    return false; // Return false to indicate that email is already in use
                }

                string updateSql = "UPDATE Customer SET ";
                List<string> updateColumns = new List<string>();

                if (username != null)
                {
                    updateColumns.Add("Username = @Username");
                }

                if (email != null)
                {
                    updateColumns.Add("Email = @Email");
                }

                if (firstName != null)
                {
                    updateColumns.Add("FirstName = @FirstName");
                }

                if (lastName != null)
                {
                    updateColumns.Add("LastName = @LastName");
                }

                updateSql += string.Join(", ", updateColumns);
                updateSql += " WHERE UserId = @UserId";

                using (SqlCommand command = new SqlCommand(updateSql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", id);

                    if (username != null)
                    {
                        command.Parameters.AddWithValue("@Username", username);
                    }

                    if (email != null)
                    {
                        command.Parameters.AddWithValue("@Email", email);
                    }

                    if (firstName != null)
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                    }

                    if (lastName != null)
                    {
                        command.Parameters.AddWithValue("@LastName", lastName);
                    }

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool DeleteCustomer(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("DELETE FROM Customer WHERE UserId = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }



        private Customer MapCustomerFromReader(SqlDataReader reader)
        {
            return new Customer
            {
                UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }
    }
}
