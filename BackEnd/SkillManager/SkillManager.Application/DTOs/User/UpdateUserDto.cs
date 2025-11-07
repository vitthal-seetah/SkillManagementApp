public class UpdateUserDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string Eid { get; set; } = string.Empty;
    public string? UtCode { get; set; }
    public string? RefId { get; set; }
    public string? Status { get; set; }
    public string? DeliveryType { get; set; }
    public string? RoleName { get; set; }
    public int? ProjectId { get; set; }
    public int? TeamId { get; set; }
}
