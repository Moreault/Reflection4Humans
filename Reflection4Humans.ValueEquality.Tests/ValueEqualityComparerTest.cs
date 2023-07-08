namespace Reflection4Humans.ValueEquality.Tests;

[TestClass]
public class ValueEqualityComparerTest
{
    [TestClass]
    public class EqualsMethod : Tester
    {
        private ValueEqualityComparer _instance = null!;

        protected override void InitializeTest()
        {
            base.InitializeTest();
            _instance = new ValueEqualityComparer { Options = Fixture.Create<ValueEqualityOptions>() };
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void WhenXIsNullButYIsNot_ReturnFalse()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void WhenYIsNullButXIsNot_ReturnFalse()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void WhenBothAreStringAndEqual_ReturnTrue()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        [DataRow(StringComparison.Ordinal, false)]
        [DataRow(StringComparison.OrdinalIgnoreCase, true)]
        [DataRow(StringComparison.CurrentCulture, false)]
        [DataRow(StringComparison.CurrentCultureIgnoreCase, true)]
        [DataRow(StringComparison.InvariantCulture, false)]
        [DataRow(StringComparison.InvariantCultureIgnoreCase, true)]
        public void WhenBothAreStringWithSameTextButDifferentCasing_ReturnTueOrFalseDependingOnStringComparison(StringComparison comparison, bool expected)
        {
            //Arrange

            //Act

            //Assert
        }
    }


}