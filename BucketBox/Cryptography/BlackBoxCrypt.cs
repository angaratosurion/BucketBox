using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using BucketBox.Arrays;
using BucketBox.Binary;

namespace BucketBox.Cryptography
{
    public class BlackBoxCrypt
    {
        Convert1DTo2D conv1 = new Convert1DTo2D();
        Conver2DTo1D conv2 = new Conver2DTo1D();
        Bytes byts = new Bytes();
        public byte Blakenit(byte data,string password)
        {
            //try
            {
                byte ap = 1;
                int i;
                byte[] r = Encoding.UTF8.GetBytes(password);
                if( password !=null)
                {
                    foreach( byte t in r)
                    {
                        //bool[] b = byts.GetBits(Convert.ToByte(t));
                     //   char [] a= t.ToCharArray();
                       // for (i=0;i<a.Length;i++)
                        {
                            int b, o,e;
                            b = (int)data;
                            o = t;
                           e =~(b ^ o);
                           ap = (byte)e;
                                
                        }



                    }

                }

                return ap;

            }
            //catch (Exception ex)
            //{
            //    Base.exceptionHandle(ex);

            //    return null;
            //}
        }
        public byte[] Encrypt(byte[] arg,uint blocksz,string  password)
        {
          //  try
            {
                byte[] ap = null;
              //  byte[,] t;
                int i = 0,j=0;
                uint sq;
                List<bool[]> q =new List< bool[]>();
                List<byte> e = new List<byte>();
              // bool [,] ar,br=null;
              // bool[] r;
                bool [][] g;
               if ((arg != null) &&(password !=null))
               {
                   //  sq =(uint) Math.Round(Math.Sqrt(arg.Length ), 0)*8;
                     foreach (byte b in arg)
                     {
                         bool[] p = byts.GetBits(b);
                         bool[] c = new bool[p.Length];
                         c[0]=p[0];
                         for (i = 1; i < p.Length; i++)
                         {
                             c[i] = p[p.Length - i];

                         }
                         e.Add(this.Blakenit(byts.GetByte(c),password)) ;
                         
                     }

                     ap = new Byte[e.Count];
                     e.CopyTo(ap, 0);
                     //foreach (byte b in arg)
                     //{
                     //    ar = conv1.BoolArray(byts.GetBits(b), sq);


                     //    br = new bool[sq, sq];
                     //    for (i = 0; i < sq; i++)
                     //    {
                     //        br[i, 0] = ar[i, 0];

                     //    }
                     //    for (i = 1; i < sq; i++)
                     //    {
                     //        for (j = 0; j < sq; j++)
                     //        {

                     //            br[i, j] = ar[i, sq - j];
                     //        }

                     //    }
                        


                         
                     //}


                     //r = conv2.BoolArray(br, sq, sq);
                    

                     //ap = new byte[arg.Length ];
                     //for (i = 0; i < arg.Length ; i++)
                     //{
                     //    bool[] o = new bool[8];
                     //    Array.Copy(r, i * 8, o, 0, 8);
                     //    ap[i] = byts.GetByte(o);


                     //}
                         

                    
                   


               }

                return ap;

            }
            //catch (Exception ex)
            //{
            //    Base.exceptionHandle(ex);

            //    return null;
            //}
        }
        public byte[] Decrypt(byte[] arg, uint blocksz,string password)
        {
            try
            {
                uint sq;
                int i = 0, j = 0;
                byte[] ap = null;
                List<byte> e = new List<byte>();
                if ((arg != null) && (blocksz != null) &&(password !=null))
                {
                    sq = (uint)Math.Round(Math.Sqrt(blocksz), 0) * 8;
                    foreach (byte b in arg)
                    {
                        bool[] p = byts.GetBits(this.Blakenit(b, password));
                        bool[] c = new bool[p.Length];
                        c[0] = p[0];
                        for (i = 1; i < p.Length; i++)
                        {
                            c[i] = p[p.Length - i];

                        }
                        e.Add(byts.GetByte(c));

                    }

                    ap = new Byte[e.Count];
                    e.CopyTo(ap, 0);
                       





                }

                return ap;

            }
            catch (Exception ex)
            { throw( ex);
            Base.exceptionHandle(ex);
                return null;
            }
        }
    }
}
