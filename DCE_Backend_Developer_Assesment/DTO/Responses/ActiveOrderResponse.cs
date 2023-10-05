namespace DCE_Backend_Developer_Assesment.DTO.Responses
{
    public class ActiveOrderResponse
    {
        public Guid OrderId { get; set; }
        public int OrderStatus { get; set; }
        public int OrderType { get; set; }
        public DateTime OrderedOn { get; set; }
        public DateTime? ShippedOn { get; set; }
        public Guid ProductId { get; set; }
        public Guid SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
