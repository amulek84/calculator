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
    
    public void inputBtn(){
        if(this.output.text.Contains("=") || this.output.text == "Делить на ноль невозможно"){
            this.history.text += this.output.text+"\n";
            this.clearInput();
            this.clearOutput();
        }
        if(this.input.text.Length > 0 && !this.input.text[this.input.text.Length-1].Equals(',')
            && (character.Equals("+") || character.Equals("-") 
            || character.Equals("x") || character.Equals("÷"))) {
            if(this.input.text.Contains("-")){
                this.input.text = "(" + this.input.text + ")";
            }
            this.input.text += character;
            this.output.text += this.input.text;
            this.clearInput();
        } else if(!character.Equals("+") && !character.Equals("-") 
            && !character.Equals("x") && !character.Equals("÷") 
            && !character.Equals(",") 
            && (this.input.text.Length == 0
            || this.input.text.Length == 1 && this.input.text.Equals("0") && float.Parse(character) > 9
            || this.input.text.Length == 1 && !this.input.text.Equals("0")
            || this.input.text.Length > 1 && !this.input.text[0].Equals('-') && !this.input.text[1].Equals('0')
            || this.input.text.Length > 2 && this.input.text[0].Equals('-') && this.input.text[1].Equals('0')
            || this.input.text.Length > 1 && this.input.text[0].Equals('-') && !this.input.text[1].Equals('0')
            || this.input.text.Length > 1 && !this.input.text[0].Equals('-') && !this.input.text[0].Equals('0'))) {
            this.input.text += character;
        } else if(this.input.text.Length > 0 && character.Equals(",") 
            && !this.input.text.Contains(",")){
            this.input.text += character;
        }
    }

    public void negativePositive(){
        if(this.input.text.Length > 0 && this.input.text[0].Equals('-')) {
            this.input.text = this.input.text.Replace("-", "");
        } else if(this.input.text.Length > 0) {
            this.input.text = "-" + this.input.text;
        }
    }

    public void equels(){
        if(this.output.text.Length > 0 && !this.output.text.Contains("=") 
            && this.input.text.Length > 0 && !this.input.text[this.input.text.Length-1].Equals(',')){
            if(this.input.text.Contains("-")){
                this.input.text = "(" + this.input.text + ")";
            }
            this.output.text += this.input.text + character;
            this.getResult(this.output.text);
            this.clearInput();
        }
    }

    public void getResult(string math){
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
                this.expression.Add(math[posMath].ToString());
                posExpress++;
                posMath++;
            }
            this.expression.Add(number);
            posExpress++;
        }
        doOperation("x");
        doOperation("÷");
        if(this.zero){
            this.clearInput();
            this.clearOutput();
            this.output.text = "Делить на ноль невозможно";
            this.zero = false;
        } else {
            doOperation("+");
            doOperation("-");
            this.output.text += this.expression[0];
        }
        this.expression.Clear();
    }

    private void doOperation(string mathOperator){
        for(int i = 0; i < this.expression.Count; i++){            
            if(this.expression[i].Equals(mathOperator)){
                if(mathOperator.Equals("x")){
                    result = float.Parse(this.expression[i-1].Replace("—", "-"))*float.Parse(this.expression[i+1].Replace("—", "-"));
                    this.expression[i-1] = result.ToString();
                }
                if(mathOperator.Equals("÷")){
                    if(float.Parse(this.expression[i+1].Replace("—", "-")) == 0){
                        this.zero = true;
                        break;
                    }
                    result = float.Parse(this.expression[i-1].Replace("—", "-"))/float.Parse(this.expression[i+1].Replace("—", "-"));
                    this.expression[i-1] = result.ToString();
                }
                if(mathOperator.Equals("+")){
                    result = float.Parse(this.expression[i-1].Replace("—", "-"))+float.Parse(this.expression[i+1].Replace("—", "-"));
                    this.expression[i-1] = result.ToString();
                }
                if(mathOperator.Equals("-")){
                    result = float.Parse(this.expression[i-1].Replace("—", "-"))-float.Parse(this.expression[i+1].Replace("—", "-"));
                    this.expression[i-1] = result.ToString();
                }

                this.expression.RemoveAt(i+1);
                this.expression.RemoveAt(i);

                if(this.expression.Count == 1) {
                    this.expression[0] = result.ToString();
                    break;
                }
                i = 0;
            }
        }
    }

    public void clearAll(){
        this.clearInput();
        this.clearOutput();
        clearHistory();
    }

    private void clearHistory(){
        this.history.text = "";
    }

    private void clearInput(){
        this.input.text = "";
    }

    private void clearOutput(){
        this.output.text = "";
    }
}
