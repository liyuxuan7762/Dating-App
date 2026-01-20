using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

/**
 * RegisterDto
 * 
 * @property {string} Email - The email of the user.
 * @property {string} DisplayName - The display name of the user.
 * @property {string} Password - The password of the user.
 */
public class RegisterDto
{
    [Required] public string Email { get; set; } = "";

    [Required] public string DisplayName { get; set; } = "";

    [Required] [MinLength(4)] public string Password { get; set; } = "";
}