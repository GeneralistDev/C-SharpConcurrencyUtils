using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;

namespace DodgyBrothers
{
    class Bank
    {
        private Dictionary<String, Account> bankAccounts = null;
        private Object lockBank;

        public Bank()
        {
            lockBank = new Object();
            bankAccounts = new Dictionary<string, Account>();
        }

        public Boolean accountExists(String name)
        {
            lock (lockBank)
            {
                if (bankAccounts.ContainsKey(name))
                    return true;
                else
                    return false;
            }
        }

        public void addAccount(String name)
        {
            lock (lockBank)
            {
                if (accountExists(name))
                    throw new Exception("Account already exists");
                else
                    bankAccounts.Add(name, new Account(0m));
            }
        }

		public void deposit(String name, Decimal amount)
		{
		    lock (lockBank)
		    {
		        if (accountExists(name))
		            bankAccounts[name].Deposit(amount);
		        else
		            throw new Exception("Account does not exist");
		    }
		}

		public void withdraw(String name, Decimal amount)
		{
		    lock (lockBank)
		    {
		        if (accountExists(name))
		            bankAccounts[name].Withdraw(amount);
		        else
		            throw new Exception("Account does not exist");
		    }
		}

		public Decimal getBalance(String name)
		{
		    lock (lockBank)
		    {
		        if (accountExists(name))
		            return bankAccounts[name].getBalance();
		        else
		            throw new Exception("Account does not exist");
		    }
		}
    }

    class Program
    {
        static void Main(string[] args)
        {
            Bank dodgyBank = new Bank();
            TcpListener server = null;
            List<Thread> connectionThreads = new List<Thread>();
            
            try
            {
                // Network Details
                IPAddress ipAddress = IPAddress.Parse("0.0.0.0");
                Int32 port = 9000;

                // Create TCP Listener
                server = new TcpListener(ipAddress, port);
                server.Start();
                while (true)
                {
                    Console.WriteLine("Waiting for connection...");
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream clientStream = client.GetStream();
                    Thread newThread = new Thread(() => handleConnection(clientStream, dodgyBank, client));
                    newThread.Start();
                    connectionThreads.Add(newThread);
                    Console.WriteLine("Connection made");
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public static void handleConnection(NetworkStream clientStream, Bank bank, TcpClient client)
        {
            Byte[] bytes = new Byte[256];
            String data = null;
            sendResponse(clientStream, "DODGY BROTHERS ATM\r\n------------------------------------------\r\nSyntax: <COMMAND> <ARGUMENTS>\r\nCreate a new account:\r\n\tCREATE <account name>\r\nSee account balance:\r\n\tBALANCE <account name>\r\nDeposit funds to account:\r\n\tDEPOSIT <account name> <amount>\r\nWithdraw funds from account:\r\n\tWITHDRAW <account name> <amount>");
            data = null;
            int i;
            while ((i = clientStream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                data = data.ToUpper();
                data = Regex.Replace(data, @"\t|\n|\r", "");
                char[] separator = { ' ' };
                String[] parsedCommand = new String[4];
                parsedCommand = data.Split(separator, 3, StringSplitOptions.None);
                if (data == "")
                    parsedCommand[0] = "";
                switch (parsedCommand[0])
                {
                    case "CREATE":
                        if (isCommandComplete(parsedCommand, 2))
                        {
                            try {
                                bank.addAccount(parsedCommand[1]);
                                sendResponse(clientStream, "Account created successfully");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                sendResponse(clientStream, "Account already exists");
                            }
                            break;
                        }
                        else
                        {
                            sendResponse(clientStream, "Command Missing arguments");
                            break;
                        }

                    case "BALANCE":
                        if (isCommandComplete(parsedCommand, 2))
                        {
                            try{
                                String balance = "Account Balance: $" + bank.getBalance(parsedCommand[1]).ToString();
                                sendResponse(clientStream, balance);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                sendResponse(clientStream, "Account does not exist");
                            }
                            break;
                        }
                        else
                        {
                            sendResponse(clientStream, "Command missing arguments");
                            break;
                        }
                    case "DEPOSIT":
                        if (isCommandComplete(parsedCommand, 3))
                        {
                            try {
                                bank.deposit(parsedCommand[1], decimal.Parse(parsedCommand[2]));
                                sendResponse(clientStream, "Deposit Successful");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                sendResponse(clientStream, "Account does not exist");
                            }
                            break;
                        }
                        else
                        {
                            sendResponse(clientStream, "Command missing arguments");
                            break;
                        }

                    case "WITHDRAW":
                        if (isCommandComplete(parsedCommand, 3))
                        {
                            try
                            {
                                bank.withdraw(parsedCommand[1], decimal.Parse(parsedCommand[2]));
                                sendResponse(clientStream, "Withdrawal successful");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                sendResponse(clientStream, "Account does not exist");
                            }
                            break;
                        }
                        else
                        {
                            sendResponse(clientStream, "Command missing arguments");
                            break;
                        }

                    default:
                        break;
                }
            }
            client.Close();
        }

        public static void sendResponse(NetworkStream clientStream, String responseMessage)
        {
            byte[] responseData = Encoding.ASCII.GetBytes("------------------------------------------\r\n"+ responseMessage + "\r\n> ");
            clientStream.Write(responseData, 0, responseData.Length);
        }

        public static Boolean isCommandComplete(String[] commandArray, int numberExpected)
        {
            return (commandArray.Length == numberExpected) ? true : false;
        }
    }
}
