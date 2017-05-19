using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BucketBox.Arrays
{
    public class Conver2DTo1D
    {
        public byte[] ByteArray(byte[,] arg, uint lines, uint col)
        {
            try
            {
                byte[] ap = null;
                int i = 0, j = 0;

                if ((arg != null) && (lines > 0)&&(col > 0))
                {
                    ap= new Byte[lines*col];
                    System.Buffer.BlockCopy(arg, 0, ap, 0,(int) (lines * col));
                
                }


                return ap;


            }
            catch (Exception ex)
            {
                Base.exceptionHandle(ex);

                return null;
            }

        }
        public bool[] BoolArray(bool[,] arg, uint lines, uint col)
        {
            try
            {
                bool[] ap = null;
                int i = 0, j = 0;

                if ((arg != null) && (lines > 0) && (col > 0))
                {
                    ap = new bool[lines * col];
                    System.Buffer.BlockCopy(arg, 0, ap, 0, (int)(lines * col));

                }


                return ap;


            }
            catch (Exception ex)
            {
                Base.exceptionHandle(ex);
                return null;
            }

        }
    }
}
