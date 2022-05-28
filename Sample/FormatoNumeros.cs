using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormatoNumeros : MonoBehaviour
{

    public static FormatoNumeros Instance {get; private set;}

    protected string begin = "{0:#,0";
    protected string dot = ".00";
    protected string end = "}";
    protected string character = string.Empty;

    protected string res = string.Empty;

    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }


    public string FormatNumber(double numero)
    {

        double num = numero;
        double cuantas_comas = 0;
        double digits = 0;
        string[] scale = new string[]
        {
                "K", "M", "B", "T", "Q", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ",
                "AR", "AS", "AT", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM",
                "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CF", "CG", "CH", "CI", "CJ",
                "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF",
                "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ", "EA",
                "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV",
                "EW", "EX", "EY", "EZ"
        };


        if (num < 1000)
        {
            
            return Math.Floor(num).ToString();
        }
        
        digits = Math.Floor(Math.Log10(num)); 
        StringBuilder sb = new StringBuilder(begin, 310); 
        cuantas_comas = Math.Floor(digits / 3);
        if (cuantas_comas - 1 > scale.Length) return "error"; 
        character = scale[(int)cuantas_comas - 1];

        for (int j = 0; j < cuantas_comas; j++)
        {
            sb.Append(",");
        }

        sb.Append(dot);
        sb.Append(character);
        sb.Append(end);
        
        res = sb.ToString();

        string a = string.Format(res, num);
        return a;

    }
}
