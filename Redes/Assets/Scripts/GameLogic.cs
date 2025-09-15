using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    //m filas n columnas
    //matriz 6 x 7

    GameNode[,] _gameBoar = new GameNode[6,7];
    [SerializeField]GameNode _nodePrefab;

    private void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                var pos = new Vector3(j,i);
                _gameBoar[i, j] = Instantiate(_nodePrefab, pos, Quaternion.identity).SetTeam(Team.Empty);
            }
        }
    }

    int dev_team = 0;
    private void Update()
    {
        //if (Input.anyKeyDown)
        //{
        //    var team = Team.Blue;
        //    if (dev_team % 2 == 0)
        //    {
        //        team = Team.Red;
        //        DropChip(0, team);
        //    }
        //    else
        //    {
        //        DropChip(1, team);

        //    }

        //    //print("a");

        //    dev_team++;
        //}

        if (Input.GetKeyDown(KeyCode.Alpha1))
            DropChip(0, Team.Red);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            DropChip(1, Team.Red);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            DropChip(2, Team.Red);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            DropChip(3, Team.Red);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            DropChip(4, Team.Red);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            DropChip(5, Team.Red);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            DropChip(6, Team.Red);

        if (Input.GetKeyDown(KeyCode.Q))
            DropChip(0, Team.Blue);
        if (Input.GetKeyDown(KeyCode.W))
            DropChip(1, Team.Blue);
        if (Input.GetKeyDown(KeyCode.E))
            DropChip(2, Team.Blue);
        if (Input.GetKeyDown(KeyCode.R))
            DropChip(3, Team.Blue);
        if (Input.GetKeyDown(KeyCode.T))
            DropChip(4, Team.Blue);
        if (Input.GetKeyDown(KeyCode.Y))
            DropChip(5, Team.Blue);
        if (Input.GetKeyDown(KeyCode.U))
            DropChip(6, Team.Blue);
    }

    public void DropChip(int column, Team team)
    {
        if (_gameBoar[5,column].Team != Team.Empty)
        {
            DropChip(Random.Range(0, 7), team);
            return;
        }

        for(int i = 0;i < 6; i++)
        {
            if (_gameBoar[i, column].Team == Team.Empty)
            {
                _gameBoar[i, column].SetTeam(team);

                if(CheckForLine(column, i, team))
                {
                    Debug.Log("<color=green>Linea formada</color>");
                }
                break;
            }
        }
        
    }

    public bool CheckForLine(int column, int line, Team team)
    {
        //if (VerticalCheck(column, line, team)) return true;
        //if (HorizontalCheck(column, line, team)) return true;
        if (DiagonalCheck(column, line, team)) return true;
        return false;
    }

    bool VerticalCheck(int column, int line, Team team)
    {
        if (line < 3) return false;

        for (int i = line; i > 0; i--)
        {
            if (_gameBoar[i, column].Team != team) return false;
        }

        return true;
    }

    bool HorizontalCheck(int column, int line, Team team)
    {
        var inLine = 0;
        for (int i = column; i >= 0; i--)
        {
            if (_gameBoar[line, i].Team != team)
                break;

            inLine++;
            if(inLine >= 5) return true;
        }
        for(int i = column; i < 7; i++)
        {
            if (_gameBoar[line, i].Team != team)
                break;

            inLine++;
            if (inLine >= 5) return true;
        }

        return false;
    }

    bool DiagonalCheck(int column, int line, Team team)
    {
        //Debug.Log(DiagonalPositiva(column, line, team));
        //if(DiagonalPositiva(column, line, team)) return true;
        if(DiagonalNegativa(column, line, team)) return true;

        return false;
    }

    bool DiagonalPositiva(int column, int line, Team team)
    {
        int inLine = 1;
        for(int i = 1; i < 5; i++)
        {
            if (column + i >= 7)
            {
                //Debug.Log($"{column} + {i} > {7}");
                break;
            }
            if (line + i >= 6)
            {
                //Debug.Log($"{line} + {i} > {6}");
                break;
            }

            if (_gameBoar[line + i, column + i].Team != team)
                break;

            inLine++;
            if (inLine >= 4) return true;
        }
        for(int i = 1; i < 5; i++)
        {
            if (column - i < 0)
            {
                //Debug.Log($"{column} - {i} < {0}");
                break;
            }
            if (line - i < 0)
            {
                //Debug.Log($"{line} - {i} < {0}");
                break;
            }

            if (_gameBoar[line - i, column - i].Team != team)
                break;

            inLine++;
            if (inLine >= 4) return true;
        }
        //Debug.Log(inLine);

        return false;
    }

    bool DiagonalNegativa(int column, int line, Team team)
    {
        int inLine = 1;
        for (int i = 1; i < 5; i++)
        {
            if (column - i < 0)
            {
                //Debug.Log($"{column} + {i} > {7}");
                break;
            }
            if (line + i >= 6)
            {
                //Debug.Log($"{line} + {i} > {6}");
                break;
            }

            if (_gameBoar[line + i, column - i].Team != team)
                break;

            inLine++;
            if (inLine >= 4) return true;
        }
        for (int i = 1; i < 5; i++)
        {
            if (column + i >= 7)
            {
                //Debug.Log($"{column} - {i} < {0}");
                break;
            }
            if (line - i < 0)
            {
                //Debug.Log($"{line} - {i} < {0}");
                break;
            }

            if (_gameBoar[line - i, column + i].Team != team)
                break;

            inLine++;
            if (inLine >= 4) return true;
        }
        //Debug.Log(inLine);

        return false;
    }
}
