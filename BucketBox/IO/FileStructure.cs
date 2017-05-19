using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;

namespace BucketBox.IO
{
    public static  class FileStructure
    { 
        static FileSystem filesys = new FileSystem();
        public const string OpenCVFiles="Emgu.CV";
        public const string haarcascadedir = "haarcascades";
        public const string crawlfoldname = "crawledfiles";
        public static string  Crawlfolder;
        public static string haarcascadeDirectory,RootOpenCv;
        public static  Dictionary<string,string> haarcascades = new Dictionary<string,string>();
        
        public static void FillFileStrureInfo()
        {
            
              try
              {
                  filesys = new FileSystem();
                RootOpenCv = Path.Combine(filesys.GetRootFolder(), OpenCVFiles);
             haarcascadeDirectory =Path.Combine(RootOpenCv ,haarcascadedir);
                  FindAllhaarcascade();


            }
            catch (Exception ex)
            {

               // Base.ErrorHandling(ex);
        // .//       return null;

            }


        }
        public static void  FindAllhaarcascade()
        {

            
              try
            {
            
                 string [] files =Directory.GetFiles(haarcascadeDirectory,"*.xml");

                  foreach(string f in files)
                  {
                      string tf=Path.GetFileNameWithoutExtension(f);
                      if (tf != null)
                      {

                          haarcascades.Add(tf.Substring(tf.LastIndexOf("_")+1), f);
                      }


                  }


            }
            catch (Exception ex)
            {

             //   Base.ErrorHandling(ex);
       
            }

        }



    }
}