using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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
            
//create a prompt function that prints the question and gets the input
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
                    using (var img2 = new FileStream(_newPath, FileMode.Create)) {
                        using (var bmp2 = new Bitmap(bmp.Width, bmp.Width)) {
                            int column;
                            for (column = 0; column < bmp.Height; column++) {
                                int row;
                                for (row = 0; row < bmp.Width ; row++) {
                                    var cp = bmp.GetPixel(row, column);

                                    if (!string.IsNullOrEmpty(_binary)) {
                                        var tmp = (int) char.GetNumericValue(_binary[0]);
                                        switch (tmp) {
                                            case 0: {
                                                if (cp.R % 2 != 0)
                                                    bmp2.SetPixel(row, column,
                                                        Color.FromArgb(cp.R - 1, cp.G,
                                                            cp.B));
                                                break;
                                            }
                                            case 1: {
                                                if (cp.R % 2 == 0 || cp.R == 0)
                                                    bmp2.SetPixel(row, column,
                                                        Color.FromArgb(cp.R + 1, cp.G, cp.B));

                                                break;
                                            }

                                            default: {
                                                Console.WriteLine("What the heck happened here? : " + tmp);
                                            }
                                                break;
                                        }

                                        _binary = _binary.Substring(1);
                                    }
                                    else {
                                        bmp2.SetPixel(row, column, cp);
                                    }
                                }
                                
                            }
                            bmp2.Save(img2, ImageFormat.Png);
                            bmp.Save(img, ImageFormat.Png);
                        }
                    }
                }
            }

            return _message;
        }

        private void CreateBinary() {
            foreach (var ch in _message) {
                var test = Convert.ToString(ch, 2);
                while (test.Length < 8) {
                    test = "0" + test;
                    _binary += test;
                }
            }
        }

      

        private string Prompt(string prompt) {
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


                            if (cp.R % 2 != 0) bindecryption += "1";


                            if (cp.R % 2 == 0 || cp.R == 0) bindecryption += "0";
                        }
                    }
                }
            }

            var decryption = "";
            while (bindecryption.Length >= 8 && !(_endNum >= _end.Length)) {
                if (Convert.ToChar(Convert.ToInt32(bindecryption.Substring(0, 8), 2)) == _end[_endNum]) {
                    _endNum++;
                    Console.WriteLine(Convert.ToChar(Convert.ToInt32(bindecryption.Substring(0, 8), 2)));
                }
                else
                    _endNum = 0;
                decryption += Convert.ToChar(Convert.ToInt32(bindecryption.Substring(0, 8), 2));
                
                bindecryption = bindecryption.Substring(8);
                
            }

            Console.WriteLine(decryption);
            return decryption;
        }
    }
}
