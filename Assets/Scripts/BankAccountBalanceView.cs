using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class BankAccountBalanceView : MonoBehaviour
{
    [SerializeField] private BankAccount _account;
    private TMP_Text _textLabel;

    private void Awake()
    {
        _textLabel = GetComponent<TMP_Text>();
        // UpdateBalanceLabel(_account.CurrentBalance);
    }

    private void OnEnable()
    {
        if (_account) _account.OnUpdateBalance += UpdateBalanceLabel;
    }

    private void OnDisable()
    {
        if (_account) _account.OnUpdateBalance -= UpdateBalanceLabel;
    }

    private void UpdateBalanceLabel(int balance)
    {
        _textLabel.text = balance.ToString();
    }
}
