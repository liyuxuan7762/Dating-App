using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext ctx, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        
        if (await EmailExists(registerDto.Email)) return BadRequest("Email already exists");
        
        // 获取哈希加密工具类
        using var hmac = new HMACSHA512();

        // 创建用户
        AppUser user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        // 保存用户信息到数据库
        ctx.Users.Add(user);
        
        await ctx.SaveChangesAsync();
        
        return user.ToDto(tokenService);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        // 根据邮箱到数据库中找到该用户
        var user = await ctx.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email);
        
        if (user == null) return Unauthorized("The email or password is incorrect");

        // 将提供的密码进行哈希比对
        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (var i = 0; i < computeHash.Length; i++)
        {
            if (computeHash[i] != user.PasswordHash[i]) 
                return Unauthorized("The email or password is incorrect");
        }

        return user.ToDto(tokenService);

    }

    /**
     * Check if the email is existing in DB
     */
    private async Task<bool> EmailExists(string email)
    {
        return await ctx.Users.AnyAsync(u => u.Email == email);
    }
}