// See https://aka.ms/new-console-template for more information
Console.WriteLine("Please enter calculation in string");
Console.WriteLine("(Please seperate numbers/operators/brackets by spaces)");
Console.WriteLine("");

while (true)
{
    try
    {
        Console.Write("Question: ");
        string sum = Console.ReadLine();
        Console.WriteLine(string.Format("Total: {0}", Calculate(sum)));
        Console.WriteLine("");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Invalid string entered");
    }
}

double Calculate(string sum)
{
    //Store string into array list
    List<string> arrayList = sum.Split(' ').ToList();

    if (arrayList.Any(s => s == ""))
        throw new Exception();

    while (arrayList.Count > 1)
    {
        while (arrayList.Any(s => s == "("))
        {
            int secondOpenBracketIndex = arrayList.LastIndexOf("(");
            int firstCloseBracketIndex = arrayList.IndexOf(")");

            int openIndex = firstCloseBracketIndex > secondOpenBracketIndex ? secondOpenBracketIndex : arrayList.IndexOf("(");
            int closeIndex = arrayList.IndexOf(")");
            
            List<string> newArrayList = arrayList.Skip(openIndex + 1).Take(closeIndex - openIndex - 1).ToList();

            while (newArrayList.Count > 1)
            {
                if (newArrayList.Any(s => s == "*" || s == "/"))
                {
                    PerformCalculation(ref newArrayList, "*", "/");
                }
                else if (newArrayList.Any(s => s == "+" || s == "-"))
                {
                    PerformCalculation(ref newArrayList, "+", "-");
                }
            }

            // Assign result into original Array List
            arrayList[openIndex] = newArrayList[0];
            RemoveElementFromList(ref arrayList, openIndex + 1, closeIndex);
        }

        if (arrayList.Any(s => s == "*" || s == "/"))
        {
            PerformCalculation(ref arrayList, "*", "/");
        }
        else if (arrayList.Any(s => s == "+" || s == "-"))
        {
            PerformCalculation(ref arrayList, "+", "-");
        }
    }
    
    return double.Parse(arrayList[0]);
}

static void PerformCalculation(ref List<string> arrayList, string firstOperation, string secondOperation)
{
    // Example: + OR *
    int firstOperationIndex = arrayList.IndexOf(firstOperation);
    // Example: - OR /
    int secondOperationIndex = arrayList.IndexOf(secondOperation);

    // Check which operation should complete first
    bool isFirstOperation = firstOperationIndex > 0 && (secondOperationIndex < 0 || firstOperationIndex < secondOperationIndex);
    bool isSecondOperation = secondOperationIndex > 0 && (firstOperationIndex < 0 || secondOperationIndex < firstOperationIndex);

    if (isFirstOperation)
    {
        arrayList[firstOperationIndex - 1] = Calculation(double.Parse(arrayList[firstOperationIndex - 1]), double.Parse(arrayList[firstOperationIndex + 1]), firstOperation).ToString("0.0");
        RemoveElementFromList(ref arrayList, firstOperationIndex, firstOperationIndex + 1);
    }

    if (isSecondOperation)
    {
        arrayList[secondOperationIndex - 1] = Calculation(double.Parse(arrayList[secondOperationIndex - 1]), double.Parse(arrayList[secondOperationIndex + 1]), secondOperation).ToString("0.0");
        RemoveElementFromList(ref arrayList, secondOperationIndex, secondOperationIndex + 1);
    }
}

static double Calculation(double firstnumber, double secondNumber, string arithmeticOperations)
{
    switch (arithmeticOperations)
    {
        case "*":
            return firstnumber * secondNumber;
        case "/":
            return firstnumber / secondNumber;
        case "+":
            return firstnumber + secondNumber;
        case "-":
            return firstnumber - secondNumber;
        default:
            break;
    }
    throw new Exception();
}

static void RemoveElementFromList(ref List<string> arrayList, int fromIndex, int toIndex)
{
    for (int i = toIndex; i >= fromIndex; i--)
        arrayList.RemoveAt(i);
}