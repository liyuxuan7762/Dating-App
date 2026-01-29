using API.Entities;

namespace API.Interfaces;

public interface IMemberRepository
{
	public Task<Member?> GetMemberByIdAsync(string id);

	public Task<IReadOnlyList<Member>> GetMembersAsync();

	public Task<bool> SaveAllAsync();

	public void UpdateAsync(Member member);

	public Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId);
}
