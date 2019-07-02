using Model;
using System;
using System.Windows.Input;

namespace ViewModel
{
    public class CalculatorViewModel : ViewModelBase
    {

        private CalculatingModel calculation;

        private string lastOperation;
        private bool newDisplayRequired = false;
        private string display;


        public CalculatorViewModel()
        {
            this.calculation = new CalculatingModel();
            this.display = "0";
            this.FirstOperand = string.Empty;
            this.SecondOperand = string.Empty;
            this.Operation = string.Empty;
            this.lastOperation = string.Empty;
        }

        private RelayCommand digitButtonPressCommand;
        private RelayCommand operationButtonPressCommand;

        public string FirstOperand
        {
            get { return calculation.FirstOperand; }
            set { calculation.FirstOperand = value; }
        }

        public string SecondOperand
        {
            get { return calculation.SecondOperand; }
            set { calculation.SecondOperand = value; }
        }

        public string Operation
        {
            get { return calculation.Operation; }
            set { calculation.Operation = value; }
        }

        public string LastOperation
        {
            get { return lastOperation; }
            set { lastOperation = value; }
        }

        public string Result
        {
            get { return calculation.Result; }
        }

        public string Display
        {
            get { return display; }
            set
            {
                display = value;
                OnPropertyChanged("Display");
            }
        }

        public ICommand OperationButtonPressCommand
        {
            get
            {
                if (operationButtonPressCommand == null)
                {
                    operationButtonPressCommand = new RelayCommand(OperationButtonPress);
                }
                return operationButtonPressCommand;
            }
        }

        public ICommand DigitButtonPressCommand
        {
            get
            {
                if (digitButtonPressCommand == null)
                {
                    digitButtonPressCommand = new RelayCommand(DigitButtonPress);
                }
                return digitButtonPressCommand;
            }
        }

        public void DigitButtonPress(object button)
        {
            switch (button)
            {
                case "C":
                    Display = "0";
                    FirstOperand = string.Empty;
                    SecondOperand = string.Empty;
                    Operation = string.Empty;
                    LastOperation = string.Empty;
                    break;
                case "+/-":
                    if (display.Contains("-") || display == "0")
                    {
                        Display = display.Remove(display.IndexOf("-"), 1);
                    }
                    else Display = "-" + display;
                    break;
                case ".":
                    if (newDisplayRequired)
                    {
                        Display = "0,";
                    }
                    else
                    {
                        if (!display.Contains("."))
                        {
                            Display = display + ",";
                        }
                    }
                    break;
                case "%":
                    if (FirstOperand == string.Empty)
                    {
                        Display = "0";
                    }
                    else
                    {
                        Display = ((Convert.ToDouble(FirstOperand)) / 100 * (Convert.ToDouble(display))).ToString();
                        display = Display;
                    }
                    break;
                default:
                    if (display == "0" || newDisplayRequired)
                        Display = button.ToString();
                    else
                        Display = display + button;
                    break;
            }
            newDisplayRequired = false;
        }


        public void OperationButtonPress(object operation)
        {
            try
            {
                if (FirstOperand == string.Empty || LastOperation == "=")
                {
                    FirstOperand = display;
                    LastOperation = operation.ToString();
                }
                else
                {
                    SecondOperand = display;
                    Operation = lastOperation;
                    calculation.CalculateResult();

                    LastOperation = operation.ToString();
                    Display = Result;
                    FirstOperand = display;
                }
                newDisplayRequired = true;
            }
            catch (Exception)
            {
                Display = Result == string.Empty ? "Error has occured" : Result;
            }
        }

    }
}


