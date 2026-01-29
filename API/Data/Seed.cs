using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

/// <summary>
/// 这和类用来从Json文件中读取数据并将其填充到数据库中
/// </summary>
public class Seed
{
	/// <summary>
	/// 从Json文件中读取用户数据并填充到数据库中
	/// </summary>
	/// <param name="context"></param> 这个参数并不是依赖注入的参数，而是我们调用的时候要传递的参数。构造器注入的前提是类必须被实例化（即 new Seed()），并且通常是由依赖注入容器（DI Container）来管理这个实例的生命周期。静态方法不属于类的任何实例，它属于类本身。调用静态方法时，不需要创建类的对象。
	/// <returns></returns>
	public static async Task SeedUsers(AppDbContext context)
	{
		// 首先查询数据库中是否已经存在数据
		if (await context.Users.AnyAsync()) return;

		// 从Json文件中读取数据
		var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

		// 反序列成C#对象
		var userList = JsonSerializer.Deserialize<List<SeedUserDto>>(userData);

		if (userList == null || userList.Count == 0)
		{
			Console.WriteLine("用户数据为空");
			return;
		}

		// 循环用户列表，给每一个用户实体
		foreach (var userDto in userList)
		{
			using var hmac = new HMACSHA512();

			var user = new AppUser
			{
				Id = userDto.Id,
				Email = userDto.Email,
				ImageUrl = userDto.ImageUrl,
				DisplayName = userDto.DisplayName,
				PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")),
				PasswordSalt = hmac.Key,
				Member = new Member
				{
					Id = userDto.Id,
					DateOfBirth = userDto.DateOfBirth,
					ImageUrl = userDto.ImageUrl,
					DisplayName = userDto.DisplayName,
					Gender = userDto.Gender,
					Description = userDto.Description,
					Country = userDto.Country,
					City = userDto.City,
					UserId = userDto.Id,
					Created = userDto.Created,
					LastActive = userDto.LastActive
				}
			};

			user.Member.Photos.Add(new Photo
			{
				Url = userDto.ImageUrl!,
				MemberId = userDto.Id
			});

			context.Users.Add(user);
		}

		await context.SaveChangesAsync();
	}
}
