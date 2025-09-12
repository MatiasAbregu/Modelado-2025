using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.BD.Entidades
{
    public class Tokens
    {
        [Key]
        public long Id { get; set; }
      
        [Required]
        public long UserId { get; set; }
        public Usuarios Usuario { get; set; }

        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime ExpiresAt { get; set; }

        public Tokens() { }

        public Tokens(long userId, string refreshToken, DateTime createdAt, DateTime expiresAt)
        {
            UserId = userId;
            RefreshToken = refreshToken;
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
        }
    }
}
