using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GT
{
    public class ATM : Singletone<ATM>
    {
        [Header("ATM")]
        [SerializeField] Button ATMBtn;
        [SerializeField] Transform ATMSystem;
        [SerializeField] InputField amountOfMoney;
        [SerializeField] Button deposit,withdraw;
        [SerializeField] Text operationStatus;
        public Action OnOperationFinished;
        Player player;
        private void Awake()
        {
            if (ATMSystem != null)
            {
                ATMSystem.gameObject.SetActive(false);
            }
            player = FindObjectOfType<Player>();
            deposit.onClick.AddListener(delegate { Deposit(amountOfMoney); });
            withdraw.onClick.AddListener(delegate { Withdrawal(amountOfMoney); });

            OnOperationFinished += player.Init;
            OnOperationFinished += delegate { amountOfMoney.text = ""; };
            UIPanelsActions.action += CloseATMSystem;
            ATMBtn.onClick.AddListener(delegate { 
                UIPanelsActions.action?.Invoke();
                ATMSystem.gameObject.SetActive(true);
            });
        }
        void CloseATMSystem()
        {
            ATMSystem.gameObject.SetActive(false);
        }
        public void Deposit(InputField amount)
        {
            float depositMoney = float.Parse(amount.text == "-" || amount.text.Length == 0 ?"0":amount.text);
            if(depositMoney <= player.coins)
            {
                player.bankBalance += depositMoney;
                player.coins -= depositMoney;
                operationStatus.text = "You have successfully deposited your money";
            }
            else
            {
                operationStatus.text = "unsucceesfull operation , you haven't this amount to deposit";
            }
            OnOperationFinished?.Invoke();
        }
        public void Withdrawal(InputField amount)
        {
            float withdrawMoney = float.Parse(amount.text == "-" || amount.text.Length == 0 ? "0" : amount.text);
            if (withdrawMoney <= player.bankBalance)
            {
                player.bankBalance -= withdrawMoney;
                player.coins += withdrawMoney;
                operationStatus.text = "You have successfully withdrawn your money";
            }
            else
            {
                operationStatus.text = "unsucceesfull operation , you haven't this amount to withdraw";
            }
            OnOperationFinished?.Invoke();
        }
    } //end of class
}

