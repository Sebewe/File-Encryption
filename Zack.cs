using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace File_Encryption {
    public class Zack : Crypter {
        private readonly string _message;
        private readonly string _path, _newPath, _end;
        private string _binary;
        private int _endNum;

        public Zack() : base("tiff", "Zack", null) {
            _endNum = 0;
            _end = "---";
            _path = Prompt("What file will you be encrypting / decrypting");

            mode:
            switch (Prompt("encode or decode?")) {
                case "encode":
                    Console.WriteLine("Where would you like the encrypted file to save");
                    _newPath = Console.ReadLine();
                    Console.WriteLine("what is the message");
                    _message = Console.ReadLine() + _end;
                    encryptFile();
                    break;
                case "decode":
                    decryptFile("why does the main class want a string?");
                    break;
                default:
                    Console.WriteLine("Try again");
                    goto mode;
            }
        }

        public sealed override string encryptFile() {
            CreateBinary();
            
            using (var img = new FileStream(_path, FileMode.Open)) {
                using (var bmp = new Bitmap(img)) {   
                        using (var bmp2 = new Bitmap(bmp.Width, bmp.Width)) {
                            // opens the images as bitmap and iterates through each pixel in the first image
                            for (var column = 0; column < bmp.Height; column++) {
                                for (var row = 0; row < bmp.Width; row++) {
                                    var cp = bmp.GetPixel(row, column);
                                    if (!string.IsNullOrEmpty(_binary)) {
                                        var tmp = (int) char.GetNumericValue(_binary[0]); // gets the first of the string which is the message in binary
                                        _binary = _binary.Substring(1);  // removes the first value so the loop can iterate through the binary
                                        var newColor = cp.R; // used to read pixel value
                                        Console.WriteLine("s :" + newColor); //debug
                                        switch (tmp) { // embeds the binary string into the image
                                            case 0: {
                                                if (cp.R % 2 != 0) { 
                                                    newColor -= 1;  //makes the number even if its not
                                                    
                                                }
                                                break;
                                            }
                                            case 1: {
                                                if (cp.R % 2 == 0 || cp.R == 0) {
                                                    newColor +=1;  //makes the number odd if its not
                                                    
                                                }
                                                break;
                                            }
                                            default: {
                                                Console.WriteLine("What the heck happened here? : " + tmp);

                                                break;
                                            }
                                        }
                                        Console.WriteLine(newColor); //debug to test pixel values
                                        
                                        bmp2.SetPixel(row, column,
                                            Color.FromArgb(newColor, cp.G, cp.B));  // sets the new color for the second image
                                        Console.WriteLine(_binary);  // debug to test deleting values
                                    }
                                    else {
                                        bmp2.SetPixel(row, column, cp);
                                    }
                                }
                            }
                            //saves everything and creates the files
                            using (var img2 = new FileStream(_newPath, FileMode.Create)) 
                            bmp2.Save(img2, ImageFormat.Png);
                            bmp.Save(img, ImageFormat.Png);
                        }
                    
                }
            }
            return _message;
        }

        private void CreateBinary() {
            foreach (var ch in _message) {
                var test = Convert.ToString(ch, 2); // uses base 2 to convert the int to string to get binary
                while (test.Length < 8) {
                    test = "0" + test;
                    _binary += test;
                }
            }
        }

        private static string Prompt(string prompt) {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        public sealed override string decryptFile(string bs) {
            var bindecryption = "";
            using (var img = new FileStream(_path, FileMode.Open)) {
                using (var bmp = new Bitmap(img)) {
                    int column;
                    for (column = 0; column < bmp.Height; column++) {
                        int row;
                        for (row = 0; row < bmp.Width; row++) {
                            var cp = bmp.GetPixel(row, column);
                            if (cp.R % 2 != 0) bindecryption += "1";  // pulls the binary string out from the image
                            if (cp.R % 2 == 0 || cp.R == 0) bindecryption += "0";
                        }
                    }
                }
            }
            var decryption = "";
            while (bindecryption.Length >= 8 && !(_endNum >= _end.Length)) {  // if there is still another byte/char and you haven't reached the end sign
                if (Convert.ToChar(Convert.ToInt32(bindecryption.Substring(0, 8), 2)) == _end[_endNum]) {
                    _endNum++;
                    //Console.WriteLine(Convert.ToChar(Convert.ToInt32(bindecryption.Substring(0, 8), 2)) + " : " + _endNum);
                    // used to debug end message code
                }
                else {
                    _endNum = 0;
                }
                decryption += Convert.ToChar(Convert.ToInt32(bindecryption.Substring(0, 8), 2));
                bindecryption = bindecryption.Substring(8);  // removes the first byte that was just read so the next can be read
            }
            Console.WriteLine(decryption);
            return decryption;
        }
    }
}
