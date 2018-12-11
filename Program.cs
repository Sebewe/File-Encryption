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
using System.IO;
using System.Text;

namespace File_Encryption {
	
	public class Program {

		public static void Main(string[] args) {
			var sebas = new Sebas();
			string path;
			string text;
			
			Console.WriteLine("Would you like to encrypt(1) or decrypt(2)?");
			if (Console.ReadLine().Equals("1")) {//encrypt
				Console.WriteLine("What is the full path of the txt file you'd like to encrypt?(.txt only, include extension)");
				path = Console.ReadLine();
				text = File.ReadAllText(path);
				sebas.encryptFile(text);
			} else { //decrypt
				Console.WriteLine("What is the full path of the txt file you'd like to decrypt?(.png only, include extension)");
				sebas.decryptFile(Console.ReadLine());
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
