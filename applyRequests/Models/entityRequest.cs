using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace applyRequests.Models
{
    public class entityRequest
    {
        public int ikey
        {
            get;
            set;
        }
        public string applyType
        {
            get;
            set;
        }
        public int processFlow
        {
            get;
            set;
        }
        public string processName
        {
            get;
            set;
        }
        public string processUser
        {
            get;
            set;
        }
        public string applyUserID
        {
            get;
            set;
        }
        public string applyUserName
        {
            get;
            set;
        }
        public DateTime? requestDate
        {
            get;
            set;
        }
        public string applyReason
        {
            get;
            set;
        }
        public string accepTaskUser
        {
            get;
            set;
        }
        public string processAction
        {
            get;
            set;
        }
        public string processType
        {
            get;
            set;
        }
        public DateTime? predictCompleteDate
        {
            get;
            set;
        }
        public DateTime? completeDate
        {
            get;
            set;
        }

        public string applyTitle
        {
            get;
            set;
        }

        private TCSNewEntities tcsDB = new TCSNewEntities();
        private entityProcessFlow entityProcessFlowObj = new entityProcessFlow();  //取得流程

        private static string chineseActionName(string strActionName)
        {
           switch (strActionName.Trim())
            {
                case "complete":
                    return "申請通過";
                case "end":
                    return "已完工";
                case "reject":
                    return "退回";
               default:
                    return "審查中";
            }
        }


        public IEnumerable<entityRequest> listRequests(string applyUserID, DateTime? startDate, DateTime? endDate, string processAction, string applyReason)
        {
            try
            {
                
                var query = from p1 in tcsDB.applyRequests
                            join p2 in tcsDB.alluser on p1.applyUser equals p2.uid into userName
                            join p3 in tcsDB.alluser on p1.processUser equals p3.uid into flowUser
                            join p4 in tcsDB.applyRequestsAuthorization on p1.applyUser equals p4.userID into authorizeUser
                            from apply in userName.DefaultIfEmpty()
                            from process in flowUser.DefaultIfEmpty()
                            from authorize in authorizeUser.DefaultIfEmpty()
                            where (p1.applyUser == applyUserID || authorize.bossID == applyUserID)
                            orderby p1.requestDate descending
                             select new entityRequest
                            {
                                ikey = p1.ikey,
                                applyUserName = apply.name,
                                applyType = p1.applyType,
                                requestDate = p1.requestDate,
                                applyReason = p1.applyReason,
                                processName = p1.processName,
                                processFlow = p1.processFlow,
                                processUser = process.name,
                                accepTaskUser = p1.accepTaskUser,
                                processAction = p1.processAction,
                                applyTitle=p1.applyTitle,
                                processType = authorize.useProcess
                            };


                ////設定起迄區間
                if (startDate != null && endDate != null)
                {
                    query = query.Where(m => m.requestDate >= startDate && m.requestDate <= endDate);
                }

                //審查類別
                if (!string.IsNullOrEmpty(processAction))
                {
                    if (processAction == "other")
                    {
                        //如果是其他類別，找出審核中…所有處理狀態
                        List<string> listProcessAction = new List<string>() { "boss", "rdAcceptTaskUser", "rdDispatch", "rdAcceptTaskUser" };

                        query = query.Where(x => listProcessAction.Contains(x.processAction));
                    }
                    else
                    {
                        query = query.Where(m => m.processAction.Contains(processAction));
                    }
                }

                //申請理由
                if (!string.IsNullOrEmpty(applyReason))
                {
                    query = query.Where(m => m.applyReason.Contains(applyReason));
                }

                var results = query.Select(n => new entityRequest
                {
                    ikey = n.ikey,
                    applyUserName = n.applyUserName,
                    applyType = n.applyType,
                    requestDate = n.requestDate,
                    applyReason = n.applyReason,
                    processName = n.processName,
                    processFlow = n.processFlow,
                    processUser = n.processUser,
                    accepTaskUser = n.accepTaskUser,
                    processAction = n.processAction,
                    applyTitle=n.applyTitle,
                    processType = n.processType
                }).AsQueryable();

                return results.AsQueryable<entityRequest>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 等待簽核的案件
        /// </summary>
        /// <param name="loginUserID"></param>
        /// <returns></returns>
        public IEnumerable<entityRequest> listWaitToSignRequests(string loginUserID, DateTime? startDate, DateTime? endDate, string applyReason)
        {

            try
            {
                var query = from p1 in tcsDB.applyRequests
                            join p2 in tcsDB.alluser on p1.applyUser equals p2.uid into userName
                            join p3 in tcsDB.alluser on p1.processUser equals p3.uid into flowUser
                            from apply in userName.DefaultIfEmpty()
                            from process in flowUser.DefaultIfEmpty()
                            where p1.processUser == loginUserID && p1.completeDate ==null
                            orderby p1.requestDate ascending
                            select new entityRequest
                            {
                                ikey = p1.ikey,
                                applyUserName = apply.name,
                                applyType = p1.applyType,                                 
                                requestDate = p1.requestDate,
                                applyReason = p1.applyReason,
                                processName = p1.processName,
                                processFlow = p1.processFlow,
                                processUser = process.name,
                                accepTaskUser = p1.accepTaskUser,
                                processAction = p1.processAction,
                                applyTitle = p1.applyTitle
                            };

                ////設定起迄區間
                if (startDate != null && endDate != null)
                {
                    query = query.Where(m => m.requestDate >= startDate && m.requestDate <= endDate);
                }

                //申請理由
                if (!string.IsNullOrEmpty(applyReason))
                {
                    query = query.Where(m => m.applyReason.Contains(applyReason));
                }


                var results = query.Select(n => new entityRequest
                {
                    ikey = n.ikey,
                    applyUserName = n.applyUserName,
                    applyType = n.applyType,
                    requestDate = n.requestDate,
                    applyReason = n.applyReason,
                    processName = n.processName,
                    processFlow = n.processFlow,
                    processUser = n.processUser,
                    accepTaskUser = n.accepTaskUser,
                    processAction = n.processAction,
                    applyTitle=n.applyTitle
                }).AsQueryable();

                return results.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 已簽核過的表單
        /// </summary>
        /// <param name="loginUserID"></param>
        /// <returns></returns>
        public IEnumerable<entityRequest> listSignedRequests(string loginUserID, DateTime? startDate, DateTime? endDate, string processAction, string applyReason)
        {
            try
            {
                var query = from p1 in tcsDB.applyRequests
                            join p2 in tcsDB.alluser on p1.applyUser equals p2.uid into userName
                            join p3 in tcsDB.alluser on p1.processUser equals p3.uid into flowUser
                            from apply in userName.DefaultIfEmpty()
                            from process in flowUser.DefaultIfEmpty()
                            where  p1.completeDate !=null
                            orderby p1.requestDate ascending
                            select new entityRequest
                            {
                                ikey = p1.ikey,
                                applyUserName = apply.name,
                                applyType = p1.applyType,
                                requestDate = p1.requestDate,
                                applyReason = p1.applyReason,
                                processName = p1.processName,
                                processFlow = p1.processFlow,
                                processUser = process.name,
                                accepTaskUser = p1.accepTaskUser,
                                processAction = p1.processAction,
                                applyTitle=p1.applyTitle
                            };

                ////設定起迄區間
                if (startDate != null && endDate != null)
                {
                    query = query.Where(m => m.requestDate >= startDate && m.requestDate <= endDate);
                }

                //審查類別
                if (!string.IsNullOrEmpty(processAction))
                {
                    if (processAction == "other")
                    {
                        //如果是其他類別，找出審核中…所有處理狀態
                        List<string> listProcessAction = new List<string>() { "boss", "rdAcceptTaskUser", "rdDispatch", "rdAcceptTaskUser" };

                        query = query.Where(x => listProcessAction.Contains(x.processAction));
                    }
                    else
                    {
                        query = query.Where(m => m.processAction.Contains(processAction));
                    }
                }

                //申請理由
                if (!string.IsNullOrEmpty(applyReason))
                {
                    query = query.Where(m => m.applyReason.Contains(applyReason));
                }


                var results = query.Select(n => new entityRequest
                {
                    ikey = n.ikey,
                    applyUserName = n.applyUserName,
                    applyType = n.applyType,
                    requestDate = n.requestDate,
                    applyReason = n.applyReason,
                    processName = n.processName,
                    processFlow = n.processFlow,
                    processUser = n.processUser,
                    accepTaskUser = n.accepTaskUser,
                    processAction = n.processAction,
                    applyTitle=n.applyTitle
                }).AsQueryable();

                return results.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        /// <summary>
        /// 讀取申請案件-單筆
        /// </summary>
        /// <param name="intApplyRequestID"></param>
        /// <returns></returns>
        public entityRequest readApplyRequest(int intApplyRequestID)
        {
            try
            {   
                var query = from p1 in tcsDB.applyRequests
                            join p2 in tcsDB.alluser on p1.applyUser equals p2.uid into userName
                            join p3 in tcsDB.alluser on p1.processUser equals p3.uid into flowUser
                            join p4 in tcsDB.applyRequestsAuthorization on p1.applyUser equals p4.userID into flowGroup
                            from apply in userName.DefaultIfEmpty()
                            from process in flowUser.DefaultIfEmpty()
                            from authorize in flowGroup.DefaultIfEmpty()
                            where p1.ikey == intApplyRequestID
                            select new entityRequest
                            {
                                ikey = p1.ikey,
                                applyUserID = p1.applyUser,
                                applyUserName = apply.name,
                                applyType = p1.applyType,
                                requestDate = p1.requestDate,
                                applyReason = p1.applyReason,
                                processName = p1.processName,
                                processFlow = p1.processFlow,
                                processUser = process.name,
                                accepTaskUser = p1.accepTaskUser,
                                processType = authorize.useProcess,
                                processAction = p1.processAction,
                                predictCompleteDate=p1.predictCompleteDate,
                                completeDate = p1.completeDate,
                                applyTitle=p1.applyTitle
                            };

                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {   
                throw new Exception(ex.Message);
            }
        }

        public applyRequests readRequest(int intApplyRequestID)
        {
            try
            {
                var query = from p1 in tcsDB.applyRequests
                            where p1.ikey == intApplyRequestID
                            select p1;

                  return (applyRequests) query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public int saveApplyRequest(string strApplyUserID, DateTime dtRequestDate,string strApplyTitle, string strContext)
        {
            try
            {
                applyRequests requestSaveData = new applyRequests();
                requestSaveData.applyUser = strApplyUserID;
                requestSaveData.requestDate = dtRequestDate;
                requestSaveData.applyDate = DateTime.Now;
                requestSaveData.applyReason = HttpUtility.HtmlDecode(strContext);
                requestSaveData.processFlow = 0;
                requestSaveData.processUser = "";
                requestSaveData.applyTitle = strApplyTitle;
                
                tcsDB.applyRequests.Add(requestSaveData);
                tcsDB.SaveChanges();
                return requestSaveData.ikey;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool deleteApplyRequest(int intApplyRequestID)
        {
            try
            {
                applyRequests applyRequestObj = new applyRequests()
                {
                    ikey = intApplyRequestID
                };

                tcsDB.applyRequests.Attach(applyRequestObj);
                tcsDB.applyRequests.Remove(applyRequestObj);
                tcsDB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool edit(int intApplyRequestID, DateTime dtRequestDate, string strApplyTitle, string strContext ){
            try
            {
                applyRequests editResuest = tcsDB.applyRequests.Find(intApplyRequestID);              
                editResuest.requestDate = dtRequestDate;
                editResuest.applyDate = DateTime.Now;
                editResuest.applyReason = HttpUtility.HtmlDecode(strContext);              
                editResuest.applyTitle = strApplyTitle;
                tcsDB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #region 處理動作

        /// <summary>
        /// 主管同意/RD處理人員同意  ==> 自動往下一流程走
        /// </summary>
        /// <param name="intApplyRequestID"></param>
        public void agree(string strProcessType, int intApplyRequestID)
        {
            try
            {
                doNextFlow(strProcessType, intApplyRequestID,null);//跑下一流程
            }
            catch (Exception ex)
            {
                throw new Exception("entityRequest:agree" + ex.Message);
            }
        }

        /// <summary>
        /// 主管同意/RD處理人員同意  ==> 自動往下一流程走
        /// </summary>
        /// <param name="intApplyRequestID"></param>
        public void agree(string strProcessType, int intApplyRequestID, DateTime? dTimePredictCompleteDate)
        {
            try
            {
                doNextFlow(strProcessType, intApplyRequestID, dTimePredictCompleteDate);//跑下一流程
            }
            catch (Exception ex)
            {
                throw new Exception("entityRequest:agree" + ex.Message);
            }
        }
        
        /// <summary>
        /// 主管退回/RD處理人員退回
        /// </summary>
        /// <param name="intApplyRequestID"></param>
        public void reject(string strProcessType, int intApplyRequestID, string strRejectReason)
        {
            try
            {
                applyRequests applyRequestObj = tcsDB.applyRequests.Where(m => m.ikey == intApplyRequestID).FirstOrDefault();
                applyRequestObj.processAction = "reject";
                applyRequestObj.rejectReason = strRejectReason; //退回的理由
                applyRequestObj.completeDate = DateTime.Now;
                tcsDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("entityRequest:reject" + ex.Message);
            }
        }

        /// <summary>
        /// RD窗口指派人員==>流程自動往下送給指派人員
        /// </summary>
        /// <param name="intApplyRequestID"></param>
        public void rdDispatch(string strProcessType, int intApplyRequestID,  string strRdAcceptTaskUserID)
        {
            try
            {
                applyRequests applyRequestObj = tcsDB.applyRequests.Where(m => m.ikey == intApplyRequestID).FirstOrDefault();
                applyRequestObj.accepTaskUser = strRdAcceptTaskUserID;
                applyRequestObj.processAction = "rdDispatch";
                tcsDB.SaveChanges();

                doNextFlow(strProcessType, intApplyRequestID,null);
            }
            catch (Exception ex)
            {
                throw new Exception("entityRequest:reject" + ex.Message);
            }
        }

        /// <summary>
        /// 完工確認
        /// </summary>
        /// <param name="intApplyRequestID"></param>
        public void complete(string strProcessType, int intApplyRequestID)
        {
            try
            {
                applyRequests applyRequestObj = tcsDB.applyRequests.Where(m => m.ikey == intApplyRequestID).FirstOrDefault();
                applyRequestObj.processAction = "end";
                applyRequestObj.processName = "結束";
                applyRequestObj.completeDate = DateTime.Now;
                tcsDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 往下一個流程
        /// </summary>
        /// <param name="strProcessType"></param>
        /// <param name="applyRequestObj"></param>
        public void doNextFlow(string strProcessType, int intApplyRequestID, DateTime? dTimePredictCompleteDate)
        {
            try
            {
                var applyRequestObj = tcsDB.applyRequests.Find(intApplyRequestID);

                FlowItem nextFlowItem = entityProcessFlowObj.getFlowNext(strProcessType, applyRequestObj.processFlow);

                if (nextFlowItem == null)
                {
                    return;
                }

                applyRequestObj.processFlow = nextFlowItem.flowItemID;
                applyRequestObj.processName = nextFlowItem.flowItemName.Trim();
                applyRequestObj.processAction = nextFlowItem.flowRoleFunc.Trim();

                //儲存預期日期
                if (dTimePredictCompleteDate != null)
                {
                    applyRequestObj.predictCompleteDate = dTimePredictCompleteDate;
                }

                entityFlowRole entityFlowRoleUser = new entityFlowRole();
                flowRole flowRoleUser = entityFlowRoleUser.getUserRole(applyRequestObj.ikey, applyRequestObj.applyUser, nextFlowItem.flowRoleFunc);

                applyRequestObj.processUser = flowRoleUser.strRoleUserID.Trim();                
                tcsDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
                
       
        #endregion
    }
}