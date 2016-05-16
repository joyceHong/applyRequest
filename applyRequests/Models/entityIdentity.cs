using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace applyRequests.Models
{
    public class entityIdentity
    {
        public int loginPass(string strAccount, string strPassword)
        {
            try
            {
                using (TCSNewEntities tcsDB = new TCSNewEntities())
                {
                    object objResult = tcsDB.IdentifyLogin3(strAccount, strPassword, "").FirstOrDefault();
                    return (int) objResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}