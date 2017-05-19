using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BucketBox.Arrays
{
   public class Convert1DTo2D
    {
       public byte[,] ByteArray(byte[] ar,uint lines)
       {
           try
           {
               byte[,] ap = null;
               int i = 0,j=0,l;
               double a = 0;
               if ((ar != null) &&(lines >0))
               {
                   a=(ar.Length / lines);
                   l =(int) Math.Round(a, 0);
                   if (l > 0)
                   {
                       ap= new byte[lines,l];
                       Array.Copy(ar, 0, ap, 0, ar.Length);



                   }


               }


               return ap;
           }
           catch (Exception ex)
           {
               Base.exceptionHandle(ex);
               return null;
           }

       }
       public bool[,] BoolArray(bool[] ar, uint lines)
       {
         //  try
           {
               bool[,] ap = null;
               int i = 0, j = 0, l;
               double a = 0;
               if ((ar != null) && (lines > 0))
               {
                   a = (ar.Length / lines);
                   l = (int)Math.Round(a, 0);
                   if (l > 0)
                   {
                       ap = new bool[lines, l];
                       Array.Copy(ar, 0, ap, 0, ar.Length);



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
            
    }
}
