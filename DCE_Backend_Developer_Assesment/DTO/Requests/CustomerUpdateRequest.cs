namespace DCE_Backend_Developer_Assessment.DTO.Requests
{
    public class CustomerUpdateRequest
    {
        public Guid UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}

