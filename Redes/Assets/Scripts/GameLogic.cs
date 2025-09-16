using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : NetworkBehaviour
{
    //m filas n columnas
    //matriz 6 x 7

    GameNode[,] _gameBoar = new GameNode[6,7];
    [SerializeField] NetworkPrefabRef _nodePrefab;

    private void Start()
    {
        EventManager.Subscribe("OnColumnInteract", Dropear);
        EventManager.Subscribe("OnColumnPoint", Pintar);

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                var pos = new Vector3(j, i + 0.5f) * 1.25f;
                //_gameBoar[i, j] = Instantiate(_nodePrefab, pos, Quaternion.identity).SetTeam(Team.Empty);
                _gameBoar[i, j] = Runner.Spawn(_nodePrefab, pos, Quaternion.identity)
                    .GetComponent<GameNode>().SetTeam(Team.Empty);
            }
        }
    }

    //public override void FixedUpdateNetwork()
    //{
    //    //if (Input.anyKeyDown)
    //    //{
    //    //    var team = Team.Blue;
    //    //    if (dev_team % 2 == 0)
    //    //    {
    //    //        team = Team.Red;
    //    //        DropChip(0, team);
    //    //    }
    //    //    else
    //    //    {
    //    //        DropChip(1, team);

    //    //    }

    //    //    //print("a");

    //    //    dev_team++;
    //    //}

    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //        DropChip(0, Team.Red);
    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //        DropChip(1, Team.Red);
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //        DropChip(2, Team.Red);
    //    if (Input.GetKeyDown(KeyCode.Alpha4))
    //        DropChip(3, Team.Red);
    //    if (Input.GetKeyDown(KeyCode.Alpha5))
    //        DropChip(4, Team.Red);
    //    if (Input.GetKeyDown(KeyCode.Alpha6))
    //        DropChip(5, Team.Red);
    //    if (Input.GetKeyDown(KeyCode.Alpha7))
    //        DropChip(6, Team.Red);

    //    if (Input.GetKeyDown(KeyCode.Q))
    //        DropChip(0, Team.Blue);
    //    if (Input.GetKeyDown(KeyCode.W))
    //        DropChip(1, Team.Blue);
    //    if (Input.GetKeyDown(KeyCode.E))
    //        DropChip(2, Team.Blue);
    //    if (Input.GetKeyDown(KeyCode.R))
    //        DropChip(3, Team.Blue);
    //    if (Input.GetKeyDown(KeyCode.T))
    //        DropChip(4, Team.Blue);
    //    if (Input.GetKeyDown(KeyCode.Y))
    //        DropChip(5, Team.Blue);
    //    if (Input.GetKeyDown(KeyCode.U))
    //        DropChip(6, Team.Blue);
    //}

    public void PreviewChip(int column, Team team)
    {
        for (int i = 0; i < 7; i++)
        {
            if (_gameBoar[5, i].Team != Team.Empty)
                continue;

            _gameBoar[5, i].SetColor(Team.Empty);

        }
        try
        {
            _gameBoar[5, column].SetColor(team);
        }
        catch
        {
            Debug.Log(column);
        }
    }

    public void DropChip(int column, Team team)
    {
        if (_gameBoar[5,column].Team != Team.Empty)
        {
            //DropChip(Random.Range(0, 7), team);
            return;
        }

        for(int i = 0;i < 6; i++)
        {
            if (_gameBoar[i, column].Team == Team.Empty)
            {
                _gameBoar[i, column].SetTeam(team);

                if(CheckForLine(column, i, team))
                {
                    Debug.Log($"<color=green>Linea {team} formada</color>");
                }
                break;
            }
        }

        switch (team)
        {
            case Team.Red:
                _turn = Team.Blue;
                break;
            case Team.Blue:
                _turn = Team.Red;
                break;
            case Team.Empty:
                break;
            default:
                break;
        }
    }

    public bool CheckForLine(int column, int line, Team team)
    {
        if (VerticalCheck(column, line, team)) return true;
        if (HorizontalCheck(column, line, team)) return true;
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
        if(DiagonalPositiva(column, line, team)) return true;
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

    Team _turn = Team.Red;

    void Dropear(params object[] _vars)
    {
        var column = (int)_vars[0];
        var team = (Team)_vars[1];
        if (team != _turn) return;

        DropChip(column, team);
    }
    void Pintar(params object[] _vars)
    {
        var column = (int)_vars[0];
        var team = (Team)_vars[1];
        if (team != _turn) return;


        PreviewChip(column, team);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnColumnInteract", Dropear);
        EventManager.Unsubscribe("OnColumnPoint", Pintar);
    }
}
