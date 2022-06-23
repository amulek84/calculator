using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Calculator : MonoBehaviour
{   
    private bool zero = false;
    private float result;
    private string[] numbers;
    [SerializeField] private string character;
    [SerializeField] public TextMeshProUGUI input;
    [SerializeField] public TextMeshProUGUI output;
    [SerializeField] public TextMeshProUGUI history;
    private char[] operators = {'+', '-', 'x', '÷'};
    List<string> expression = new List<string>();
    
    public void InputBtn(){
        if(output.text.Contains("=") || output.text == "Делить на ноль невозможно"){
            history.text += output.text+"\n";
            ClearInput();
            ClearOutput();
        }
        if(output.text.Length > 0 && input.text.Length == 0 && (character.Equals("+") || character.Equals("-") 
            || character.Equals("x") || character.Equals("÷"))){
            output.text = output.text.Substring(0, output.text.Length - 1);
            output.text += character;
        }
        if(input.text.Length > 0 && !input.text[input.text.Length-1].Equals(',')
            && (character.Equals("+") || character.Equals("-") 
            || character.Equals("x") || character.Equals("÷"))) {
            if(input.text.Contains("-")){
                input.text = "(" + input.text + ")";
            }
            input.text += character;
            output.text += input.text;
            ClearInput();
        } else if(!character.Equals("+") && !character.Equals("-") 
            && !character.Equals("x") && !character.Equals("÷") 
            && !character.Equals(",") 
            && (input.text.Length == 0
            || input.text.Length == 1 && input.text.Equals("0") && float.Parse(character) > 9
            || input.text.Length == 1 && !input.text.Equals("0")
            || input.text.Length > 1 && !input.text[0].Equals('-') && !input.text[1].Equals('0')
            || input.text.Length > 2 && input.text[0].Equals('-') && input.text[1].Equals('0')
            || input.text.Length > 1 && input.text[0].Equals('-') && !input.text[1].Equals('0')
            || input.text.Length > 1 && !input.text[0].Equals('-') && !input.text[0].Equals('0'))) {
            input.text += character;
        } else if(input.text.Length > 0 && character.Equals(",") 
            && !input.text.Contains(",")){
            input.text += character;
        }
    }

    public void NegativePositive(){
        if(input.text.Length > 0 && input.text[0].Equals('-')) {
            input.text = input.text.Replace("-", "");
        } else if(input.text.Length > 0) {
            input.text = "-" + input.text;
        }
    }

    public void Equels(){
        if(output.text.Length > 0 && !output.text.Contains("=") 
            && input.text.Length > 0 && !input.text[input.text.Length-1].Equals(',')){
            if(input.text.Contains("-")){
                input.text = "(" + input.text + ")";
            }
            output.text += input.text + character;
            GetResult(output.text);
            ClearInput();
        }
    }

    public void GetResult(string math){
        math = math.Replace("=", "");
        math = math.Replace("(-", "—");
        math = math.Replace(")", "");
        numbers = math.Split(operators);
        foreach(var number in numbers){
            math = math.Replace(number, "");
        }
        var posExpress = 0;
        var posMath = 0;
        foreach(var number in numbers){
            if((posExpress+1)%2 == 0){
                expression.Add(math[posMath].ToString());
                posExpress++;
                posMath++;
            }
            expression.Add(number);
            posExpress++;
        }
        DoOperation("x");
        DoOperation("÷");
        if(zero){
            ClearInput();
            ClearOutput();
            output.text = "Делить на ноль невозможно";
            zero = false;
        } else {
            DoOperation("+");
            DoOperation("-");
            output.text += expression[0];
        }
        expression.Clear();
    }

    private void DoOperation(string mathOperator){
        for(int i = 0; i < expression.Count; i++){            
            if(expression[i].Equals(mathOperator)){
                if(mathOperator.Equals("x")){
                    result = float.Parse(expression[i-1].Replace("—", "-"))*float.Parse(expression[i+1].Replace("—", "-"));
                    expression[i-1] = result.ToString();
                }
                if(mathOperator.Equals("÷")){
                    if(float.Parse(expression[i+1].Replace("—", "-")) == 0){
                        zero = true;
                        break;
                    }
                    result = float.Parse(expression[i-1].Replace("—", "-"))/float.Parse(expression[i+1].Replace("—", "-"));
                    expression[i-1] = result.ToString();
                }
                if(mathOperator.Equals("+")){
                    result = float.Parse(expression[i-1].Replace("—", "-"))+float.Parse(expression[i+1].Replace("—", "-"));
                    expression[i-1] = result.ToString();
                }
                if(mathOperator.Equals("-")){
                    result = float.Parse(expression[i-1].Replace("—", "-"))-float.Parse(expression[i+1].Replace("—", "-"));
                    expression[i-1] = result.ToString();
                }

                expression.RemoveAt(i+1);
                expression.RemoveAt(i);

                if(expression.Count == 1) {
                    expression[0] = result.ToString();
                    break;
                }
                i = 0;
            }
        }
    }

    public void ClearAll(){
        ClearInput();
        ClearOutput();
        ClearHistory();
    }

    private void ClearHistory(){
        history.text = "";
    }

    private void ClearInput(){
        input.text = "";
    }

    private void ClearOutput(){
        output.text = "";
    }
}