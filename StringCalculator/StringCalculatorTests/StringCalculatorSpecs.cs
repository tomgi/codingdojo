using System.Runtime.InteropServices;
using Machine.Specifications;
using StringCalculatorKata;

namespace StringCalculatorTests
{
    public class given_string_calculator
    {
        Establish context = () => sut = new StringCalculator();
        protected static StringCalculator sut;
    }

    [Subject(typeof (StringCalculator))]
    public class when_empty_input_given : given_string_calculator
    {
        Because of = () => result = sut.Add("");
            
        It should_return_0 = () => result.ShouldEqual(0);

        static int result;
    }
    
    public class when_a_single_number_is_given : given_string_calculator
    {
        Because of = () =>
            result = sut.Add("1");
        It should_return_the_same_number = () =>
        {
            result.ShouldEqual(1);
        };

        static int result;
    }

    [Subject(typeof (StringCalculator))]
    public class when_two_numbers_are_given : given_string_calculator
    {
        Because of = () =>
            result = sut.Add("2,3");

        It should_return_their_sum = () =>
            result.ShouldEqual(5);

        static int result;
    }

    [Subject(typeof (StringCalculator))]
    public class when_set_of_numbers_is_given : given_string_calculator
    {
        Because of = () => result = sut.Add("1,2,3,4");

        It should_return_sum_of_all_numbers = () => result.ShouldEqual(10);
            
        static int result;
            
    }

    [Subject(typeof (StringCalculator))]
    public class when_numbers_are_separated_by_new_lines_or_commas_are_given : given_string_calculator
    {
        Because of = () =>
            result = sut.Add("1\n2,3");

        It should_return_their_sum = () => result.ShouldEqual(6);
            

        static int result;
    }
}