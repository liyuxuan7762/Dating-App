using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MemberRepository(AppDbContext context) : IMemberRepository
{
	public async Task<Member?> GetMemberByIdAsync(string id)
	{
		return await context.Members.FindAsync(id);
	}

	public async Task<IReadOnlyList<Member>> GetMembersAsync()
	{
		return await context.Members.ToListAsync();
	}

	public async Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId)
	{
		return await context.Photos.Where(p => p.MemberId == memberId).ToListAsync();
	}

	public async Task<bool> SaveAllAsync()
	{
		return await context.SaveChangesAsync() > 0;
	}

	public void UpdateAsync(Member member)
	{
		// 这段代码并没有实际的作用，只是为了告诉 Entity Framework Core 该实体已经被修改了
		// 当在业务层调用保存方法的时候，Entity Framework Core 会根据这个状态来生成 SQL 语句
		context.Entry(member).State = EntityState.Modified;
	}
}
