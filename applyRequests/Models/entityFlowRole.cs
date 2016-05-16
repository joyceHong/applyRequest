using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace applyRequests.Models
{
   
    public class entityFlowRole
    {

        public TCSNewEntities tcsDB = new TCSNewEntities();

        #region 流程角色

        public flowRole getUserRole(int intRequestID,string strApplyUserID, string strRoleName)
        {

            try
            {
                switch (strRoleName)
                {
                    case "boss":
                        return boss(strApplyUserID);
                    case "rdDispatch":
                        return rdDispatchRole();
                    case "rdAcceptTaskUser":
                        return rdAcceptTaskUser(intRequestID);
                    default:
                        return rdAcceptTaskUser(intRequestID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("entityFlowRole:getUserRole" + ex.Message);
            }
        }

        /// <summary>
        /// 申請者的主管角色
        /// </summary>
        /// <param name="intApplyUserID"></param>
        /// <returns></returns>
        public flowRole boss(string strApplyUserID)
        {
            try
            {
                var query = from p1 in tcsDB.applyRequestsAuthorization
                            join p2 in tcsDB.alluser on p1.bossID equals p2.uid into roleUserName
                            from role in roleUserName.DefaultIfEmpty()
                            where p1.userID == strApplyUserID
                            select new flowRole
                            {
                                strRoleUserID = role.uid,
                                strRoleUserName = role.name,
                                strProcessName = "主管",
                                strEmail=p1.email
                            };

                flowRole flowRoleObj =(flowRole) query.FirstOrDefault();
                return flowRoleObj;
            }
            catch (Exception ex)
            {
                throw new Exception("entityFlowRole:bossRole" + ex.Message);
            }
        }

        /// <summary>
        /// RD 窗口的角色
        /// </summary>
        /// <returns></returns>
        public flowRole rdDispatchRole()
        {
            try
            {
                var query = from p1 in tcsDB.applyRequestsAuthorization
                            join p2 in tcsDB.alluser on p1.userID equals p2.uid into roleUserName
                            from role in roleUserName.DefaultIfEmpty()
                            where p1.isRDDispatch == true
                            select new flowRole
                            {
                                strRoleUserID = role.uid,
                                strRoleUserName = role.name,
                                strProcessName = "RD窗口",
                                strEmail = p1.email
                            };

                flowRole flowRoleObj = (flowRole)query.FirstOrDefault();
                return flowRoleObj;
            }
            catch (Exception ex)
            {
                throw new Exception("entityFlowRole:rdDispatch" + ex.Message);
            }
        }

        /// <summary>
        /// 接受指派任務的RD人員名單
        /// </summary>
        /// <returns></returns>
        public IEnumerable<flowRole> listRdAcceptTaskUsers()
        {
            try
            {
                var query = from p1 in tcsDB.applyRequestsAuthorization
                            join p2 in tcsDB.alluser on p1.userID equals p2.uid into roleUserName
                            from role in roleUserName.DefaultIfEmpty()
                            where p1.department == "3"
                            select new flowRole
                            {
                                strRoleUserID = role.uid,
                                strRoleUserName = role.name,
                                strProcessName = "RD處理人員"
                            };
                
                IEnumerable<flowRole> listRdAcceptTaskUsers = query.AsEnumerable<flowRole>();
                return listRdAcceptTaskUsers;
            }
            catch (Exception ex)
            {
                throw new Exception("entityFlowRole:listRdAcceptTaskUsers" + ex.Message);
            }
        }

        /// <summary>
        /// 申請者本人
        /// </summary>
        /// <param name="intApplyRequestID"></param>
        /// <returns></returns>
        public flowRole rdAcceptTaskUser(int intRequestID)
        {
            try
            {
                var query = from p1 in tcsDB.applyRequests
                            join p2 in tcsDB.alluser on p1.accepTaskUser equals p2.uid into roleUserName
                            join p3 in tcsDB.applyRequestsAuthorization on p1.applyUser equals p3.userID into authorizeUser
                            from role in roleUserName.DefaultIfEmpty()
                            from authorize in authorizeUser.DefaultIfEmpty()
                            where p1.ikey==intRequestID
                            select new flowRole
                            {
                                strRoleUserID = role.uid,
                                strRoleUserName = role.name,
                                strProcessName = "完工",
                                strEmail = authorize.email,
                                strBossID = authorize.bossID
                            };

                flowRole flowRoleObj = (flowRole)query.FirstOrDefault();
                return flowRoleObj; 
            }
            catch (Exception ex)
            {
                throw new Exception("entityFlowRole:rdAcceptTaskUser" + ex.Message);
            }
        }

        /// <summary>
        /// 申請使用者，所跑的流程類別
        /// </summary>
        /// <param name="strApplyUserID"></param>
        /// <returns></returns>
        public flowRole readUserFlowRole(string strApplyUserID)
        {
            try
            {
                 var query = from p1 in tcsDB.applyRequestsAuthorization
                             join p2 in tcsDB.alluser on p1.userID equals p2.uid into roleUserName
                             join p3 in tcsDB.applyRequestsAuthorization on p1.bossID equals p3.userID into bossData
                             from role in roleUserName.DefaultIfEmpty()
                             from boss in bossData.DefaultIfEmpty()
                             where p1.userID==strApplyUserID
                             select new flowRole
                             {
                                strRoleUserID=p1.userID,
                                strEmail= p1.email,
                                strProcessType=p1.useProcess,                                
                                strRoleUserName = role.name,
                                strBossID = p1.bossID,
                                strBossEmail=boss.email
                             };

                 return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


        public IEnumerable<flowRole> listAllUsers(string userID)
        {
            try
            {
                var query = (from p1 in tcsDB.alluser
                             where p1.uid == userID
                             select new flowRole
                             {
                                 strRoleUserID = p1.uid,
                                 strRoleUserName = p1.name,
                             }).Union(
                            from p2 in tcsDB.applyRequestsAuthorization
                            join p3 in tcsDB.alluser on p2.userID equals p3.uid into users
                            from user in users.DefaultIfEmpty()
                            where p2.bossID == userID
                            select new flowRole
                            {
                                strRoleUserID = user.uid,
                                strRoleUserName = user.name
                            });
                 IEnumerable<flowRole> listAllUsers = query.AsEnumerable<flowRole>();
                 return listAllUsers;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //顯示所有員工姓名
        public IEnumerable<flowRole> listAllUsers()
        {
            try
            {
                var query = (from p1 in tcsDB.alluser
                             where p1.staff == false
                             select new flowRole
                             {
                                 strRoleUserID = p1.uid,
                                 strRoleUserName = p1.name,
                             });
                IEnumerable<flowRole> listAllUsers = query.AsEnumerable<flowRole>();
                return listAllUsers;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
    }
}