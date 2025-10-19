using Application.Enums;

namespace Application.ValueObject;
public class CoutReaction
{
    /// <summary>
    ///  Reaction type to count
    /// </summary>
    public ReactionType ReactionType { get; set; }
    /// <summary>
    /// Count for reaction
    /// </summary>
    public long CountReaction { get; set; }
}