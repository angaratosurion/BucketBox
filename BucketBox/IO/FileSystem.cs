using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DropCloud.Api.DAL.DropCloudTableAdapters;
//using DropCloud.Api.DAL;

using System.IO;

namespace BucketBox.IO
{
    public class FileSystem
    {
         HttpServerUtility Server;
       public   string apppath;

       const string userroot = "UserFiles";

         string rootpath;
        public FileSystem()
         {
             apppath =System.Web.Hosting.HostingEnvironment.MapPath(HttpContext.Current.Request.ApplicationPath);
            rootpath = Path.Combine(apppath,"App_Data");

        }
        public void CreateUserFolder(string username)
        {
            try
            {
                if (username != null)
                {
                    Directory.CreateDirectory(Path.Combine(this.GetUserRoot(), username));
                    
                }


            }
            catch (Exception ex)
            {

               // Base.ErrorHandling(ex);
        // .//       return null;

            }

        }
        public void DeleteUserFolder(string username)
        {
            try
            {
                if (username != null)
                {
                    Directory.Delete(Path.Combine(this.GetUserRoot(), username),true);

                }


            }
            catch (Exception ex)
            {

                //Base.ErrorHandling(ex);
                // .//       return null;

            }

        }
        public string GetUserRoot()
        {


         //   try
            {
                string ap = "/";
               

                    ap = Path.Combine(apppath, userroot);
               
                return ap;


            }

            //catch (Exception ex)
            //{

            //    Base.ErrorHandling(ex);
            //    return null;

            //}
        }
        public string  GetUserFolder(string username)
        {
            try
            {
                string ap = null;
                if (username != null)
                {

                    ap = Path.Combine(this.GetUserRoot(), username);
                }
                return ap;


            }
            catch (Exception ex)
            {

                //Base.ErrorHandling(ex);
                    return null;

            }

        }
        public string GetRootFolder()
        {
            try
            {


              string   ap = rootpath;
               
                return ap;


            }
            catch (Exception ex)
            {

                //Base.ErrorHandling(ex);
                return null;

            }

        }
        public Boolean FolderExists(string path)
        {

            try
            {
                Boolean ap = false;
                if (path != null)
                {
                    ap = Directory.Exists(path);

                }
                return ap;

            }
            catch (Exception ex)
            {

                //Base.ErrorHandling(ex);
                return false;

            }
        }
        public Boolean FileExists(string path)
        {

            try
            {
                Boolean ap = false;
                if (path != null)
                {
                    ap = File.Exists(path);

                }
                return ap;

            }
            catch (Exception ex)
            {

                //Base.ErrorHandling(ex);
                return false;

            }
        }
    }
}