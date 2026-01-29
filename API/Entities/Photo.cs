using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Entities;

public class Photo
{
	public int Id { get; set; }

	public required string Url { get; set; }

	public string? PublicId { get; set; }

	public string MemberId { get; set; } = null!;

	[JsonIgnore]
	[ForeignKey(nameof(MemberId))]
	public Member Member { get; set; } = null!;

}
