using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BucketBox.Binary;
using System.IO;
using System.Windows.Forms;
using BucketBox.Cryptography;
using BucketBox.Devices;

namespace buckettest
{
    class Program
    { 
        static void Main(string[] args)
        {
            string file = Application.StartupPath;
            string pass= "test";
            const string inf ="in.txt";
            const string of="out.txt";
            const string bf="ef.txt";
            const string df="df.txt";
            BlackBoxCrypt bcrypt = new BlackBoxCrypt();
            BucketBox.Binary.Bytes bit = new Bytes();
           // System.IO.FileStream tf= new FileStream(Path.Combine(file,inf),FileMode.Open);
            System.IO.FileStream tf2 = new FileStream(Path.Combine(file,of), FileMode.Append );
            System.IO.FileStream tf3 = new FileStream(Path.Combine(file, bf), FileMode.Append);
            System.IO.FileStream tf4 = new FileStream(Path.Combine(file, df), FileMode.Append);
            byte [] r= File.ReadAllBytes(Path.Combine(file,inf));
            byte[] t = bcrypt.Encrypt(r, 4,pass);
            byte[] d = bcrypt.Decrypt(t, 4,pass);
            foreach (byte b in r)
            {
                bool[] bits = bit.GetBits(b);
                
                tf2.WriteByte(bit.GetByte(bits));
                

               
                

            }
            foreach(byte c in t)
            {
                Console.WriteLine("{0}", c);
                tf3.WriteByte(c);

            }
             foreach(byte c in d)
            {
                Console.WriteLine("{0}", c);
                tf4.WriteByte(c);

            }
           

            tf2.Flush();
            tf2.Close();
            tf3.Flush();
            tf3.Close();
            tf4.Flush();
            tf4.Close();

            Console.WriteLine(Machine.GetCurrentChassisType().ToString());
            //Monitor mon = new Monitor();
            byte[] blevels = Monitor.GetWMIBrightnessLevels();
            for(int i=0; i<blevels.Length;i++)
            {
                Console.WriteLine( blevels[i].ToString());
            }

            
            Console.ReadLine();
        }
    }
}
