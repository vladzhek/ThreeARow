using System;
using System.Collections.Generic;

namespace Services
{
    public class CurrencyService
    {
        public int SoftCoins => _currency[CurrencyType.Soft];
        public event Action<CurrencyType, int> OnSpend;
        public event Action<CurrencyType, int> OnCollect;
        public event Action OnUpdate;
        
        private Dictionary<CurrencyType, int> _currency = new();

        public CurrencyService()
        {
            _currency.Add(CurrencyType.Soft, 0);
            _currency.Add(CurrencyType.Hard, 0);
        }

        public void AddCurrency(CurrencyType type, int amount)
        {
            _currency[type] += amount;
            OnCollect?.Invoke(type, amount);
            OnUpdate?.Invoke();
            
        }

        public bool SpendCurrency(CurrencyType type, int amount)
        {
            if (amount > SoftCoins)
            {
                return false;
            }
            
            _currency[type] -= amount;
            OnSpend?.Invoke(type, amount);
            OnUpdate?.Invoke();
            
            return true;
        }
    }
}