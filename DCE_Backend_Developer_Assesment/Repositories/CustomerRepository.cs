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
