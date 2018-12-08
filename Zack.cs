using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace File_Encryption {

    public class Zack : Crypter {

        private readonly int _size;
        private readonly string _message;
  
		
        public Zack(string file, string message) : base("tiff", "Zack", file) {
            
            _message = message;
            _size = (int)Math.Ceiling(Math.Sqrt(message.Length));
            encryptFile();

        }

           
        public sealed override string encryptFile() {
//ignore this part for now ill fix these errors later with the path
            using (var img = new FileStream(@"C:\Users\Zacke\Documents\zack.tif", FileMode.Create)) {
                using (var bmp = new Bitmap(_size, _size)) {
                    int column;
                    for (column = 0; column < bmp.Height; column++) {
                        int row;
                        for (row = 0; row < bmp.Width; row++)
                            bmp.SetPixel(row, column,
                                row + (column * bmp.Width) < bmp.Width
                                    ? GetRgb(_message[row + (column * bmp.Width)])
                                    : GetRgb(' '));
                    }
                    bmp.Save(img, ImageFormat.Tiff);
                }
            }
            return _message;
        }

        private static Color GetRgb (char ch){
            int cbyte = ch;
            var color = Color.FromArgb(cbyte,cbyte,cbyte);
            return color;
        }

        public override string decryptFile(string filename) {
            throw new NotImplementedException();
        }
    }
    
}
