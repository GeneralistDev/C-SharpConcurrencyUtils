using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodgyBrothers
{
    class Account
    {
        private decimal balance;
        private Object balanceLock;

        public Account(decimal beginningBalance)
        {
            balanceLock = new Object();
            balance = beginningBalance;
        }

        public Account(): this(0m) {}

        // Deposits decimal amount to account balance and prints the new balance
        public decimal Deposit(decimal amount)
        {
            lock (balanceLock)
            {
                balance += amount;
                return getBalance();
            }
        }

        // Withdraws decimal amount from balance and prints new balance
        public decimal Withdraw(decimal amount)
        {
            lock (balanceLock)
            {
                balance -= amount;
                return getBalance();
            }
        }

        public decimal getBalance()
        {
            lock (balanceLock)
            {
                return balance;
            }
        }
    }
}
