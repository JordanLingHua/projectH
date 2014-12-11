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

	AudioManager am;
	PopUpMenu pum;
	
	void Start () 
	{
		connected = false;
		pum = GameObject.Find ("PopUpMenu").GetComponent<PopUpMenu> ();
		am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		process = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		latency = -1;
		userName = string.Empty;
		password = string.Empty;
		ip = string.Empty;
	}
	
	void OnGUI () 
	{	
		userName = GUI.TextField(new Rect(Screen.width / 2 - 55, Screen.height / 2 - 75, 125, 20), userName, 30);
		password = GUI.PasswordField(new Rect(Screen.width / 2 - 55, Screen.height / 2 - 50, 125, 20), password, '*', 30);
		ip = GUI.TextField(new Rect(Screen.width / 2 - 55, Screen.height / 2 - 25, 125, 20), ip, 30);

		if(GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 30, 50, 20), "Login"))
		{
			am.playButtonSFX();

			//use the alternative ip provided by the user if the ip field is not blank
			if (!ip.Equals(string.Empty))
				process.returnSocket().setIP(ip);

			if(!connected)
			{
				guiText.text = "Connecting...";
				if ( process.returnSocket().Connect() )
				{						
					guiText.text = "Connect Succeeded";	
					connected = true;
				}

				else {
					am.playErrorSFX ();
					guiText.text = "Connect Failed";}
			}

			string source = password;
			string hash;

			//hash an encrypted password for the user
			using (MD5 md5Hash = MD5.Create())
			{
				hash = GetMd5Hash(md5Hash, source);				
				Console.WriteLine("The MD5 hash of " + source + " is: " + hash + ".");				
				process.returnSocket().SendTCPPacket("userInfo\\" + userName + "\\" + hash);
			}
		}		
	}

	//called from gameprocess when a user inputs invalid login info
	public void loginFail()
	{
		password = string.Empty;
		am.playErrorSFX ();
		guiText.text = "Invalid Login Info. Try Again.";
	}

	//server verifies info and user is logged in
	public void loginSucceed()
	{
		DontDestroyOnLoad(process);
		DontDestroyOnLoad (am);
		DontDestroyOnLoad (pum);
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
		guiText.text = string.Empty;
	}
	
	void Update () 
	{
	}	
}