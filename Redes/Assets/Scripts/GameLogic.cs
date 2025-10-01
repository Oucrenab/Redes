using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : NetworkBehaviour, IPlayerJoined
{
    //m filas n columnas
    //matriz 6 x 7
    //cosa para le spawn
    [Networked] public bool _redTaken { get; set; }

    GameNode[,] _gameBoar = new GameNode[6,7];
    [SerializeField] NetworkPrefabRef _nodePrefab;
    [SerializeField] Player _playerturn;
    [Networked] public bool GameStarted { get; set; } = false;

    [SerializeField] Light _globalLight;
    [SerializeField] WinCanvas _winCanvas;

    Team _turn = Team.Red;
    //public static GameLogic Instance { get; private set; }

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

    [Rpc]
    public void RPC_PreviewChip(int column, Team team)
    {
        //Debug.Log("PreviewChip");
        for (int i = 0; i < 7; i++)
        {
            if (_gameBoar[5, i].Team != Team.Empty)
                continue;

            _gameBoar[5, i].RPC_SetColor(Team.Empty);

        }
        _gameBoar[5, column].RPC_SetColor(team);


    }

    //void StartGame(params object[] noUse) => RPC_StartGame();
    [Rpc]
    void RPC_StartGame()
    {
        //EventManager.Unsubscribe("StartGame", StartGame);
        //EventManager.Subscribe("OnColumnInteract", Dropear);
        //EventManager.Subscribe("OnColumnPoint", Pintar);

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                var pos = new Vector3(j, i + 0.5f) * 1.25f;
                //_gameBoar[i, j] = Instantiate(_nodePrefab, pos, Quaternion.identity).SetTeam(Team.Empty);
                _gameBoar[i, j] = Runner.Spawn(_nodePrefab, Vector3.zero, Quaternion.identity)
                    .GetComponent<GameNode>();
                _gameBoar[i, j].transform.position = pos;
                _gameBoar[i, j].RPC_SetTeam(Team.Empty);
            }
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameStarted = true;
        Debug.Log($"GameStarted {GameStarted}");

        switch (_turn)
        {
            case Team.Red:
                _globalLight.color = Color.red;
                break;
            case Team.Blue:
                _globalLight.color = Color.blue;
                break;
            case Team.Empty:
                _globalLight.color = Color.white;
                break;
        }

    }

    [Rpc]
    void RPC_ClearGameBoard()
    {
        for(int i = 0;i < _gameBoar.GetLength(0); i++)
        {
            for (int j = 0; j < _gameBoar.GetLength(1); j++)
            {
                _gameBoar[i, j].RPC_SetTeam(Team.Empty);
            }
        }
    }

    [Rpc]
    public void RPC_DropChip(int column, Team team)
    {
        //Debug.Log("DropChip");
        if (_gameBoar[5,column].Team != Team.Empty)
        {
            //DropChip(Random.Range(0, 7), team);
            return;
        }


        for(int i = 0;i < 6; i++)
        {
            if (_gameBoar[i, column].Team == Team.Empty)
            {
                _gameBoar[i, column].RPC_SetTeam(team);

                if (CheckForLine(column, i, team))
                    GameOver(team);
                break;
            }
        }

        SwitchTeam(team);
    }


    void SwitchTeam(Team team)
    {
        switch (team)
        {
            case Team.Red:
                _turn = Team.Blue;
                _globalLight.color = Color.blue;
                break;
            case Team.Blue:
                _turn = Team.Red;
                _globalLight.color = Color.red;
                break;
            case Team.Empty:
                _globalLight.color = Color.white;
                break;
            default:
                break;
        }
    }

    [SerializeField] ParticleSystem _redWins, _blueWins;

    void GameOver(Team team)
    {
        _globalLight.color = Color.white;

        Debug.Log($"<color=green>Linea {team} formada</color>");

        _winCanvas.RPC_WinImage(team);

        if (team == Team.Red)
            _redWins.Play();
        else
            _blueWins.Play();

        RPC_ClearGameBoard();
        Invoke("RPC_StartGame", 5f);

        GameStarted = false;
    }

    #region Line Checks
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
            if (inLine >= 5) return true;
        }
        for (int i = column; i < 7; i++)
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
        if (DiagonalPositiva(column, line, team)) return true;
        if (DiagonalNegativa(column, line, team)) return true;

        return false;
    }

    bool DiagonalPositiva(int column, int line, Team team)
    {
        int inLine = 1;
        for (int i = 1; i < 5; i++)
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
        for (int i = 1; i < 5; i++)
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
    #endregion


    //public void Dropear(params object[] _vars) => RPC_Dropear((int)_vars[0], (Team)_vars[1]);

    public void Dropear(int i, Team t)
    {
        if (!GameStarted) return;

        var column = i;
        var team = t;
        if (team != _turn) return;
        //Debug.Log("Dropear");
        RPC_DropChip(column, team);
    }

    //public void Pintar(params object[] _vars) => RPC_Pintar((int)_vars[0], (Team)_vars[1]);

    public void Pintar(int i , Team t)
    {
        if (!GameStarted) return;
        var column = i;
        var team = t;
        if (team != _turn) return;

        //Debug.Log("Pintar");

        RPC_PreviewChip(column, team);
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.SessionInfo.PlayerCount >= 2 && !GameStarted)
            RPC_StartGame();
    }

    //public void PlayerLeft(PlayerRef player)
    //{
    //    var p = FindObjectOfType<Player>()._team;

    //    GameOver(p);
    //}
}
