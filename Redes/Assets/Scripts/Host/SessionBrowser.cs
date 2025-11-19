using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionBrowser : MonoBehaviour
{
    [SerializeField] RunnerHandler _runnerhandler;
    [SerializeField] SessionItemPresenter _itemPrefab;
    [SerializeField] VerticalLayoutGroup _content;//a

    [SerializeField] TMP_Text _statusText;

    private void OnEnable()
    {
        _runnerhandler.OnSessionsUpdate += UpdateList;
    }

    private void OnDisable()
    {
        _runnerhandler.OnSessionsUpdate -= UpdateList;
    }

    void UpdateList(List<SessionInfo> list)
    {
        ClearContent();

        if(list.Count == 0)
        {
            _statusText.gameObject.SetActive(true);
            return;
        }

        _statusText.gameObject.SetActive(false);


        foreach (var info in list) 
        {
            var i = Instantiate(_itemPrefab, _content.transform);
            i.Initialize(info, _runnerhandler);
        }
    }

    void ClearContent()
    {
        foreach(Transform child in _content.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
