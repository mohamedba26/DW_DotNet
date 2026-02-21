using System.ComponentModel.DataAnnotations;

namespace SalesDW.API.Models;

public class AuthUser
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string PasswordHash { get; set; } = null!;

    // Salt for password hashing (base64)
    public string Salt { get; set; } = null!;

    // Role: use integer codes (e.g., 1 = Admin, 2 = User)
    public int Role { get; set; } = 2;
}
