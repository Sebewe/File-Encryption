using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace File_Encryption {

    public class Zack : Crypter {
        
        private string _message, _binstr;
        private readonly string _path;
  
        public Zack() : base("tiff", "Zack", null) {
            
            Console.WriteLine("What file path will you be using (include file+extension)");
            var input = Console.ReadLine();
            _path = input;
            
            Console.WriteLine("encode or decode?");
            userinput:
            var mode = Console.ReadLine();
                      
            switch (mode) {
                case "encode":
                    Console.WriteLine("what is the message");
                    _message = Console.ReadLine() + "---";
                    encryptFile();
                    break;
                case "decode":
                    decryptFile("why does the main class want a string?");
                    break;
                default:
                    Console.WriteLine("Try again");
                    goto userinput;
            }

        }

        public sealed override string encryptFile() {
            CreateBinary();
            using (var img = new FileStream(_path, FileMode.Open)) {
                using (var bmp = new Bitmap(img)) {
                    int column;
                    for (column = 0; column < bmp.Height; column++) {
                        int row;
                        for (row = 0; row < bmp.Width; row++) {
                            var cp = bmp.GetPixel(row, column);
                            var tmp = (int)char.GetNumericValue(_binstr[0]);
                            _binstr.Remove(0, 1);
                            switch (tmp) {
                                case 0: {
                                    if (cp.R % 2 != 0 ) {
                                        bmp.SetPixel(row, column, Color.FromArgb(cp.R-1, cp.G, cp.B));  //please don't ask why i subtract to one and add to the other
                                                                                                        //it hurts my brain to think about it
                                    }

                                    break;
                                }
                                case 1: {
                                    if (cp.R % 2 == 0 || cp.R == 0) {
                                                                                    
                                        bmp.SetPixel(row, column, Color.FromArgb(cp.R+1, cp.G, cp.B));                                      
                                        
                                    }
                                    break;
                                }

                                default: {
                                    Console.WriteLine("What the heck happened here? : " + tmp);
                                }
                                break;                                
                            }
                        }
                    }
                    bmp.Save(img, ImageFormat.Png);
                }
            }
            return _message;
        }

        private void CreateBinary() {
            foreach (var ch in _message) {
                var test = Convert.ToString((int) ch, 2);
                while (test.Length < 8) {
                    test = "0" + test;
                    _binstr += test;
                }
            }
        }

        private static Color GetRgb (char ch){
            int cbyte = ch;
            var color = Color.FromArgb(cbyte,cbyte,cbyte);
            return color;
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



                            if (cp.R % 2 != 0) {
                                bindecryption += "1";
                            }



                            if (cp.R % 2 == 0 || cp.R == 0) {

                                bindecryption += "0";

                            }


                        }
                    }
                }
            }

            var decryption= "";
            while (bindecryption.Length >= 8){
                decryption += Convert.ToChar(Convert.ToInt32(bindecryption.Substring(0,8),2));
                bindecryption = bindecryption.Substring(8);
            }

            Console.WriteLine(decryption);
            return decryption;
        }
    }
    
}
