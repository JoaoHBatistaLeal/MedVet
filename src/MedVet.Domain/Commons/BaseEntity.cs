namespace MedVet.Domain.Commons;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    DateTime CreatedAt { get; set; } = DateTime.Now;
}