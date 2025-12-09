using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.DataModels;

public partial class User
{
    public int UserId { get; set; }

    public int? RoleId { get; set; }

    [Required(ErrorMessage = "Обязательно для заполнения")]
    [MinLength(3, ErrorMessage = "Минимальная длина имени пользователя - 3 символа")]
    [MaxLength(20, ErrorMessage = "Максимальная длина имени пользователя - 20 символов")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Обязательно для заполнения")]
    [MinLength(8, ErrorMessage = "Минимальная длина пароля - 8 символов")]
    [MaxLength(16, ErrorMessage = "Максимальная длина пароля - 16 символов")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Обязательно для заполнения")]
    [MaxLength(20, ErrorMessage = "Имя не может быть длиннее 20 символов")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Обязательно для заполнения")]
    [MaxLength(20, ErrorMessage = "Фамилия не может быть длиннее 20 символов")]
    public string Surname { get; set; } = null!;

    [Required(ErrorMessage = "Обязательно для заполнения")]
    [MaxLength(20, ErrorMessage = "Отчество не может быть длиннее 25 символов")]
    public string Patronymic { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual Role Role { get; set; } = null!;
}
