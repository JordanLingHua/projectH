using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;

public class LoginScreenGUI : MonoBehaviour {	
	

	public double delTime;
	public GUIText guiText;
	public string userName;
	public string password;
	public string ip;

	public GameProcess process;	
	public bool connected;
	public long latency;
	
	void Start () 
	{
		connected = false;
		process = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		latency = -1;
		userName = "";
		password = "";
		ip = "";
	}
	
	void OnGUI () 
	{	
		userName = GUI.TextField(new Rect(Screen.width / 2 - 55, Screen.height / 2 - 75, 125, 20), userName, 30);
		password = GUI.PasswordField(new Rect(Screen.width / 2 - 55, Screen.height / 2 - 50, 125, 20), password, '*', 30);
		ip = GUI.TextField(new Rect(Screen.width / 2 - 55, Screen.height / 2 - 25, 125, 20), ip, 30);

		if(GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 30, 50, 20), "Login"))
		{
			if (!ip.Equals(""))
				process.returnSocket().setIP(ip);

			if(!connected)
			{
				guiText.text = "Connecting...";
				if ( process.returnSocket().Connect() )
				{						
					guiText.text = "Connect Succeeded";	
					connected = true;
				}

				else 
					guiText.text = "Connect Failed";
			}

			string source = password;
			string hash;
			using (MD5 md5Hash = MD5.Create())
			{
				hash = GetMd5Hash(md5Hash, source);				
				Console.WriteLine("The MD5 hash of " + source + " is: " + hash + ".");				
				process.returnSocket().SendTCPPacket("userInfo\\" + userName + "\\" + hash);
			}
		}		
	}

	public void loginFail()
	{
		userName = "";
		password = "";
		guiText.text = "Invalid Login Info. Try Again.";
	}

	public void loginSucceed()
	{
		DontDestroyOnLoad(process);
		Application.LoadLevel(1);
	}

	string GetMd5Hash(MD5 md5Hash, string input)
	{
		
		// Convert the input string to a byte array and compute the hash. 
		byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
		
		// Create a new Stringbuilder to collect the bytes 
		// and create a string.
		StringBuilder sBuilder = new StringBuilder();
		
		// Loop through each byte of the hashed data  
		// and format each one as a hexadecimal string. 
		for (int i = 0; i < data.Length; i++)
		{
			sBuilder.Append(data[i].ToString("x2"));
		}
		
		// Return the hexadecimal string. 
		return sBuilder.ToString();
	}


	public void resetGuiText()
	{
		guiText.text = "";
	}
	
	void Update () 
	{
	}
	
	public void printGui ( string printStr )
	{
		int wordCount = 0 ;
		string[] words = printStr.Split(' ');
		
		printStr = "";
		
		for ( int i = 0 ; i < words.Length ; ++ i )
		{
			if ( wordCount <= 4 )
			{
				printStr += words[i] + " " ;
				wordCount ++ ;
			}
			else
			{
				printStr += words[i] + "\n" ;
				wordCount = 0;
				
			}	
		}
		
		guiText.text = printStr ;
	}
	
	
	
}
