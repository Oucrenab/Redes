using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    //m filas n columnas
    //matriz 6 x 7
    //cosa para le spawn
    [Networked] public bool _redTaken { get; set; }

    GameNode[,] _gameBoar = new GameNode[6, 7];
    [SerializeField] NetworkPrefabRef _nodePrefab;
    [SerializeField] Player _playerturn;
    [Networked] public bool GameStarted { get; set; } = false;

    [SerializeField] Light _globalLight;
    [SerializeField] WinCanvas _winCanvas;

    [Networked] Team _turn { get; set; } = Team.Red;
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

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
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
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    void RPC_StartGame()
    {
     //   RPC_StartPlayerCheck();
        if(_error) return;
        //EventManager.Unsubscribe("StartGame", StartGame);
        //EventManager.Subscribe("OnColumnInteract", Dropear);
        //EventManager.Subscribe("OnColumnPoint", Pintar);
        _winCanvas.RPC_TurnOff();
        if (_gameBoar[0, 0] == null)
        {
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
        }
        else
        {
            RPC_ClearGameBoard();
        }

        RPC_LockMouse();

        GameStarted = true;
        Debug.Log($"GameStarted {GameStarted}");

        switch (_turn)
        {
            case Team.Red:
                RPC_ChangeLights(Color.red);
                break;
            case Team.Blue:
                RPC_ChangeLights(Color.blue);
                break;
            case Team.Empty:
                RPC_ChangeLights(Color.white);
                break;
            default:
                break;
        }
    }

    //[Rpc(RpcSources.StateAuthority, targets: RpcTargets.All)]
    //void RPC_StartPlayerCheck()
    //{
    //    FindObjectOfType<PlayerLeftCheck>().StartCheck();
    //}


    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    void RPC_ClearGameBoard()
    {
        for (int i = 0; i < _gameBoar.GetLength(0); i++)
        {
            for (int j = 0; j < _gameBoar.GetLength(1); j++)
            {
                _gameBoar[i, j].RPC_SetTeam(Team.Empty);
            }
        }
        //for (int i = 0; i < 6; i++)
        //{
        //    for (int j = 0; j < 7; j++)
        //    {
        //        Runner.Despawn(_gameBoar[i, j].Object);
        //    }
        //}
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_DropChip(int column, Team team)
    {
        //Debug.Log("DropChip");
        if (_gameBoar[5, column].Team != Team.Empty)
        {
            //DropChip(Random.Range(0, 7), team);
            return;
        }


        for (int i = 0; i < 6; i++)
        {
            if (_gameBoar[i, column].Team == Team.Empty)
            {
                _gameBoar[i, column].RPC_SetTeam(team);

                if (CheckForLine(column, i, team))
                    RPC_GameOver(team);
                break;
            }
        }

        _audioSource.PlayOneShot(_chipDrop);
        RPC_SwitchTeam(team);
    }

    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_SwitchTeam(Team team)
    {
        switch (team)
        {
            case Team.Red:
                _turn = Team.Blue;
                RPC_ChangeLights(Color.blue);
                break;
            case Team.Blue:
                _turn = Team.Red;
                RPC_ChangeLights(Color.red);
                break;
            case Team.Empty:
                RPC_ChangeLights(Color.white);
                break;
            default:
                break;
        }
    }

    [Rpc(sources: RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_ChangeLights(Color color) => _globalLight.color = color;


    [SerializeField] ParticleSystem _redWinsP, _blueWinsP;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _redWinsAs, _blueWinsAs, _chipDrop;

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    void RPC_GameOver(Team team)
    {
        _globalLight.color = Color.white;

        Debug.Log($"<color=green>Linea {team} formada</color>");

        _winCanvas.RPC_WinImage(team);

        switch (team)
        {
            case Team.Red:
                _audioSource.PlayOneShot(_redWinsAs);
                _redWinsP.Play();
                Invoke("RPC_StartGame", 5f);
                break;
            case Team.Blue:
                _audioSource.PlayOneShot(_blueWinsAs);
                _blueWinsP.Play();
                Invoke("RPC_StartGame", 5f);
                break;
            case Team.Empty:
                break;
        }

        RPC_ClearGameBoard();
        //Invoke("RPC_StartGame", 5f);

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

        for (int i = line; i > line - 4 || i > 0; i--)
        {
            if (_gameBoar[i, column].Team != team)
            {
                return false;
                break;
            }

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

    [SerializeField] Canvas _errorCanvas;
    bool _error = false;
    public void PlayerLeft(PlayerRef player)
    {
        Debug.Log("Coño");
        if (Runner.SessionInfo.PlayerCount >= 2) return;

        //RPC_GameOver(Team.Empty);
        //GameStarted = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _errorCanvas.enabled = true;
        GameStarted = false;
        _error = true;
        
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            Application.ExternalEval("window.close();");
#elif UNITY_STANDALONE
            Application.Quit();
#else
            Application.Quit();
#endif
    }

    //public void PlayerLeft(PlayerRef player)
    //{
    //    var p = FindObjectOfType<Player>()._team;

    //    GameOver(p);
    //}
}
