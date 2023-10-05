using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DCE_Backend_Developer_Assesment.DTO.Responses;
using Microsoft.Extensions.Configuration;

namespace DCE_Backend_Developer_Assesment.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            // Use IConfiguration to retrieve the connection string from appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<ActiveOrderResponse> GetActiveOrdersByCustomer(Guid customerId)
        {
            List<ActiveOrderResponse> activeOrders = new List<ActiveOrderResponse>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetActiveOrdersByCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            activeOrders.Add(new ActiveOrderResponse
                            {
                                OrderId = reader.GetGuid(reader.GetOrdinal("OrderId")),
                                OrderStatus = reader.GetInt32(reader.GetOrdinal("OrderStatus")),
                                OrderType = reader.GetInt32(reader.GetOrdinal("OrderType")),
                                OrderedOn = reader.GetDateTime(reader.GetOrdinal("OrderedOn")),
                                ShippedOn = reader.IsDBNull(reader.GetOrdinal("ShippedOn")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("ShippedOn")),
                                ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                                SupplierId = reader.GetGuid(reader.GetOrdinal("SupplierId")),
                                SupplierName = reader.GetString(reader.GetOrdinal("SupplierName")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice"))
                            });
                        }
                    }
                }
            }

            return activeOrders;
        }

    }
}
