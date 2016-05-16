using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace applyRequests.Models
{
    public class controlDoActioncs : InterlDoActioncs
    {

        entityFlowRole entityFlowRoleObj = new entityFlowRole();
        entityRequest entityDoAction = new entityRequest();
        entityIdentity entityIdentity = new entityIdentity();

        entityAuthority entityAuthorityObj = new entityAuthority();
        controlEmail controlEmail = new controlEmail();

        #region 角色定義

        /// <summary>
        /// 主管
        /// </summary>
        /// <param name="strApplyUserID"></param>
        /// <returns></returns>
        public flowRole bossRole(string strApplyUserID)
        {
            try
            {
                return entityFlowRoleObj.boss(strApplyUserID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// RD窗口
        /// </summary>
        /// <returns></returns>
        public flowRole rdDispatchRole()
        {
            try
            {
                return entityFlowRoleObj.rdDispatchRole();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 指派人員
        /// </summary>
        /// <param name="intRequestID"></param>
        /// <returns></returns>
        public flowRole rdAcceptTaskUser(int intRequestID)
        {
            try
            {
                return entityFlowRoleObj.rdAcceptTaskUser(intRequestID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// RD人員(負責接受指派任務的人員)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<flowRole> listRdAcceptTaskUsers()
        {
            try
            {
                return entityFlowRoleObj.listRdAcceptTaskUsers();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //顯示該userID 管理的員工和自已 列表
        public IEnumerable<flowRole> listAllUsers(string bossID)
        {
            try
            {
                return entityFlowRoleObj.listAllUsers(bossID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //顯示所有員工
        public IEnumerable<flowRole> listAllUsers()
        {
            try
            {
                return entityFlowRoleObj.listAllUsers();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 讀取申請者的流程群組flow1(一般)，flow2(主管)
        /// </summary>
        /// <param name="strApplyUserID"></param>
        /// <returns></returns>
        public flowRole readUserFlowType(string strApplyUserID)
        {
            try
            {
                return entityFlowRoleObj.readUserFlowRole(strApplyUserID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
        
        #region 動作處理

        /// <summary>
        /// 同意
        /// </summary>
        /// <param name="strProcessType"></param>
        /// <param name="intApplyRequestID"></param>
        public void agree(string strProcessType, int intApplyRequestID)
        {
            try
            {
                entityDoAction.agree(strProcessType, intApplyRequestID);
                #region 寄發e-mail 給申請人
                //sendMail(intApplyRequestID);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void agree(string strProcessType, int intApplyRequestID, DateTime? dTimePredictCompleteDate)
        {
            try
            {
                entityDoAction.agree(strProcessType, intApplyRequestID, dTimePredictCompleteDate);

                #region 寄發e-mail 給申請人
                //sendMail(intApplyRequestID);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void sendMail(int intApplyRequestID)
        {
            
            entityRequest entityRequestData = entityDoAction.readApplyRequest(intApplyRequestID);

            if (entityRequestData.processAction.Trim() == "boss")
            {
               // flowRole flowRoleUser = entityFlowRoleObj.readUserFlowRole(entityRequestData.applyUserID);
               // controlEmail.sendMail(entityRequestData, flowRoleUser.strBossEmail);

            }else if (entityRequestData.processAction.Trim() == "rdDispatch")
            {
                //只發送給mail給RD窗口
                //flowRole flowRoleUser = entityFlowRoleObj.rdDispatchRole();
               // controlEmail.sendMail(entityRequestData, flowRoleUser);
            }
            else if (entityRequestData.processAction.Trim() == "complete" )
            {
                //flowRole flowRoleUser = entityFlowRoleObj.readUserFlowRole(entityRequestData.applyUserID); //申請人
                //發送完成預期完成通知給申請人、主管
               // sendMailBossApplyUser(entityRequestData, flowRoleUser);
            }
            else if (entityRequestData.processAction.Trim() == "reject" && entityRequestData.processName.Trim() == "主管")
            {
                //主管拒絕時，送信件給申請人
                //flowRole flowRoleUser = entityFlowRoleObj.readUserFlowRole( entityRequestData.applyUserID); //申請人
               // controlEmail.sendMail(entityRequestData, flowRoleUser);
            }
            else if (entityRequestData.processAction.Trim() == "reject" && entityRequestData.processName.Trim() == "RD處理人員")
            {
                //RD處理人員 拒絕時，送信件給申請人
                //flowRole flowRoleUser = entityFlowRoleObj.readUserFlowRole(entityRequestData.applyUserID); //申請人
                //發送完成預期完成通知給申請人、主管
                //sendMailBossApplyUser(entityRequestData, flowRoleUser);
            }
        }

        private void sendMailBossApplyUser(entityRequest entityRequestData, flowRole flowRoleUser)
        {
            try
            {
                controlEmail.sendMail(entityRequestData, flowRoleUser);

                if (!string.IsNullOrEmpty(flowRoleUser.strBossID) )
                {
                    //非主管 需另外寄送給主管一封mail 
                    flowRole flowRoleBoss = entityFlowRoleObj.readUserFlowRole(flowRoleUser.strBossID);
                    if (flowRoleBoss != null)
                    {
                        controlEmail.sendMail(entityRequestData, flowRoleBoss);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 退回/不准
        /// </summary>
        /// <param name="strProcessType"></param>
        /// <param name="intApplyRequestID"></param>
        /// <param name="strRejectReason"></param>
        public void reject(string strProcessType, int intApplyRequestID, string strRejectReason)
        {
            try
            {
                entityDoAction.reject(strProcessType, intApplyRequestID, strRejectReason);

                #region 寄發e-mail 給申請人
                //sendMail(intApplyRequestID);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// RD窗口指派人員
        /// </summary>
        /// <param name="strProcessType"></param>
        /// <param name="intApplyRequestID"></param>
        /// <param name="strRdAcceptTaskUserID"></param>
        public void rdDispatch(string strProcessType, int intApplyRequestID, string strRdAcceptTaskUserID)
        {
            try
            {
                entityDoAction.rdDispatch(strProcessType, intApplyRequestID, strRdAcceptTaskUserID);

                #region 寄發e-mail 給負責處理人員

                //entityRequest entityRequestData = entityDoAction.readApplyRequest(intApplyRequestID);
                //flowRole flowRoleUser = entityFlowRoleObj.readUserFlowRole(entityRequestData.accepTaskUser);
                //controlEmail.sendMail(entityRequestData, flowRoleUser);

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 完工
        /// </summary>
        /// <param name="strProcessType"></param>
        /// <param name="intApplyRequestID"></param>
        public void complete(string strProcessType, int intApplyRequestID)
        {
            try
            {
                entityDoAction.complete(strProcessType, intApplyRequestID);
                                
                #region 寄發e-mail 給申請人
                //entityRequest entityRequestData = entityDoAction.readApplyRequest(intApplyRequestID);
                //flowRole flowRoleUser = entityFlowRoleObj.readUserFlowRole(entityRequestData.applyUserID);

                //if (flowRoleUser == null)
                //{
                //    return;
                //}

                //sendMailBossApplyUser(entityRequestData, flowRoleUser);//預期完成通知申請人、主管
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void saveApplyRequest(string applyUser, DateTime applyDate, string title, string context)
        {
            try
            {
                
               int intApplyRequestID =  entityDoAction.saveApplyRequest(applyUser, applyDate,title, context);
               
               flowRole applyUserRole = entityFlowRoleObj.readUserFlowRole(applyUser);
               
               if (applyUserRole == null)
               {
                   return;
               }
               entityDoAction.doNextFlow(applyUserRole.strProcessType, intApplyRequestID,null);

               #region 寄發e-mail 給申請人
               //sendMail(intApplyRequestID);
               #endregion
            }
            catch (Exception ex)
            {   
                throw new Exception(ex.Message);
            }
        }

        public void edit(int applyRequestID, DateTime applyDate, string applyTitle, string applyReason)
        {
            try
            {
                entityDoAction.edit(applyRequestID, applyDate, applyTitle, applyReason);
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public void delete(int applyRequestID)
        {
            try
            {
                entityDoAction.deleteApplyRequest(applyRequestID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 讀取案件
        /// <summary>
        /// 讀取申請案件
        /// </summary>
        /// <param name="intApplyRequestID"></param>
        /// <returns></returns>
        public entityRequest readApplyRequest(int intApplyRequestID)
        {
            try
            {
                return entityDoAction.readApplyRequest(intApplyRequestID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 登入

        public int loginPass(string strAccount, string strPassword)
        {
            try
            {
                return  entityIdentity.loginPass(strAccount, strPassword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        public bool saveApplyAuthority(string userID, string bossID, string gmail, string power, string department)
        {
            try
            {
                return entityAuthorityObj.saveApplyAuthority(userID, bossID, gmail, power, department);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }   
        }


        public IEnumerable<authorityRole> listAuthorityRoles()
        {
            try
            {
                return entityAuthorityObj.listAllAuthorityRoles();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}