using Machine.Specifications;
using StringCalculatorKata;

namespace StringCalculatorTests
{
    [Subject(typeof (StringCalculator))]
    public class when_empty_input_given
    {
        It should_return_0 = () =>
        {
            new StringCalculator().Add("").ShouldEqual(0);
        };
    }
}