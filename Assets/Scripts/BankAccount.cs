using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Bank/Create Account", fileName = "BankAccount", order = 0)]
public class BankAccount : InitializeOnStartSO
{
    public event Action<int> OnUpdateBalance;
    [SerializeField] private int _startBalance;
    [SerializeField] private int _currentBalance;
    public int CurrentBalance => _currentBalance;
    
    public override void Initialize()
    {
        _currentBalance = _startBalance;
        UpdateLabelValue(_currentBalance);
    }

    public void Deposit(int amount)
    {
        if (amount > 0)
        {
            _currentBalance += amount;
            UpdateLabelValue(_currentBalance);
        }
    }

    public bool Withdraw(int amount)
    {
        var balance = _currentBalance - amount;
        
        if (balance >= 0)
        {
            _currentBalance = balance;
            UpdateLabelValue(_currentBalance);
            return true;
        }

        return false;
    }

    private void UpdateLabelValue(int value)
    {
        OnUpdateBalance?.Invoke(value);
    }
}
