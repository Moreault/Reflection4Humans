namespace Reflection4Humans.TypeFetcher.Tests.Garbage;

[Garbage(Description = "I am a dummy")]
public class AttributedGarbage
{
    public DateTime SomeDate { get; set; }
}