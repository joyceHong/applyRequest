using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace applyRequests.Models
{
    public class entityAuthority
    {
        private TCSNewEntities tcsDB = new TCSNewEntities();

        public bool saveApplyAuthority(string userID, string bossID, string gmail, string power,string departmentID)
        {
            try
            {
                applyRequestsAuthorization applyRequestAuthorizObj = new applyRequestsAuthorization();
                applyRequestAuthorizObj.userID = userID;
                applyRequestAuthorizObj.bossID = bossID;
                applyRequestAuthorizObj.email = gmail;
                applyRequestAuthorizObj.useProcess = power;
                applyRequestAuthorizObj.department = departmentID;
                applyRequestAuthorizObj.isRDDispatch=false;

                tcsDB.applyRequestsAuthorization.Add(applyRequestAuthorizObj);
                tcsDB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public IEnumerable<authorityRole> listAllAuthorityRoles()
        {
            try
            {
                 var query = from p1 in tcsDB.applyRequestsAuthorization
                            join p2 in tcsDB.alluser on p1.userID equals p2.uid into roleUserName
                            join p3 in tcsDB.alluser on p1.bossID equals p3.uid into bossData
                            from role in roleUserName.DefaultIfEmpty()
                            from boss in bossData.DefaultIfEmpty()
                            select new authorityRole
                            {
                                Department = p1.department,
                                strBossUserName=boss.name,
                                strEmail=p1.email,
                                strUserName= role.name,
                                 flow=p1.useProcess,
                            };

                 IEnumerable<authorityRole> listAuthorityRoles = query.AsEnumerable<authorityRole>();
                 return listAuthorityRoles;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
        //public IEnumerable<>
    }
}