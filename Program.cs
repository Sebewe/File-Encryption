/*
 *  								---NOTE---
 * PLEASE CREATE YOUR CLASSES IN A SEPERATE FILE, MAKE SURE IT'S IN THE SAME NAMESPACE, AND YOU'RE GOOD TO GO.
 * WE ARE DOING THIS SO FILE SHARING IS MUCH EASIER.
 * CURRENT FILE EXTENSIONS BEING USED (can be changed at any time): 
 * Sebastian - PNG
 * Zack - IMG
 * Elija - JPEG
 */

using System;

namespace File_Encryption {
	
	public class Program {

		public static void Main(string[] args) {
			Console.WriteLine("Would you like to give a custom path to save the file(1) or use the default?(2)");
			var path = "D:\\";
				
			if (Console.ReadLine().Equals("1")) {
				Console.WriteLine("Please enter the full desired path.");
				path = Console.ReadLine();
			} else 
				Console.WriteLine($"Default path set to {path}");
			
			Console.WriteLine("Would you like to create a new Image (png)? (yes/no)");
			if (Console.ReadLine().Equals("no")) {
				Console.WriteLine("What is the full path of the Image(png) (including extenstion)");
				var filePath = Console.ReadLine();
				var sebas = new Sebas(true, filePath, path);
				Console.WriteLine($"Message recieved: \n{sebas.decryptFile(filePath)}");
			}
			else {
				Console.WriteLine("Would you like to give a message via console (1) or give a txt file containing a message(2)");
				if(Console.ReadLine().Equals("1")){
					Console.WriteLine("What would you like the secret message to be?");
					var sebas = new Sebas(false, Console.ReadLine(), path);
					sebas.log($"Successfully created image! \n      {sebas.encryptFile()}");
					sebas.log($"Sebas returned this value: {sebas.decryptFile("")}");
				} else {
					Console.WriteLine("What is the full path of the file including the name and extension? (txt only)");
					var sebas = new Sebas(false, System.IO.File.ReadAllText(Console.ReadLine()), path);
					sebas.log($"Successfully created image! \n      {sebas.encryptFile()}");
					sebas.log($"Sebas returned this value: {sebas.decryptFile("")}");
				}
			}
		}
	}
	
	public abstract class Crypter {
		
		// YOUR CLASS MUST EXTEND THIS CLASS
		
		protected string extension, name, file, secret;

		protected Crypter(string extension, string name, string file) {
			this.name = name;
			this.extension = extension;
			this.file = file;
		}

		public void log(string log) {
			Console.WriteLine($"{name}: {log}");	
		}

		public string getFile() {
			return this.file;
		}

		public string setFile(string file) {
			string temp = this.file;
			this.file = file;
			return temp;
		}

		public abstract string encryptFile(); //return your file's name

		public abstract string decryptFile(string file); //return encrypted info

	}

}
