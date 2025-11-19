using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ShowSessionName : MonoBehaviour
{
    TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();

        var provider = GetComponentInParent<SessionItemDefinition.IProvider>();
        provider.OnSessionUpdate += Refresh;
    }

    void Refresh(SessionItemDefinition sessionItem)
    {
        _text.text = sessionItem.info.Name;
    }
}
