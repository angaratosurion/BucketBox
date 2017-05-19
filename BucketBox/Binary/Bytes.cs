using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace BucketBox.Binary
{
  public   class Bytes
    {
      public bool[] GetBits(Byte  arg)
      {
         // try
          {
              bool[] ap = new bool[8];
              //bool a;
              int i = 0;
              if (arg !=null)
              {
                  

                      for (i = 0; i <ap.Length; i++)
                      {
                          ap[i] = this.GetBit(arg, i);

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
      private bool GetBit(byte value,int bitNumber)
      {
          try
          {

              // if ((bitNumber < 0) || (bitNumber > 7))
            {
               // throw new ArgumentOutOfRangeException("bitNumber", bitNumber, "bitNumber must be 0..7");
            }

            return ((value & (1<<bitNumber)) != 0);
          }

          catch (Exception ex)
          {
              Base.exceptionHandle(ex);
              return false;

          }
      }
      public byte GetByte(bool[] val)
      {
          try
          {
              byte ap = 0;
              BitArray bits;
              if (val != null)
              {
                  bits = new BitArray(val);
                  if (bits.Length != 8)
                  {
                   //   throw new ArgumentException("bits");
                  }
                  byte[] bytes = new byte[1];
                  bits.CopyTo(bytes, 0);
                  ap = bytes[0];
              }
             // return bytes[0];
              

              

              return ap;
          }

          catch (Exception ex)
          {
              Base.exceptionHandle(ex);
              return 0;
          }

      }
    }
}
