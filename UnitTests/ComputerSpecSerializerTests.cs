using DetectiveSpecs;
using DetectiveSpecs.Enums;

namespace UnitTests;

[TestClass]
public class ComputerSpecSerializerTests
{
    [TestMethod]
    public void Serialization_Returns_NotNull()
    {
        var computerSpecs = TestData.CreateComputerSpecs();

        var computerSpecSerializer = new ComputerSpecSerializer();
        var serializedSpecs = computerSpecSerializer.Serialize(computerSpecs);
        
        Assert.IsNotNull(serializedSpecs);
    }



    [TestMethod]
    public void SerializedData_IsProperlyPadded()
    {
        var computerSpecs = TestData.CreateComputerSpecs();
        var computerSpecSerializer = new ComputerSpecSerializer();
        var serializedSpecs = computerSpecSerializer.Serialize(computerSpecs);
        string[] lines = serializedSpecs.Split(Environment.NewLine);
        
        foreach (var line in lines)
        {
            IEnumerable<string> enumStrings = Enum
                .GetValues<ComponentProperty>()
                .Select(s => s.ToString());
            
            if (!enumStrings.Contains(line))
                continue;
            
            ComponentProperty? componentProperty = Enum
                .GetValues<ComponentProperty>()
                .FirstOrDefault(@enum => @enum.ToString() == line[computerSpecSerializer.PadLength..]);

            Assert.IsNotNull(componentProperty);
        }
    }
}