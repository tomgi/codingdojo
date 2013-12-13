using System;
using Machine.Specifications;

namespace Ajejczes.CodingDojo.Tests
{
    public class with_string_calculator
    {
        Establish context = () => sut = new StringCalculator();
        protected static StringCalculator sut;
    }

    [Subject(typeof (StringCalculator))]
    public class when_empty_input_given : with_string_calculator
    {
        Because of = () => result = sut.Add("");

        It should_return_0 = () => result.ShouldEqual(0);

        static int result;
    }

    public class when_a_single_number_is_provided_for_addition : with_string_calculator
    {
        Because of = () =>
            result = sut.Add("1");

        It should_return_the_same_number = () => { result.ShouldEqual(1); };

        static int result;
    }

    [Subject(typeof (StringCalculator))]
    public class when_two_numbers_are_provided_for_addition : with_string_calculator
    {
        Because of = () =>
            result = sut.Add("2,3");

        It should_return_their_sum = () =>
            result.ShouldEqual(5);

        static int result;
    }

    [Subject(typeof (StringCalculator))]
    public class when_set_of_numbers_is_provided_for_addition : with_string_calculator
    {
        Because of = () => result = sut.Add("1,2,3,4");

        It should_return_sum_of_all_numbers = () => result.ShouldEqual(10);

        static int result;
    }

    [Subject(typeof (StringCalculator))]
    public class when_numbers_are_separated_by_new_lines_or_commas_are_provided_for_addition : with_string_calculator
    {
        Because of = () =>
            result = sut.Add("1\n2,3");

        It should_return_their_sum = () => result.ShouldEqual(6);


        static int result;
    }

    [Subject(typeof (StringCalculator))]
    public class when_custom_delimiter_is_provided_for_addition : with_string_calculator
    {
        Because of = () =>
            result = sut.Add("//;\n1;2");

        It should_return_sum_of_numbers_separated_by_the_custom_delimiter = () =>
            result.ShouldEqual(3);

        static int result;
    }

    [Subject(typeof (StringCalculator))]
    public class when_negative_numbers_are_provided_for_addition : with_string_calculator
    {
        Because of = () =>
            exception = Catch.Exception(() => result = sut.Add("-1,-2"));

        It should_throw = () =>
            exception.ShouldNotBeNull();

        It should_throw_meaningfull_message = () =>
            exception.Message.ShouldStartWith("Negatives not allowed");

        It should_throw_message_containing_negative_numbers = () =>
        {
            exception.Message.ShouldContain("-1");
            exception.Message.ShouldContain("-2");
        };

        static int result;
        static Exception exception;
    }

    [Subject(typeof (StringCalculator))]
    public class when_numbers_greater_than_1000_are_provided_for_addition : with_string_calculator
    {
        Because of = () =>
            result = sut.Add("1001, 2");

        It should_ignore_numbers_greater_than_1000 = () =>
            result.ShouldEqual(2);

        static int result;
    }

    [Subject(typeof (StringCalculator))]
    public class when_long_custom_delimiter_is_provided_for_addition : with_string_calculator
    {
        Because of = () =>
            result = sut.Add("//[***]\n1***2***3");

        It should_return_sum_of_numbers_separated_by_the_long_custom_delimiter = () => 
            result.ShouldEqual(6);

        static int result;
    }
}