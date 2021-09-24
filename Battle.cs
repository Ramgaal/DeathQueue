using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Unit
{
    public int Fraction;
    public int Position;
    public string Name;
    public int Initiative;
    public int Speed;

    public Unit(int fraction, int position, string name,int initiative, int speed)
	{
        Fraction = fraction;
		Position = position;
		Name = name;
		Initiative = initiative;
		Speed = speed;
	}
}

public class Battle : MonoBehaviour
{
    public static GameObject Image;
    public bool Attack = false;
    public bool Skip = false;
    public GameObject UnitPosition;
    int SelectUnit = 0;
    public uint Turn = 1;
    Unit[] UnitList = new Unit[14];

    public Canvas canvas;

    int UnitCounterBlue = 7;
    int UnitCounterRed = 7;

    bool EndBattle = false;

    Unit unit_temp = new Unit(-1, 0, "Def", 0, 0);
    Unit unit_def = new Unit(-1, 0, "Def", 0, 0);

    Unit unit_k1 = new Unit(0, 1, "K1", 8, 4);
    Unit unit_k2 = new Unit(0, 2, "K2", 8, 4);
    Unit unit_k3 = new Unit(0, 3, "K3", 9, 5);
    Unit unit_k4 = new Unit(0, 4, "K4", 4, 3);
    Unit unit_k5 = new Unit(0, 5, "K5", 3, 3);
    Unit unit_k6 = new Unit(0, 6, "K6", 2, 4);
    Unit unit_k7 = new Unit(0, 7, "K7", 1, 1);

    Unit unit_c1 = new Unit(1, 1, "C1", 6, 6);
    Unit unit_c2 = new Unit(1, 2, "C2", 8, 5);
    Unit unit_c3 = new Unit(1, 3, "C3", 9, 5);
    Unit unit_c4 = new Unit(1, 4, "C4", 8, 4);
    Unit unit_c5 = new Unit(1, 5, "C5", 2, 3);
    Unit unit_c6 = new Unit(1, 6, "C6", 4, 2);
    Unit unit_c7 = new Unit(1, 7, "C7", 1, 1);

    
    public GameObject TurnPlate;
    public GameObject UnitPlate;
    public GameObject Frame;

    int TurnCounter = 0;
    int SelectCounter = 0;

    GameObject[] TurnPanel;

    void Awake()
    {

        UnitList[00] = unit_k1;
        UnitList[01] = unit_k2;
        UnitList[02] = unit_k3;
        UnitList[03] = unit_k4;
        UnitList[04] = unit_k5;
        UnitList[05] = unit_k6;
        UnitList[06] = unit_k7;

        UnitList[07] = unit_c1;
        UnitList[08] = unit_c2;
        UnitList[09] = unit_c3;
        UnitList[10] = unit_c4;
        UnitList[11] = unit_c5;
        UnitList[12] = unit_c6;
        UnitList[13] = unit_c7;

        UnitSort();
        SetFrame();
    }

    public void ButtonAttack()
    {
        Attack = true;
    }

    public void ButtonSkip()
    {
        Skip = true;
    }

    void Update()
    { 
        if (UnitCounterBlue != 0 && UnitCounterRed != 0)
        {
            if (Skip == Attack)
            { }
            else
            {
                if (Attack || Input.GetKeyDown(KeyCode.Return))
                {
                    Debug.Log(UnitList[SelectUnit].Name + " атакует!");
                    ActionAttack();
                    UnitSelector();
                    SetFrame();
                }

                if (Skip || Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log(UnitList[SelectUnit].Name + " Пропускает ход!");
                    ActionSkip();
                    UnitSelector();
                    SetFrame();
                }
            }
        }
        else
        {
            if (!EndBattle)
            {
                if (UnitCounterBlue == 0)
                {
                    Debug.Log("Красные победили!");
                }
                else
                {
                    Debug.Log("Синие победили!");
                }
                Debug.Log("Нажмите ESC для выхода или Backspace для перезагрузки сцены");
                EndBattle = true;

            }
            else
            {
                WaitExit();
            }
        }
    }

    void ActionAttack()
    {
        if (SelectUnit< UnitList.Length-1)
        {
            unit_temp = UnitList[SelectUnit + 1];
            UnitList[SelectUnit + 1] = unit_def;
        }
        else
        {
            unit_temp = UnitList[0]; 
            UnitList[0] = unit_def;
        }
        Debug.Log(unit_temp.Name + " погибает!");
        if (unit_temp.Fraction == 0)
        {
            UnitCounterRed = UnitCounterRed - 1;
        }
        else
        {
            UnitCounterBlue = UnitCounterBlue - 1;
        }

        for (int x = 0; x <= UnitList.Length - 1; x++)
        {
            if (UnitList[x] == unit_def)
            {
                UnitList[x] = UnitList[UnitList.Length - 1];
                Unit[] UnitListTemp = new Unit[UnitList.Length - 1];
                for (int y = 0; y <= UnitListTemp.Length - 1; y++)
                {
                    UnitListTemp[y] = UnitList[y];
                }
                UnitList = new Unit[UnitListTemp.Length];
                for (int y = 0; y <= UnitListTemp.Length - 1; y++)
                {
                    UnitList[y] = UnitListTemp[y];
                }
            }
        }
        unit_temp = UnitList[SelectUnit];
        for (int x = SelectUnit; x <= UnitList.Length - 2; x++)
        {
            UnitList[x] = UnitList[x + 1];
        }
        UnitList[UnitList.Length - 1] = unit_temp;


        return;
    }

    void ActionSkip()
    {
        
        unit_temp = UnitList[SelectUnit];
        for (int x = SelectUnit; x<= UnitList.Length - 2;x++)
        {
            UnitList[x] = UnitList[x + 1];
        }
        UnitList[UnitList.Length - 1] = unit_temp;
        return;
    }

    void UnitSelector()
    {
        SelectUnit = 0; 
         
        SelectCounter = SelectCounter + 1;
        if (SelectCounter > UnitList.Length-1)
        {
            SelectCounter = 0;
            Turn = Turn++;
        }
        Debug.Log("Ход " + UnitList[SelectUnit].Name);
        Attack = false;
        Skip = false;
        return;
    } 

    void UnitSort()
    {
        for (int x = 0; x<= UnitList.Length-1;x++)
        {
            for (int y = x + 1; y < UnitList.Length; y++)
            {
                if (UnitList[x].Initiative == UnitList[y].Initiative)
                {
                    if (UnitList[x].Speed == UnitList[y].Speed)
                    {
                        if (UnitList[x].Fraction != UnitList[y].Fraction)
                        {
                            if (Turn%2==0)
                            {
                                if (UnitList[x].Fraction == 1)
                                {
                                    unit_temp = UnitList[x];
                                    UnitList[x] = UnitList[y];
                                    UnitList[y] = unit_temp;
                                }
                            }

                        }
                        else
                        {
                            if (UnitList[x].Position < UnitList[y].Position)
                            {
                                unit_temp = UnitList[x];
                                UnitList[x] = UnitList[y];
                                UnitList[y] = unit_temp;
                            }
                        }
                    }
                    else
                    {
                        if (UnitList[x].Speed < UnitList[y].Speed)
                        {
                            unit_temp = UnitList[x];
                            UnitList[x] = UnitList[y];
                            UnitList[y] = unit_temp;
                        }
                    }
                }
                else
                {
                    if (UnitList[x].Initiative < UnitList[y].Initiative)
                    {
                        unit_temp = UnitList[x];
                        UnitList[x] = UnitList[y];
                        UnitList[y] = unit_temp;
                    }
                }
            }
            
        }
        SetFrame();
        return;
    }

    void SetFrame()
    {
        if (TurnPanel != null)
        {
            for (int x = 0; x < TurnPanel.Length; x++)
            {
                Destroy(TurnPanel[x]);
            }
        }
        TurnCounter = 0;

        int x_temp = 0;
        int TurnPanelSeted = 0;
        int z = 20;
        while(UnitList.Length <= z)
        {
            z = z - UnitList.Length;
            TurnCounter = TurnCounter = +1;
        }
        
        TurnPanel = new GameObject[21 + TurnCounter];

        int f = 0;
        for (int x = 0; x<TurnPanel.Length;x++)
        {
            if (x == 0 || x == UnitList.Length*TurnPanelSeted +1 && x!=1)
            {
                TurnPanel[x] = Instantiate(TurnPlate, new Vector3(376, 700 - 32 * x, 0), Quaternion.identity);
                TurnPanelSeted = TurnPanelSeted + 1;
                TurnPanel[x].GetComponentInChildren<Text>().text = "Раунд " + Turn + TurnPanelSeted;
                TurnPanel[x].transform.SetParent(canvas.transform);
            }
            else
            {
                
                //Debug.Log(" calculate f=" + f);
                if (x < UnitList.Length)
                {
                    x_temp = f;
                }
                else
                {
                    x_temp = f % UnitList.Length;
                    
                }
                f = f + 1;
                //Debug.Log("x=" + x + ", x_temp=" + x_temp + ", f=" + f + ", f% UnitList.Length=" + (f % UnitList.Length) + ", TurnPanelSeted=" + TurnPanelSeted + ", UnitList.Length" + UnitList.Length + ", TurnPanel.Length=" + TurnPanel.Length);
                TurnPanel[x] = Instantiate(UnitPlate, new Vector3(300, 700 - 32 * x, 0), Quaternion.identity);
                TurnPanel[x].GetComponentInChildren<Text>().text = " " + (x +1 - TurnPanelSeted);
                switch (UnitList[x_temp].Fraction)
                {
                    case 0:
                        TurnPanel[x].GetComponent<Image>().color = Color.red;
                        break;
                    case 1:
                        TurnPanel[x].GetComponent<Image>().color = Color.blue;
                        break;
                }
                TurnPanel[x].transform.SetParent(canvas.transform);
                GameObject Frame_temp = Instantiate(Frame, new Vector3(300, 700 - 32 * x, 0), Quaternion.identity);
                Frame_temp.transform.SetParent(TurnPanel[x].transform);
                Frame_temp.transform.position = Frame_temp.transform.position + new Vector3(100, 0, 0);
                Frame_temp.GetComponentInChildren<Text>().text = "Существо " + UnitList[x_temp].Name + "/n" + "Инициатива: " + UnitList[x_temp].Initiative + "  Скорость: " + UnitList[x_temp].Speed;

            }
        }      
    }

    void WaitExit()
    {
        Console.ReadKey();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                SceneManager.LoadScene(0);
            }
    }
}