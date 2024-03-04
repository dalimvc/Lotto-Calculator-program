using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Calculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        string firstNumber = "0";
        string secondNumber = "0";
        int currentValue;
        bool newNumber = true;
        bool errorPresent = false;
        bool divisinWithZero = false;
        string buttonValueOperation;
        string previousOperation;

        //method to assign numbers to variables when user clicks on number buttons
        //currentValue is current result; first and secons numbers are result of user input
        //if a number is first imput it will be assihned to fisrtNumber, othervise it will be assigned to the second variable
        private void AppendNumber(string number)
        {
            errorPresent = false;
            divisinWithZero = false;
            if (newNumber)
            {
                firstNumber = firstNumber + number;
                try
                {
                    currentValue = int.Parse(firstNumber);
                    ResultData.Text = currentValue.ToString();
                }
                catch
                {
                    ResultData.Text = "Number is too large.";
                }
            }
            else
            {
                secondNumber = secondNumber + number;
                try
                {
                    currentValue = int.Parse(secondNumber);
                    ResultData.Text = currentValue.ToString();
                }
                catch
                {
                    ResultData.Text = "Number is too large.";
                }
            }
        }

        //based on chosen operation different operation will be performed
        //checked is used to check if number is too large and it would return an error message in that case
        //current value will be assigned the value of result after aritmetic operation is performed
        public void CalculateResult()
        {
            errorPresent = false;
            divisinWithZero = false;
            if (!string.IsNullOrEmpty(previousOperation) && secondNumber != "0")
            {
                if (previousOperation == "+")
                {
                    try
                    {
                        checked
                        {
                            currentValue = int.Parse(firstNumber) + int.Parse(secondNumber);
                        }
                    }
                    catch (OverflowException)
                    {
                        errorPresent = true;
                        newNumber = true;
                        firstNumber = "";
                        secondNumber = "";
                        currentValue = 0;
                    }
                }
                else if (previousOperation == "-")
                {
                    try
                    {
                        checked
                        {
                            currentValue = int.Parse(firstNumber) - int.Parse(secondNumber);
                        }
                    }
                    catch (OverflowException)
                    {
                        errorPresent = true;
                        newNumber = true;
                        firstNumber = "";
                        secondNumber = "";
                        currentValue = 0;
                    }
                    catch (FormatException)
                    {
                        currentValue = 0;
                        newNumber = true;
                        firstNumber = "0";
                        secondNumber = "0";
                        currentValue = 0;
                    }
                }
                else if (previousOperation == "*")
                {
                    try
                    {
                        checked
                        {
                            currentValue = int.Parse(firstNumber) * int.Parse(secondNumber);
                        }
                    }
                    catch (OverflowException)
                    {
                        errorPresent = true;
                        newNumber = true;
                        firstNumber = "";
                        secondNumber = "";
                        currentValue = 0;
                    }
                    catch (FormatException)
                    {
                        currentValue = 0;
                        newNumber = true;
                        firstNumber = "0";
                        secondNumber = "0";
                        currentValue = 0;
                    }
                }
                else if (previousOperation == "/" && secondNumber != "0" && secondNumber != "00")
                {
                    Debug.WriteLine(secondNumber);
                    currentValue = int.Parse(firstNumber) / int.Parse(secondNumber);
                }
                firstNumber = currentValue.ToString();
                secondNumber = "";
            }
            else if (previousOperation == "/" && secondNumber == "0")
            {
                divisinWithZero = true;
            }
        }

        //method, on click registers users input- numbers
        //it calls AppendNumber method with button value
        private void ButtonNumber(object sender, RoutedEventArgs e)
        {
            var buttonValue = (Button)sender;
            AppendNumber(buttonValue.Content.ToString());
        }

        //here buuton on click operation is detected
        //based on that value different aritmetic operation will be performed
        //error messages are handled here
        private void ButtonOperation(object sender, RoutedEventArgs e)
        {
            newNumber = false;
            var buttonValue = (Button)sender;
            buttonValueOperation = buttonValue.Content.ToString();
            CalculateResult();
            previousOperation = buttonValueOperation;
            if (divisinWithZero)
            {
                ResultData.Text = "Error: Division by zero";
                newNumber = true;
                firstNumber = "";
                secondNumber = "";
                currentValue = 0;
                previousOperation = null;
            }
            else if (errorPresent)
            {
                ResultData.Text = "Number is too large.";
            }
            else
            {
                ResultData.Text = currentValue.ToString();
            }
        }

        //it works in a similar way as previuos method
        //when user clicks on "=" an operation will be performed by calling CalculateResult() method
        //error massages are handled here
        private void ButtonEquals(object sender, RoutedEventArgs e)
        {
            CalculateResult();
            if (divisinWithZero)
            {
                ResultData.Text = "Error: Division by zero";
                newNumber = true;
                firstNumber = "";
                secondNumber = "";
                currentValue = 0;
                previousOperation = null;
            }
            else if (errorPresent)
            {
                ResultData.Text = "Number is too large.";
            }
            else
            {
                ResultData.Text = firstNumber.ToString();
            }
            previousOperation = null;
        }

        //all data is reset by calling this method
        private void ButtonC(object sender, RoutedEventArgs e)
        {
            newNumber = true;
            firstNumber = "0";
            secondNumber = "0";
            currentValue = 0;
            ResultData.Text = currentValue.ToString();
            previousOperation = null;
        }
    }
}
