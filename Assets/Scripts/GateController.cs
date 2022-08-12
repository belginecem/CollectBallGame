using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateController : MonoBehaviour
{
    [SerializeField] private enum GateType
    {
        PositiveGate,
        NegativeGate
    }

    [SerializeField] private GateType _gateType;

    [SerializeField] private int _gateNumber;

    [SerializeField] private TMP_Text _gateNumberText = null;

    public int GetGateNumber()
    {
        return _gateNumber;
    }

    void Start()
    {
        RandomGateNumber();
    }

    private void RandomGateNumber()
    {
        switch (_gateType)
        {
            case GateType.PositiveGate: _gateNumber = Random.Range(1, 10);
                _gateNumberText.text = _gateNumber.ToString();
                break;
            case GateType.NegativeGate:;_gateNumber = Random.Range(-10, -1);
                _gateNumberText.text = _gateNumber.ToString();
                break;
        }
    }
}
